using Overlord.Blazor.Utils;

namespace Overlord.Blazor.Models.Maps;

public abstract class Map
{
    public abstract LatLon TopLeftCorner { get; set; }
    public abstract LatLon BottomRightCorner { get; set; }
    public abstract string Name { get; set; }
}