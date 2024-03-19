var ExtendedRadio_radioPanel = document.getElementsByClassName("radio-panel_rXp")[0]

var ExtendedRadio_radioPannelTop = document.getElementsByClassName("title-bar_PF4")[0]
var ExtendedRadio_space = document.getElementsByClassName("icon-space_h_f")[0]

var ExtendedRadio_radioPanelBottom = document.getElementsByClassName("content_r9x")[0]
var ExtendedRadio_stationsMenu = document.getElementsByClassName("stations-menu_kAr")[0]
var ExtendedRadio_networks = document.getElementsByClassName("networks_SQ5")[0]
var ExtendedRadio_stations = document.getElementsByClassName("stations_mU1")[0]
var ExtendedRadio_programs = document.getElementsByClassName("list_Kl3")[0]

function ExtendedRadio_getterValue(event, element, onUpdate) {
    const updateEvent = event + ".update"
    const subscribeEvent = event + ".subscribe"
    const unsubscribeEvent = event + ".unsubscribe"
    
    var sub = engine.on(updateEvent, (data) => {
        element && onUpdate(element, data)
    })

    engine.trigger(subscribeEvent)
    return () => {
        engine.trigger(unsubscribeEvent)
        sub.clear()
    };
}

function ExtendedRadio_CreateScrollBar(element) {

    var ExtendedRadio_ScrollBarTop = document.createElement("div")
    var ExtendedRadio_ScrollBarBot = document.createElement("div")

    ExtendedRadio_ScrollBarTop.className = "track_e3O y_SMM"
    ExtendedRadio_ScrollBarBot.className = "thumb_Cib y_SMM"

    ExtendedRadio_ScrollBarTop.appendChild(ExtendedRadio_ScrollBarBot)
    element.appendChild(ExtendedRadio_ScrollBarTop)

}
