# SoundTouched

This mod allows to replace the game sounds with custom ones.

## Installation
1. Download **SoundTouched.dll** here: <https://github.com/Ale32bit/SoundTouched/releases/latest>
2. Install **BepInEx** by following [this guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html).
3. Move the DLL file into the `GAME_DIR/BepInEx/plugins/` directory where `GAME_DIR` is the directory of the game.

## How to replace sounds
You can add a custom sound by placing a WAV file in the `sounds` folder in the plugins directory.
The WAV file must be named exactly in this format `SoundName.wav`, `SoundName` is the name of the sound to replace.

**The following list contains the sounds you can replace:**

* **BgmBattleDawn**: Game music
* **BgmEpilogue**: Main menu music
* **BuffAdditionalJump**: Additional jump object
* **BuffDarknessRemover**: Darkness remover object
* **BuffForceBoost**: Jump boost object
* **BuffLiveGiver**: One extra life
* **BuffSpeedBoost**: Speed boost object
* **CheckPoint**: Checkpoint
* **Jump**: Player jump
* **KnightDeath**: Enemy death
* **KnightHit**: Enemy hit by player
* **KnightSlash**: Emeny hits player
* **MenuSelectOption**: Option selection in menus
* **PlayerConsumedByDarkness**: Death by too much darkness
* **PlayerDeath**: Generic player death
* **PlayerDimensionToNormal**: Player switches to normal dimension
* **PlayerDimensionToShadow**: Player switches to shadow dimension
* **PlayerHit**: Player got hit
* **PlayerJump**: Player jump
* **PlayerNoJumpsLeft**: No more jumps for player :(
* **PlayerStep**: Player walking step
* **PlayerSwitchDimension**: Begin switching dimension
* **PlayerSwordSlash**: Player attacks

For an up to date list of available sounds, turn on console logging in BepInEx config and look for SoundTouched logs.
