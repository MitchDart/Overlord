window.milsymbolInterop = {
    getSymbol: function(name, size) {
        return (new ms.Symbol(name, {size: size})).asCanvas().toDataURL();
    }
}