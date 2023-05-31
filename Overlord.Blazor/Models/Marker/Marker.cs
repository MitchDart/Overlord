using Overlord.Blazor.Utils;

namespace Overlord.Blazor.Models.Marker;

public class Marker
{
    private bool _isChanged;
    private string _icon;
    private int _size;
    private LatLon _latLon;
    private Action<Marker>? _onLocationChanged;

    public Marker(string icon, int size, LatLon latLon)
    {
        _icon = icon;
        _size = size;
        _latLon = latLon;
    }

    public void SetLocationChangedEventHandler(Action<Marker> onLocationChanged)
    {
        _onLocationChanged = onLocationChanged;
    } 

    public LatLon LatLon
    {
        get => _latLon;
        set
        {
            _latLon = value;
            _onLocationChanged?.Invoke(this);
        }
    }

    public bool IsChanged
    {
        get => _isChanged;
        set => _isChanged = value;
    }

    public string Icon
    {
        get => _icon;
        set => _icon = value;
    }

    public int Size
    {
        get => _size;
        set => _size = value;
    }
}