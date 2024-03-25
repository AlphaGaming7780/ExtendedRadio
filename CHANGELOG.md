# 1.1.2
* Fix a bug where Icon and local radio aren't loaded.
* Optimization

# 1.1.1
* Fix a bug where radio in the `ModsData/ExtendedRadio` folder aren't loaded.

# 1.1.0
- Update the mod for the new game version.
- Change how RadioMod are loadded.
- Update the wiki.

# 1.0.0
- Now can load custom radio without DLL
- Update the wiki for the 1.0.0
- PDX mods related changes
    - Update the mod to support PDX mods
    - Update the UI to use the official way
    - Settings is now in the game settings

# 0.5.3
- Add Settings: Load last radio on startup

# 0.5.2
- Add: Sounds to the new UI
- Update: Credit

# 0.5.1
- Update: Readme
- Fixed: Ads not loading

# 0.5.0
> [!IMPORTANT]  
> This version remove the support for the old API

- New UI
- Add settings UI
- Add settings.json in `ExtendedRadio-mods` folder
- Compatible with HookUi
- Fixed bug

# 0.4.0
> [!IMPORTANT]  
This version of the mod may break some mods, I made a temporary fix for the mods while the mod creators update them. This temporary fix will be removed in the next update. I can leave this temporary fix longer if necessary, if you ask me

- New Feature : [Radio Add-ons](https://github.com/AlphaGaming7780/ExtendedRadio/wiki/RadioAddons)
- New: API for [Radio Add-ons](https://github.com/AlphaGaming7780/ExtendedRadio/wiki/RadioAddons) (`ExtendedRadio.RadioAddons`)
- Change: The class name for CustomRadios (Before you call API for CustomRadios using `ExtendedRadio.ExtendedRadio.methode();`, now it's `ExtendedRadio.CustomRadios.methode();`)
- Change: The JSON for [RadioNetwork](https://github.com/AlphaGaming7780/ExtendedRadio/wiki/Radio-Elements#radio-network) and [RadioChannel](https://github.com/AlphaGaming7780/ExtendedRadio/wiki/Radio-Elements#radio-channel)
- Updated the wiki for 0.4.0

# 0.3.4
- Fixed: The support for ADS clip.
- Fixed: Failed to add to the mod database.

# 0.3.3
- Fixed: Bug

# 0.3.2
- Update: Readme.
- Fixed: Artist not loaded corectly.

# 0.3.1
- Update: Readme.

# 0.3.0
- Add: A new folder can be used for Custom Radio outside of the mod folder to avoid is deletion when the mod update on Thunderstore Mod Manager. (`BepInEx/plugins/ExtendedRadio_mods/CustomRadios`)
- Add: Complete new way to create radio, that support a lot of customization.
- Add: The API is finally here, you can now create radio using mod.
- Update: Audio file is now loaded when the radio need it (Tanks to [DragonOfMercy](https://github.com/dragonofmercy))
- Update: The wiki is update for the 0.3.0 version of the mod.
- Fixed: Multiple bug.

# 0.2.1
- Fixed: Changelog
- add link to the wiki
- And add waring to backup before updating the mod.

# 0.2.0
- Feature: Now the metadata of audio files is loaded and display correctly.
- Feature: Audio files are now played randomly inside the radio.
- Fixed: Now if two radios have the same name, they don't bug.

# 0.1.1
- Fixed: Create the `CustomRadio` folder at the start of the game.
- Fixed (Maybe): The resources folder is removed when installing with the Thunderstore mod manager (Spoiler Alerte Didn't work)

# 0.1.0
- Initial Release