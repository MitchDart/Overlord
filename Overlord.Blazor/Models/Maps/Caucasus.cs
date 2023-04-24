using Overlord.Blazor.Utils;

namespace Overlord.Blazor.Models.Maps;

public class Caucasus : Map
{
    public override LatLon TopLeftCorner { get; set; } = new(47.212, 26.455);
    public override LatLon BottomRightCorner { get; set; } = new(39.893, 44.879);
    public override string Name { get; set; } = "Caucasus";
}