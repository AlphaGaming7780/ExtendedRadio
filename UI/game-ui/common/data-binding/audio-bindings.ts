import { getModule } from "cs2/modding"

const path$ = "game-ui/common/data-binding/audio-bindings.ts"

let uiSound: UISound | null = null

export let UISound: UISound = uiSound === null ? uiSound = getUISound() : uiSound;

type UISound = {
    selectItem: string,
    dragSlider: string,
    hoverItem: string,
    expandPanel: string,
    grabSlider: string,
    selectDropdown: string,
    selectToggle: string,
    focusInputField: string,
    signatureBuildingEvent: string,
    bulldoze: string,
    bulldozeEnd: string,
    relocateBuilding: string,
    mapTilePurchaseMode: string,
    mapTilePurchaseModeEnd: string,
    xpEvent: string,
    milestoneEvent: string,
    economy: string,
    chirpEvent: string,
    likeChirp: string,
    chirper: string,
    purchase: string,
    enableBuilding: string,
    disableBuilding: string,
    pauseSimulation: string,
    resumeSimulation: string,
    simulationSpeed1: string,
    simulationSpeed2: string,
    simulationSpeed3: string,
    togglePolicy: string,
    takeLoan: string,
    removeItem: string,
    toggleInfoMode: string,
    takePhoto: string,
    tutorialTriggerCompleteEvent: string,
    selectRadioNetwork: string,
    selectRadioStation: string,
    generateRandomName: string,
    decreaseElevation: string,
    increaseElevation: string,
    selectPreviousItem: string,
    selectNextItem: string,
    openPanel: string,
    closePanel: string,
    openMenu: string,
    closeMenu: string,
}


export function getUISound(): UISound {
    return getModule(path$, "UISound")
}