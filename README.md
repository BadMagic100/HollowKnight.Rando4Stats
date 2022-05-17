# Rando4Stats

![GitHub Build Workflow Status](https://img.shields.io/github/workflow/status/BadMagic100/HollowKnight.Rando4Stats/Build)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/BadMagic100/HollowKnight.Rando4Stats)
![Downloads GitHub all releases](https://img.shields.io/github/downloads/BadMagic100/HollowKnight.Rando4Stats/total)
![Lines of Code](https://tokei.rs/b1/github/BadMagic100/HollowKnight.Rando4Stats)

Rando4Stats (better known as RandoStats) is a Hollow Knight mod used with Randomizer 4 on patch 1.5. It provides more detailed stats about all the things you've
done on the completion screen after the credits, and various quality-of-life changes to make it easier to view and share your stats.

# Compatibility

Rando4Stats is compatible with Hollow Knight 1.5 and the current versions of Randomizer 4 and Itemsync on Scarab. If for some reason you're looking for Rando3/1.4 compatibility,
you might be interested in [Rando3Stats](https://github.com/BadMagic100/HollowKnight.Rando3Stats) (but you should really really update because Rando4 is cool). This mod does not
work on vanilla saves.

# Feature list

* Prevents mashing through the completion screen - replaces press to skip with a 1.5 second hold.
* Allows players to Ctrl+C to copy completion statistics to the clipboard, with custom formatting available via global settings (mod menu coming soon (TM)).
  I'll also add better explanation on the formatting rules later. If you want to set up custom formatting in the meanwhile, feel free to discuss it with me and
  I'd be glad to help.
* Allows players to skip to the completion screen at any time from the pause manu (intended for 3L or lockout races). Uses the button in the bottom right
  above BingoUI or the hotkey <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>C</kbd>.
* Customizable layouts - choose which stats you want to show and where to display them on the end screen via global settings (mod menu coming soon (TM))
* Available stats
  * **Randomized locations checked**
    This stat is the number of randomized locations that you've previewed and/or cleared. There is a stat for total locations checked, which are also
    subcategorized by the check's pool group and map area.
  * **Randomized items obtained**
    This stat is the number of randomized items you picked up. There is a stat for total items obtained, which is also subcategorized by the item's pool
    group.
  * **Randomized transitions visited**
    This stat is the number of randomized transitions you've gone through. There is a stat for total visited transitions, which is also subcategorized by
    the source transition's map area. In coupled transition rando, going through A -> B also counts both A and B as visited if the transition isn't a one-way.
    In uncoupled transition rando, going through A -> B only counts A as visited.

If you have any feature requests, feel free to ask in #rando-general in the HKSC discord or #badmagic-mods on the HK Modding discord.

# Installation

Use Scarab. If for some reason you can't use Scarab, links to the required dependencies are listed in [ModDependencies.txt](Rando4Stats/ModDependencies.txt).

# Acknowledgements

Modding is hard. I owe thanks to several folks for making this possible:

* The HKSC discord for inspiring the idea and generally being a cool place.
* Homothety, Flib, and the rest of the HK Modding discord for helping with various modding issues and getting integrated with Rando4.
* Phenomenol for some borrowed code and help with the previous version of RandoStats.