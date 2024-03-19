var ExtendedRadio_networksContainer = document.createElement("div")

ExtendedRadio_stationsMenu.style.width = "325rem" 
ExtendedRadio_stationsMenu.style.flexDirection = "row"

// ExtendedRadio_networksContainer.className = "scrollable_DXr"
ExtendedRadio_networksContainer.style.width = "23%"
ExtendedRadio_networksContainer.style.height = "100%"
ExtendedRadio_networksContainer.style.overflowY = "auto"
ExtendedRadio_networksContainer.style.borderRightStyle = "solid"
ExtendedRadio_networksContainer.style.borderRightWidth = "var(--stroke2)"
ExtendedRadio_networksContainer.style.borderRightColor = "rgba(0, 0, 0, 0.100000)"

ExtendedRadio_networks.style.flexDirection = "column"
ExtendedRadio_networks.style.maxHeight = "100%"
ExtendedRadio_networks.style.width = "100%"
ExtendedRadio_networks.style.justifyContent = "flex-start"
ExtendedRadio_networks.style.borderBottomStyle = "none"
ExtendedRadio_networks.style.borderBottomWidth = "0"
ExtendedRadio_networks.style.borderBottomColor = null
ExtendedRadio_networks.style.marginTop = "10rem"
ExtendedRadio_networks.style.marginBottom = "10rem"

ExtendedRadio_stations.style.setProperty("--contentPaddingVertical", "0") // = "--contentPaddingVertical : 0"

ExtendedRadio_programs.style.setProperty("--contentPaddingHorizontal", "24rem") // = "--contentPaddingHorizontal : 24rem"

// networkbuttons.style.marginLeft = "0px"

document.getElementsByClassName("network-item_tGo").forEach(element => {
    element.style.marginLeft = "0px"
});

ExtendedRadio_networksContainer.appendChild(ExtendedRadio_networks)
// ExtendedRadio_CreateScrollBar(ExtendedRadio_networksContainer)
// ExtendedRadio_networksContainer.appendChild(bar)
ExtendedRadio_stationsMenu.insertBefore(ExtendedRadio_networksContainer, ExtendedRadio_stations)



// mask-image: url(Media/Glyphs/Gear.svg); 