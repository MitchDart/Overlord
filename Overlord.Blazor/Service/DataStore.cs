using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using RurouniJones.Dcs.Grpc.V0.Common;

namespace Overlord.Blazor.Service;

public class DataStore
{
    private object _lock = new object();
    private ConcurrentDictionary<uint, Unit> _unitData = new ConcurrentDictionary<uint, Unit>();
    private Subject<UnitChangedEventArgs> _unitChangedSubject = new Subject<UnitChangedEventArgs>();

    public IObservable<IList<UnitChangedEventArgs>> SubscribeAndLoadInitialUnitData(out IDictionary<uint, Unit> initialUnitData)
    {
        lock (_lock)
        {
            initialUnitData = new Dictionary<uint, Unit>(_unitData);
            return _unitChangedSubject.AsObservable()
                .Buffer(TimeSpan.FromMilliseconds(1000))
                .Where(x => x.Any());
        }
    }

    public void AddUnitChangedEvent(Unit unit, UnitChangedEventArgs.UnitChangeType unitChangeType)
    {
        lock (_lock)
        {
            if (unitChangeType == UnitChangedEventArgs.UnitChangeType.Removed)
            {
                if (!_unitData.ContainsKey(unit.Id)) return;
                _unitData.Remove(unit.Id, out _);
                _unitChangedSubject.OnNext(new UnitChangedEventArgs(unit, UnitChangedEventArgs.UnitChangeType.Removed));

                return;
            }
            
            _unitData[unit.Id] = unit;
            
            //Check if unit is already in the list
            _unitChangedSubject.OnNext(_unitData.ContainsKey(unit.Id)
                ? new UnitChangedEventArgs(unit, UnitChangedEventArgs.UnitChangeType.Updated)
                : new UnitChangedEventArgs(unit, UnitChangedEventArgs.UnitChangeType.Created));
        }
    } 
}