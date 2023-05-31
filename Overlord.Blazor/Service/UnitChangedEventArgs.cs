using RurouniJones.Dcs.Grpc.V0.Common;

namespace Overlord.Blazor.Service;

public class UnitChangedEventArgs : EventArgs
{
    public Unit Unit { get; }
    public UnitChangeType UnitChange { get; }

    public UnitChangedEventArgs(Unit unit, UnitChangeType unitChange)
    {
        Unit = unit;
        UnitChange = unitChange;
    }

    public enum UnitChangeType
    {
        Created,
        Updated,
        Removed
    }
}