------------------------------------------------------------------
-- OrX - Is There Anybody Out There
------------------------------------------------------------------

OrX brings Geo Caching and Scuba Diving as well as the joy of Wind to KSP


--Feature List--

------------------------------------------------------------------
-- OrX Scuba Kerb System
------------------------------------------------------------------

The OrX Scuba Kerb system gives your kerbals the ability to explore the depths of any ocean on any planet

When your kerbal is splashed and is the active vessel, a menu will open giving you a trim up and a trim down button that you can use to control the ballast of your kerbal using your mouse

Balast can be controlled by hotkeys while your Kerbal is splashed and the Scuba Kerb menu is open ... Q to trim up, E to trim down, X to reset ballast to zero and Z to add 10x your Kerbals mass to emergency dive (HOTKEYS ARE EXPERIMENTAL)

The Oxygen slider displays your Kerbals total Oxygen amount which depletes over time while underwater

The modifier slider in the menu is for making minor adjustments to how much ballast is added or subtracted from your kerbal when trimming up or down ... click on the slider to adjust the ballast amount

If you are at the surface of the ocean or on land your Oxygen will replenish if in an atmosphere that contains Oxygen

PLEASE NOTE: Beware of Nitrogen Narcosis and the Bends ... Don't go too deep too fast and don't go deeper than you're rated for


------------------------------------------------------------------
--OrX HoloCache System--
------------------------------------------------------------------

The OrX HoloCache System brings GeoCaching to KSP ... Now you can leave a trail of breadcrumbs spanning the solar system (and beyond?)

Now Kerbo-not's can go to a location and create a 'HoloCache' that will save the location coordinates (including SOI and other pertinant data) to a config file (.orx file) ... PLEASE NOTE: If within an atmosphere the HoloCache must be landed or splashed before you can proceed with its creation

The creator of the HoloCache can also add a craft file from their save as well as lock the HoloCache behind a password 

The .orx file created is saved to GameData/OrXHoloCache/ ... Players can share the .orx files inside of the GameData/OrXHoloCache directory with other players and the coordinates will be available to be loaded into the HoloCache control menu in the flight scene if they have OrX installed (copy shared HoloCache directories to GameData/OrXHoloCache/'.orx file name'/) ... If a craft was added when the HoloCache was created the craft will be saved to the current games save folder if the HoloCache is unlocked

Craft files and most other data contained in the .orx file has been put through some encryption so as to make it difficult for anyone to 'hack' the HoloCache by config editing outside of the game ... If you want to hack the HoloCache you must use the OrX BlackHat

To load a HoloCache ... Open the HoloCache Control menu (green OrX with angled yellow script button) then click on 'Load HoloCache Data' and any Holocache directory contained within GameData/OrXHoloCache will be added to the list in the menu ... While in the flight scene, click on a HoloCache in the list to have an indicator telling you where that location is visually (red dot on screen in flight scene)

To creat a HoloCache ... Go to the location where you want to create the HoloCache and open the HoloCache Control menu, click on 'Spawn Empty HoloCache' and a HoloCache will spawn then the HoloCache menu will appear... PLEASE NOTE: HoloCache names must have no spaces or special characters in them (only letters and numbers, nothing else or the HoloCache System will get cranky)

Click on the 'ADD BLUEPRINTS' button to open the blueprints selection menu ... click on a craft in the list to add that craft file or click cancel to return to the previous menu

Click on the 'LOCK' button to add a password to unlock ... Default password is OrX

In the 'Holo Name:' text entry box enter a name for your HoloCache

Click on the 'SAVE HOLOCACHE' button to save the HoloCache ... If a .orx file exists with the same name you will have the option to add to that file, therby adding another location which would be unlocked upon opening of the Holocache that is being added to (each HoloCache will be unlocked in sequence ... meaning one after the other)

When you select a HoloCache location in the HoloCache Control menu, the system will check your distance from that point and if you are within the physics load range of your KSP save then the HoloCache will be spawned (Maximum HoloCache spawn range capped at 15km due to reasons)

HoloCaches are spawned in an unknown status and can only be opened by being within 2 meters of the HoloCache while on EVA

If you see a portal, please ignore ... You never know where you might end up


------------------------------------------------------------------
--OrX Wind System-- (EXPERIMENTAL)
------------------------------------------------------------------

Wind is a simple addition of forces to any part that has ModuleLiftingSurface ... You can now use wings as sails on a boat in the Kerbin seas (and Laythe? ... maybe fly a kite on Duna?)

Fully controllable wind intensity, wind direction and variability from within the Wind menu




------------------------------------------------------------------
------------------------------------------------------------------
INSTALLATION INSTRUCTIONS
------------------------------------------------------------------
------------------------------------------------------------------

In the release download (.zip file from the OrX releases section on GitHub) you will find a GameData directory (GameData/OrX)

Copy the GameData/OrX directory and it's contents to your KSP's GameData directory



------------------------------------------------------------------
------------------------------------------------------------------
License and Attribution
------------------------------------------------------------------
------------------------------------------------------------------

OrX incorporates the vessel spawning system released with Vessel Mover under the MIT License and is contained witnin the OrX.spawn namespace ... To comply with the requirements of the MIT license, the following permission notice, applicable to those parts of the code only, is included below:

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

All credit goes to @BahamutoD for creating Vessel Mover as well as @Papa_Joe for maintaining and continuing its development.

https://github.com/BahamutoD/VesselMover/releases
https://forum.kerbalspaceprogram.com/index.php?/topic/123646-11-vesselmover-v15-vessel-spawning-toolbar-ui-apr-25/

------------------------------------------------------------------

The OrX HoloCache System incorporates code from the GPS system in BD Armory Continued and is licensed under CC-BY-SA 2.0. 
(applicable to those parts of the code only).

Please read about the license at https://creativecommons.org/licenses/by-sa/2.0/

All credit goes to @BahamutoD for creating BD Armory and many thanks go to the BD Armory Continued team for maintaining and continuing development of this amazing mod

------------------------------------------------------------------

All other code contained within OrX is licensed GPLv3

Copyright © 2019 DoctorDavinci

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
