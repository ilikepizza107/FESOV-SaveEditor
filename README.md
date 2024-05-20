# A Fire Emblem Echoes: Shadows of Valentia Save Editor

### Features
  * Edit every character's class, inventory, and stats
  * Edit the convoy's contents
  
### How to use
  * Dump save file using a save manager (e.g. Checkpoint) Make a backup just in case

From here, you can either:
  * Run program, Click File -> Open, select the file and edit stuff

Or:
  * Use FEST to decompress the save file to get a "Chapter_dec" file. https://github.com/RainThunder/FEST
  * Run program, Click File -> Open, select the decompressed file and edit stuff
  * Once finished, Click File -> Save and compress the file using FEST

### Resources

  * Hex Values of items From these people:
    * https://serenesforest.net/forums/index.php?/topic/71279-shadows-of-valentia-save-editingsave-bank/

  * Info on character blocks: https://gbatemp.net/threads/research-fe-sov-save-discussion.471890/

  * Serenes Forest for character base and max stats: https://serenesforest.net/fire-emblem-echoes-shadows-valentia/

  * Inspired by FeTwiddler by Soaprman: https://github.com/Soaprman/FEFTwiddler 

    (Learned some file organisations and basic save editor code stuff)

  * Extended WPFToolkit for the numeric up downs (apparently WPF doesn't have it): http://wpftoolkit.codeplex.com/

  * Dark and Light mode themes modified from here: https://github.com/AngryCarrot789/WPFDarkTheme
  
### Building

To build from the CLI, change directory to the ./FESOVSE folder, then enter `dotnet publish -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true`

An .exe will be built to ./FESOVSE/bin/Release/net8.0-windows/win-x64/publish/
