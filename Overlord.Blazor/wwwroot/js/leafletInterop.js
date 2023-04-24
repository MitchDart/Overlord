window.leafletInterop = {
    initializeMap: function (element) {
        const map = L.map(element, {
            minZoom: 7,
            maxZoom: 10
        }).setView([13.029, 38.057], 7);
        L.tileLayer('https://tile.thunderforest.com/landscape/{z}/{x}/{y}.png?apikey=e282c8b16fde4775941c2a773c131a03', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);
        return map;
    },
    addMarker: function (map, lat, lng, iconCode, size) {
        var icon = L.icon({
            iconUrl: (new ms.Symbol(iconCode, {size: size})).toDataURL(),
            iconSize: [size, size],
            iconAnchor: [size/2, size/2],
            popupAnchor: [0, -size/2]
        });
        const marker = L.marker([lat, lng], {
            icon: icon,
            draggable: true
        }).addTo(map);
        if (iconCode) {
            marker.bindPopup(iconCode);
        }
        return marker._leaflet_id;
    },
    setMaxBounds: function (map, corner1lat, corner1lon, corner2lat, corner2lon) {
        var corner1 = L.latLng(corner1lat, corner1lon),
            corner2 = L.latLng(corner2lat, corner2lon),
            bounds = L.latLngBounds(corner1, corner2);
        map.setMaxBounds(bounds)
    },
    fitBounds: function (map, corner1lat, corner1lon, corner2lat, corner2lon) {
        var corner1 = L.latLng(corner1lat, corner1lon),
            corner2 = L.latLng(corner2lat, corner2lon),
            bounds = L.latLngBounds(corner1, corner2);
        map.fitBounds(bounds)
    }
};