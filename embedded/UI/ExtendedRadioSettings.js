var ExtendedRadio_settingsPanelBool = false
var ExtendedRadio_settingsPanel_OpenTab = null
var ExtendedRadio_settingsPanel_SelectedTabButton = null
var ExtendedRadio_settingsPanel_TabButtonDict = {}
var ExtendedRadio_settingsPanel_TabPanelDict = {}

var ExtendedRadio_settingsPanel = null
var ExtendedRadio_settingsPanel_TabsButton = null
var ExtendedRadio_settingsPanel_Tabs = null

ExtendedRadio_CreateSettingsButton()

function ExtendedRadio_CreateSettingsButton() {
	var ExtendedRadio_settingsButton = document.createElement("button", document.getElementsByClassName("close-button_wKK")[0]);
	var ExtendedRadio_settingsButtonImage = document.createElement("div")
	ExtendedRadio_settingsButton.className = "ExtendedRadio_settingsButton button_bvQ"
	
	ExtendedRadio_settingsButton.addEventListener("click", function() {
		engine.trigger("audio.playSound", "select-item", 1);
		if(!ExtendedRadio_settingsPanelBool) {
			if(ExtendedRadio_settingsPanel == null) CreateSettingsPanel()
			ExtendedRadio_radioPanelBottom.style.display = "none"
			ExtendedRadio_settingsPanel.style.display = ""
			ExtendedRadio_settingsPanelBool = true
		} else {
			ExtendedRadio_radioPanelBottom.style.display = ""
			ExtendedRadio_settingsPanel.style.display = "none"
			ExtendedRadio_settingsPanelBool = false
		}
	})

	ExtendedRadio_settingsButton.addEventListener("mouseenter", function() {
		engine.trigger("audio.playSound", "hover-item", 1);
	})
	
	ExtendedRadio_settingsButtonImage.className = "ExtendedRadio_settingsButtonImage tinted-icon_iKo icon_PhD"
	ExtendedRadio_settingsButtonImage.style.maskImage = "url(Media/Glyphs/Gear.svg)"
	ExtendedRadio_settingsButtonImage.style.height = "24rem"
	ExtendedRadio_settingsButtonImage.style.width = "24rem"
	// ExtendedRadio_settingsButtonImage.style.backgroundColor = "var(--iconColor)"
	// ExtendedRadio_settingsButtonImage.style.maskSize = "contain"
	// ExtendedRadio_settingsButtonImage.style.maskPosition = "50% 50%"
	
	ExtendedRadio_settingsButton.appendChild(ExtendedRadio_settingsButtonImage)
	
	ExtendedRadio_radioPannelTop.insertBefore(ExtendedRadio_settingsButton, ExtendedRadio_space)
	ExtendedRadio_radioPannelTop.removeChild(ExtendedRadio_space)
}

function CreateSettingsPanel() {
	ExtendedRadio_settingsPanel = document.createElement("div", ExtendedRadio_radioPanelBottom)

	ExtendedRadio_settingsPanel_Tabs = document.createElement("div")
	ExtendedRadio_settingsPanel_Tabs.className = "ExtendedRadio_settingsPanel_Tabs" //detail_eTp
	ExtendedRadio_settingsPanel_Tabs.style.overflowY = "auto"
	ExtendedRadio_settingsPanel_Tabs.style.height = "100%"
	ExtendedRadio_settingsPanel_Tabs.style.width = "80%"
	ExtendedRadio_settingsPanel_Tabs.style.display = "flex"
	ExtendedRadio_settingsPanel_Tabs.style.flexDirection = "column"
	// ExtendedRadio_settingsPanel_Tabs.style.backgroundColor = "rgba(0,0,255, 0.2)"

	ExtendedRadio_settingsPanel_TabsButton = document.createElement("div")
	ExtendedRadio_settingsPanel_TabsButton.className = "ExtendedRadio_settingsPanel_TabsButton" //menu_hb1
	ExtendedRadio_settingsPanel_TabsButton.style.height = "100%"
	ExtendedRadio_settingsPanel_TabsButton.style.width = "20%"
	ExtendedRadio_settingsPanel_TabsButton.style.overflowY = "auto"
	ExtendedRadio_settingsPanel_TabsButton.style.borderRightStyle = "solid"
	ExtendedRadio_settingsPanel_TabsButton.style.borderRightWidth = "var(--stroke2)"
	ExtendedRadio_settingsPanel_TabsButton.style.borderRightColor = "rgba(0, 0, 0, 0.100000)"
	ExtendedRadio_settingsPanel_TabsButton.style.display = "flex"
	ExtendedRadio_settingsPanel_TabsButton.style.flexDirection = "column"
	// ExtendedRadio_settingsPanel_TabsButton.style.backgroundColor = "rgba(0,0,255, 0.2)"

	ExtendedRadio_CreateSettingsCat("Global")
	ExtendedRadio_CreateSettingsCatToggle("Global", "Disable ads on startup", "extended_radio_settings.DisableAdsOnStartup")
	ExtendedRadio_CreateSettingsCatToggle("Global", "Load last radio on startup", "extended_radio_settings.SaveLastRadio")
	ExtendedRadio_CreateSettingsCatButton("Global", "Reload Radio", "extended_radio.reloadradio")

	ExtendedRadio_CreateSettingsCat("UI")
	ExtendedRadio_CreateSettingsCatToggle("UI", "Custom network UI", "extended_radio_settings.customnetworkui")

	ExtendedRadio_SetDefaultOpenSettingsPanel("Global")

	// ExtendedRadio_settingsPanel_TabPanelDict["UI"].innerHTML = getterValue("extended_radio_settings.customsetworkui")

	ExtendedRadio_settingsPanel.className = "ExtendedRadio_settingsPanel base_V9Q " + ExtendedRadio_radioPanelBottom.className
	ExtendedRadio_settingsPanel.style.display = "none"
	ExtendedRadio_settingsPanel.style.flexDirection = "row"

	ExtendedRadio_settingsPanel.appendChild(ExtendedRadio_settingsPanel_TabsButton)
	ExtendedRadio_settingsPanel.appendChild(ExtendedRadio_settingsPanel_Tabs)

	ExtendedRadio_radioPanel.appendChild(ExtendedRadio_settingsPanel)
}

function ExtendedRadio_CreateSettingsCat(name) {
	var ExtendedRadio_settingsPanel_Tabs_catButton = document.createElement("button")
	var ExtendedRadio_settingsPanel_Tabs_catPanel = document.createElement("div")

	ExtendedRadio_settingsPanel_Tabs_catButton.className = name
	ExtendedRadio_settingsPanel_Tabs_catPanel.className = name

	ExtendedRadio_settingsPanel_TabButtonDict[name] = ExtendedRadio_settingsPanel_Tabs_catButton
	ExtendedRadio_settingsPanel_TabPanelDict[name] = ExtendedRadio_settingsPanel_Tabs_catPanel

	ExtendedRadio_setupTab(ExtendedRadio_settingsPanel_Tabs_catPanel)
	ExtendedRadio_setupTabButton(ExtendedRadio_settingsPanel_Tabs_catButton, ExtendedRadio_settingsPanel_Tabs_catPanel)
}

function ExtendedRadio_setupTabButton(tabButton) {

	tabButton.innerHTML = tabButton.className
	tabButton.className += "ExtendedRadio_settingsTab_" + tabButton.className + " station-item_cOt" //item_pq7
	tabButton.style.width = "100%"
	tabButton.style.height = "auto"
	tabButton.style.display = "flex"

	tabButton.addEventListener("click", function() {

		engine.trigger("audio.playSound", "select-item", 1);
		ExtendedRadio_settingsPanel_SelectedTabButton.classList.remove('selected');
		ExtendedRadio_settingsPanel_SelectedTabButton = tabButton
		ExtendedRadio_settingsPanel_SelectedTabButton.className = "selected " + ExtendedRadio_settingsPanel_SelectedTabButton.className

		ExtendedRadio_settingsPanel_OpenTab.style.display = "none"
		ExtendedRadio_settingsPanel_OpenTab = ExtendedRadio_settingsPanel_TabPanelDict[tabButton.innerHTML]
		ExtendedRadio_settingsPanel_OpenTab.style.display = "flex"

	})

	tabButton.addEventListener("mouseenter", function() {
		engine.trigger("audio.playSound", "hover-item", 1);
	})

	ExtendedRadio_settingsPanel_TabsButton.appendChild(tabButton)
}

function ExtendedRadio_setupTab(tab) {

	tab.className = "ExtendedRadio_settingsTab_" + tab.className
	tab.style.width = "100%"
	tab.style.height = "auto"
	tab.style.display = "none"
	tab.style.flexDirection = "column"

	ExtendedRadio_settingsPanel_Tabs.appendChild(tab)

}

function ExtendedRadio_SetDefaultOpenSettingsPanel(name) {

	ExtendedRadio_settingsPanel_OpenTab = ExtendedRadio_settingsPanel_TabPanelDict[name]
	ExtendedRadio_settingsPanel_OpenTab.style.display = "flex"

	ExtendedRadio_settingsPanel_SelectedTabButton = ExtendedRadio_settingsPanel_TabButtonDict[name]
	ExtendedRadio_settingsPanel_SelectedTabButton.className = "selected " + ExtendedRadio_settingsPanel_SelectedTabButton.className
}

function ExtendedRadio_CreateSettingsCatToggle(panelname, name, event) {

	var ExtendedRadio_settingsContainer = document.createElement("div")
	var ExtendedRadio_settingsToggleLabel = document.createElement("div")
	var ExtendedRadio_settingsToggle = document.createElement("div")
	var ExtendedRadio_settingsToggleCheckmark = document.createElement("div")

	ExtendedRadio_settingsContainer.className = "station-item_cOt field_MBO field_UuC"
	ExtendedRadio_settingsToggleLabel.className = "label_DGc label_ZLb"
	ExtendedRadio_settingsToggle.className = "toggle_cca item-mouse-states_Fmi toggle_th_"
	ExtendedRadio_settingsToggleCheckmark.className = "checkmark_NXV"

	ExtendedRadio_settingsToggle.style.setProperty("--checkmarkColor", "rgba(80, 76, 83, 1.000000)")
	ExtendedRadio_settingsToggle.style.alignItems = "flex-end"
	ExtendedRadio_settingsToggle.style.backgroundColor = "rgba(236, 236, 236, 1.000000)"
	ExtendedRadio_settingsToggleLabel.innerHTML = name

	ExtendedRadio_settingsContainer.addEventListener("click", function () {
		// ExtendedRadio_Check(ExtendedRadio_settingsContainer.childNodes[1], !ExtendedRadio_isCheck(ExtendedRadio_settingsContainer.childNodes[1]))
		engine.trigger("audio.playSound", "select-item", 1);
		engine.trigger(event, !ExtendedRadio_isCheck(ExtendedRadio_settingsContainer.childNodes[1]))
	})

	ExtendedRadio_settingsContainer.addEventListener("mouseenter", function() {
		engine.trigger("audio.playSound", "hover-item", 1);
	})

	ExtendedRadio_settingsContainer.appendChild(ExtendedRadio_settingsToggleLabel)
	ExtendedRadio_settingsContainer.appendChild(ExtendedRadio_settingsToggle)
	ExtendedRadio_settingsToggle.appendChild(ExtendedRadio_settingsToggleCheckmark)
	
	ExtendedRadio_getterValue(event, ExtendedRadio_settingsToggle, ExtendedRadio_Check)

	// ExtendedRadio_Check(ExtendedRadio_settingsToggle, )

	ExtendedRadio_settingsPanel_TabPanelDict[panelname].appendChild(ExtendedRadio_settingsContainer)
}

function ExtendedRadio_CreateSettingsCatButton(panelname, name, event) {

	var ExtendedRadio_settingsContainer = document.createElement("div")
	var ExtendedRadio_settingsButton = document.createElement("button")

	ExtendedRadio_settingsContainer.className = "buttons_hd7"
	ExtendedRadio_settingsButton.className = "button_WWa button_SH8"

	ExtendedRadio_settingsButton.style.backgroundColor = "rgba(255, 255, 255, 1)"
	ExtendedRadio_settingsButton.style.borderColor = "rgba(212, 23, 23, 1)"
	ExtendedRadio_settingsButton.style.color = "rgba(212, 23, 23, 1)"
	ExtendedRadio_settingsButton.innerHTML = name

	ExtendedRadio_settingsButton.addEventListener("click", function () {
		engine.trigger("audio.playSound", "select-item", 1);
		engine.trigger(event)
	})

	ExtendedRadio_settingsButton.addEventListener("mouseover", function () {

		this.style.backgroundColor = "rgba(212, 23, 23, 1)"
		this.style.color = "rgba(255, 255, 255, 1)"

	})

	ExtendedRadio_settingsButton.addEventListener("mouseout", function () {

		this.style.backgroundColor = "rgba(255, 255, 255, 1)"
		this.style.color = "rgba(212, 23, 23, 1)"

	})

	ExtendedRadio_settingsButton.addEventListener("mouseenter", function() {
		engine.trigger("audio.playSound", "hover-item", 1);
	})

	ExtendedRadio_settingsContainer.appendChild(ExtendedRadio_settingsButton)

	ExtendedRadio_settingsPanel_TabPanelDict[panelname].appendChild(ExtendedRadio_settingsContainer)
}

function ExtendedRadio_Check(element, bool) {
	if(bool) {
		element.classList.remove("unchecked")
		element.classList.add("checked")
		element.childNodes[0].classList.remove("unchecked")
		element.childNodes[0].classList.add("checked")
	} else {
		element.classList.remove("checked")
		element.classList.add("unchecked")
		element.childNodes[0].classList.remove("checked")
		element.childNodes[0].classList.add("unchecked")
	}
}

function ExtendedRadio_isCheck(element) {
	return element.classList.contains("checked")
}