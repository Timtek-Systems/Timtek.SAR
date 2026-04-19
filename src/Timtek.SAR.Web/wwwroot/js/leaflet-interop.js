window.leafletInterop = {
    maps: {},

    initialize: function (elementId, lat, lng, zoom) {
        if (this.maps[elementId]) {
            this.maps[elementId].remove();
        }

        var map = L.map(elementId).setView([lat, lng], zoom || 13);

        var osmLayer = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 19
        });

        var satelliteLayer = L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', {
            attribution: '&copy; Esri, Maxar, Earthstar Geographics',
            maxZoom: 19
        });

        var terrainLayer = L.tileLayer('https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://opentopomap.org">OpenTopoMap</a> contributors',
            maxZoom: 17
        });

        osmLayer.addTo(map);

        L.control.layers({
            'Street': osmLayer,
            'Satellite': satelliteLayer,
            'Terrain': terrainLayer
        }).addTo(map);

        this.maps[elementId] = map;
        return true;
    },

    addMarker: function (elementId, lat, lng, popupText) {
        var map = this.maps[elementId];
        if (!map) return false;

        var marker = L.marker([lat, lng]).addTo(map);
        if (popupText) {
            marker.bindPopup(popupText);
        }
        return true;
    },

    setView: function (elementId, lat, lng, zoom) {
        var map = this.maps[elementId];
        if (!map) return false;

        map.setView([lat, lng], zoom || 13);
        return true;
    },

    addDraggableMarker: function (elementId, lat, lng, dotNetRef) {
        var map = this.maps[elementId];
        if (!map) return false;

        var marker = L.marker([lat, lng], { draggable: true }).addTo(map);
        marker.on('dragend', function (e) {
            var position = marker.getLatLng();
            dotNetRef.invokeMethodAsync('OnMarkerDragged', position.lat, position.lng);
        });

        map.on('click', function (e) {
            marker.setLatLng(e.latlng);
            dotNetRef.invokeMethodAsync('OnMarkerDragged', e.latlng.lat, e.latlng.lng);
        });

        return true;
    },

    invalidateSize: function (elementId) {
        var map = this.maps[elementId];
        if (!map) return false;

        map.invalidateSize();
        return true;
    },

    dispose: function (elementId) {
        var map = this.maps[elementId];
        if (map) {
            map.remove();
            delete this.maps[elementId];
        }
    }
};
