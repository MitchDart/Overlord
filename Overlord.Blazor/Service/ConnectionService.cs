using Grpc.Core;
using Grpc.Net.Client;
using RurouniJones.Dcs.Grpc.V0.Common;
using RurouniJones.Dcs.Grpc.V0.Mission;

namespace Overlord.Blazor.Service;

public class ConnectionService : IDisposable
{
    private readonly ILogger<ConnectionService> _logger;
    private readonly DataStore _dataStore;
    private GrpcChannel? _channel;
    private MissionService.MissionServiceClient? _missionServiceClient;

    public ConnectionService(ILogger<ConnectionService> logger, DataStore dataStore)
    {
        _logger = logger;
        _dataStore = dataStore;
    }

    public void Start()
    {
        _logger.LogInformation("Starting connection service thread...");
        Task.Run(async () =>
        {
            try
            {
                await BeginStream();
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Connection service crashed!");
                _logger.LogInformation("Will attempt reconnection in 10 seconds...");
                await Task.Delay(10000);
                Start();
            }
        });
    }

    private async Task BeginStream()
    {
        _logger.LogInformation("Connecting to gRPC server...");
        _channel = GrpcChannel.ForAddress("http://127.0.0.1:50051");
        _missionServiceClient = new MissionService.MissionServiceClient(_channel);

        var stream = _missionServiceClient.StreamUnits(new StreamUnitsRequest() {Category = GroupCategory.Airplane, PollRate = 1}).ResponseStream.ReadAllAsync();

        await foreach (var streamEvent in stream)
        {
            _logger.LogInformation("Got event from DCS");
            switch (streamEvent.UpdateCase)
            {
                case StreamUnitsResponse.UpdateOneofCase.None:
                    break;
                case StreamUnitsResponse.UpdateOneofCase.Unit:
                    _dataStore.AddUnitChangedEvent(streamEvent.Unit, UnitChangedEventArgs.UnitChangeType.Updated);
                    break;
                case StreamUnitsResponse.UpdateOneofCase.Gone:
                    _dataStore.AddUnitChangedEvent(streamEvent.Unit, UnitChangedEventArgs.UnitChangeType.Removed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
    }
}