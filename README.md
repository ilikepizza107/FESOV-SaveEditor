# Fire Emblem Echoes: Shadows of Valentia Save Editor

### Features
  * Edit every character's class, inventory, and stats
  * Edit the convoy's contents
  * Edit both protagonists' silver and gold marks
  * Remove or add units, including bosses and other normally un-recruitable units
  * Remove or add items in the convoy

  * *Note on adding units*:
    * Units default to being added right after Alm or right after Celica, depending on if the unit you've selected is after Alm or after Celica. If you have no unit selected, it defaults to adding the unit after Alm
  * *Note on adding items*:
    * Items default to being added at the end of Alm's convoy or at the end of Celica's convoy, depending on if the item you've selected is in Alm's or Celica's convoy. If you have no item selected, it defaults to adding the item in Alm's convoy
  
### How to use
  * Dump save file using a save manager (e.g. Checkpoint) Make a backup just in case

From here, you can either:
  * Run program, Click File -> Open, select the file and edit stuff

Or:
  * Use FEST to decompress the save file to get a "Chapter_dec" file. https://github.com/RainThunder/FEST
  * Run program, Click File -> Open, select the decompressed file and edit stuff
  * Once finished, Click File -> Save and compress the file using FEST

### Resources

  * Hex Values of some items from these people: https://serenesforest.net/forums/index.php?/topic/71279-shadows-of-valentia-save-editingsave-bank/

  * Info on character blocks: https://gbatemp.net/threads/research-fe-sov-save-discussion.471890/

  * Serenes Forest for character base and max stats: https://serenesforest.net/fire-emblem-echoes-shadows-valentia/
 
  * Fire Emblem Wiki for Overclass max stat changes: https://fireemblemwiki.org/wiki/Class_change/Nintendo_3DS_games#Fire_Emblem_Echoes:_Shadows_of_Valentia

  * Inspired by FeTwiddler by Soaprman: https://github.com/Soaprman/FEFTwiddler 

  * Extended WPFToolkit for the numeric up downs: https://github.com/xceedsoftware/wpftoolkit

  * Dark and Light mode themes modified from here: https://github.com/AngryCarrot789/WPFDarkTheme
  
### Building

To build from the CLI, change directory to the ./FESOVSE folder, then enter `dotnet publish -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true`

An .exe will be built to ./FESOVSE/bin/Release/net8.0-windows/win-x64/publish/
