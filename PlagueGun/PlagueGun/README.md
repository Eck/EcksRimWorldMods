# EcksRimWorldMods
A public repo for my learning-to-mod Rim World efforts.

## PlagueGun
I worked through Jecrell's old PlagueGun tutorial mod and updated it for version 1.3 of RimWorld.

* [RimWorld Modding Playlist](https://www.youtube.com/watch?v=mbplt3dLVbM&list=PL2M43bS2cfSNisHXDGbN67ByWI3fdVvbI)
* [How To Mod PlagueGun - Step by Step](https://www.youtube.com/watch?v=UgCOhFzeX4A&list=PL2M43bS2cfSNisHXDGbN67ByWI3fdVvbI&index=2)

## Release Notes
* 1.0 - Mar 03, 2022 
  - Initial Release
* 1.1 - Jul 24, 2023 
  - Corrected mod for use with version 1.4

## Build/Deploy Notes
I automated the build for this project so that when it builds, it copies all the files into the appropriate place. It's hardcoded to build its dll to MY rimworld directory. That almost certainly won't work for you, so make sure you change that. There are two parts of the deployment script you need to change: The Output Path and the Pre Build Step
<ol>
	<li>In Visual Studio, right click on the project and select properties.</li>
	<li>Select the Build tab on the left</li>
	<li>Scroll down under Output, and change the Output Path field to the Assemblies folder of your plague gun:
		<ul>
			<li>My folder looks like this: (yours will be different)</li>
			<li>E:\SteamLibrary\steamapps\common\RimWorld\Mods\PlagueGun\Assemblies\</li>
		</ul>
	</li>
	<li>Still in the project properties, select the Build Events tab on the left</li>
	<li>In the Pre-Build event commandline, change the PlagueGunModPath to where you want the mod installed.</li>
	<li>That should do it. Now when you build the project it should update the dll and xml data in your mod folder. </li>
</ol>

## Links
* [EcksRimWorldMods](https://github.com/Eck/EcksRimWorldMods) - Git repo
* [My Discord](bit.ly/eck314-Discord) - Feel free to come hang out and ask questions.
* [twitter/Eck314](https://twitter.com/Eck314)
* [twitch/Eck314](https://twitch.tv/Eck314)
* [youtube/Eck314](https://youtube.com/c/Eck314)

## Credits
Jecrell deserves almost all the credit here. I followed his plague gun tutorial and just worked through some of the issues.
* Original Post: https://ludeon.com/forums/index.php?topic=33219.0
* Also, he did the original plague gun graphic.

