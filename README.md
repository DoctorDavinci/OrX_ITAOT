OrX - Is There Anybody Out There

OrX brings Geo Caching, Scuba Diving, the joy of Wind and more to KSP


--Feature List--

------------------------------------------------------------------
-- OrX Scuba Kerb System
------------------------------------------------------------------

The OrX Scuba Kerb system gives your kerbals the ability to explore the depths of any ocean on any planet

When you are EVA and splashed, the Scuba Kerb menu will automatically open (if you are the active vessel) ... it will automatically close if you are no longer splashed or no longer EVA

Balast can be controlled by hotkeys while your Kerbal is splashed ... Q to trim up, E to trim down, X to reset ballast to zero and Z to add 10x your Kerbals mass to emergency dive

The Oxygen slider displays your Kerbals total Oxygen amount which depletes over time while underwater

If you are at the surface of the ocean or on land your Oxygen will replenish if in an atmosphere that contains Oxygen

PLEASE NOTE: Beware of Nitrogen Narcosis (pay attention to your Martini's) and the Bends (Don't rise to the surface too fast)


------------------------------------------------------------------
--OrX Wind -- (EXPERIMENTAL)
------------------------------------------------------------------

OrX Wind is a simple implementation of adding forces to a vessel ... The Weather Simulation in OrX Wind, not so much

The Weather Simulation is a controller for directing the wind direction based on a number of factors and is turned on from within the OrX Wind menu .... PLEASE NOTE: This is experimental and is in the process of being written so consider it a use at your own risk

As for manual control of the wind, open the OrX Wind menu and you will see some sliders for controlling the wind intensity, variability and tease timing (how often things change) as well as an entry field to manually select the wind heading (0 to 360 degrees)

There is also a button to turn the wind on or off in the menu

Wind affects a vessel in two ways if OrX Wind has been turned on ... 

- First, each vessel that is loaded in the flight scene has a slight force applied to it in the direction the wind is blowing

- Second, any part with ModuleLiftingSurface will have a button in the PAW that enables and disables the part as a sail ... When the sail button is active the part will have a force applied to it in a direction obtained by bisecting it's up transform and the wind direction
PLEASE NOTE: Sailing into the wind may not work very well at this time

The amount of force applied to parts with sail mode activated is directly proportional to it's exposed surface area divided by 2 multiplied by the ModuleLiftingSurface lift coefficient for the part divided by the angle at which the wind vector hits the part up transform (0 to 90 degrees ... higher angle means lower force)


------------------------------------------------------------------
--OrX HoloCache System--
------------------------------------------------------------------

The OrX HoloCache System brings GeoCaching to KSP ... Now you can leave a trail of breadcrumbs spanning the solar system (and beyond?)

Now Kerbo-not's can go to a location and create a 'HoloCache' that will save the location coordinates (including SOI and other pertinent data) to a config file (.orx file) ... PLEASE NOTE: If within an atmosphere the HoloCache must be landed or splashed before you can proceed with its creation as well as YOU MUST BE EVA TO CREATE A HOLOCACHE

A creator is also able to place a simple challenge in their holo ... Currently there are 3 types which are Wind, Scuba and OutLaw Racing (EXPERIMENTAL .... UNFINISHED)

Each challenge in the HoloCache will have an associated scoreboard which keeps track of the best 10 challenger times which automatically updates the scoreboard in the .orx file when the challenge is completed

An additional file is generated when a challenge is completed called 'The HoloCache Name'.scores which can be found in the same directory as the .orx file ... This file records the challengers scores (created new if doesn't exist and overwrites only if the scores are better) and can be used to update the scoreboard in any .orx file of the same name by placing it into the OrX/Import directory (easy way for a HoloCache creator to keep the .orx file up to date with all challengers scores ... Top 10 scores only in descending order, all others get deleted)

The creator of the HoloCache can also add a craft file that is unlocked and saved to the users save game directory when the holocache is opened, or if the holo has a challenge in it, after the player has completed the challenge

The .orx file created is saved inside of GameData/OrXHoloCache/'The Holo Cache Name'/ ... Players can share the directories and .orx files contained within the GameData/OrXHoloCache directory with other KSP players and the coordinates will be available to be loaded into the HoloCache control menu in the flight scene if they have OrX installed (copy shared HoloCache directories to GameData/OrXHoloCache/'The Holo Cache Name'/)

Craft files contained in the .orx file have been put through encryption so as to make it difficult for anyone to 'hack' the HoloCache by config editing outside of the game ... If you want to hack the HoloCache you must use the OrX BlackHat (isn't a thing ... yet) or go through the trouble of decoding the .craft file manually (good luck ... lol)

To load a HoloCache ... Open the HoloCache Control menu (green OrX button) then click on 'Load HoloCache Data' and any Holocache (.orx file) contained within GameData/OrXHoloCache and any directories found inside GameData/OrXHoloCache will be added to the list in the menu if the HoloCache was created while within the same SOI as the current active vessel ... While in the flight scene, click on a HoloCache in the list to have an indicator telling you where that location is visually (red dot on screen in flight scene) ... Click on it again to deselect it

To creat a HoloCache you must be EVA ... Go to the location where you want to create the HoloCache and open the HoloCache Control menu, click on 'Spawn Empty HoloCache' and a HoloCache will spawn and the HoloCache Creation menu will appear ... Yes, it floats in the air slowly rotating

Click on the HoloCache Type button to select from a Geo-Caching holo or a Challenge holo

Click on the Challenge Type to change the type of challenge ... Currently there is OutLaw Racing, Scuba and Wind (EXPERIMENTAL ... UNFINISHED)

Click on the 'Add Blueprints' check box to open the OrX Craft Browser ... Select a hangar in the OrX Craft Browser (SPH or VAB from your current game save ... Default is SPH) then click on a craft in the list to add that craft file as a blueprint ... click cancel to return to the previous menu

Click on the 'Save Local Vessels' check box to save all vessels within a 1km radius of the HoloCache ... Each vessel will be saved into the Holocache (.orx file) as well all information required for later spawning 
- When you select a HoloCache location in the HoloCache Control menu, the system will check your distance from that point and if you are within 5km then the HoloCache will be spawned and then the HoloCache will spawn each of the saved vessels at their saved locations (save your mun base ????) ..... PLEASE NOTE: This is experimental and only kinda/sorta works at this time

In the 'HoloCache Name:' text entry box enter a name for your HoloCache

Add a description of your GeoCache/Challenge in the description text entry section ... as you type in a text entry field a new text entry box will appear below it, press 'TAB' to jump to the new text entry field (maximum of 10 lines)

If the HoloCache being created is a Geo-Cache, click on the 'SAVE HOLOCACHE' button to save the HoloCache ... If a HoloCache file exists with the same name you will have the option to add to that file, therby adding another location which would be unlocked upon opening/finishing a challenge in the Holocache that is being added to (each HoloCache will be unlocked in sequence ... meaning one after the other)

If the HoloCache is a challenge type you will see an ADD COORDS button ... Click on this button to save the current data (holo name, description, type etc...) and the GPS coord entry menu will appear
Either get into a vehicle, start running around with your kerbal or use Vessel Mover and start travelling around and saving GPS coords then click on the SAVE button when done

If setting up a Wind challenge, use the OrX Wind control menu to change the wind direction before saving a GPS location to the coord database in order to change the wind direction for the next stage ... Wind intensity, variability etc... is saved along with the GPS location in all challenge types

If setting up a Scuba challenge, all GPS coords for the challenge must be underwater

If setting up an OutLaw Racing challenge then pretty much anything goes (at this time)

HoloCaches are spawned in an unknown status and can only be opened while on EVA

If you see a portal, please ignore ... You never know where you might end up



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

OrX incorporates Vessel Mover released under the MIT License ... To comply with the requirements of the MIT license, the following permission notice, applicable to those parts of the code only, is included below:

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

All credit goes to @BahamutoD for creating Vessel Mover as well as @Papa_Joe for maintaining and continuing its development.

https://github.com/BahamutoD/VesselMover/releases
https://forum.kerbalspaceprogram.com/index.php?/topic/123646-11-vesselmover-v15-vessel-spawning-toolbar-ui-apr-25/

------------------------------------------------------------------

The OrX HoloCache System incorporates code from BD Armory Continued and is licensed under CC-BY-SA 2.0. (applicable to the OrX HoloCache System only).

Please read about the license at https://creativecommons.org/licenses/by-sa/2.0/

All credit goes to @BahamutoD for creating BD Armory and many thanks go to the BD Armory Continued team for maintaining and continuing development of this amazing mod

------------------------------------------------------------------

OrX incorporates code from the Cloaking Device mod ... All credit goes to @wasml 
http://spacedock.info/mod/217/Cloaking%20Device

All code used from the Cloaking Device mod has been absorbed into OrX via one-way compatibility from CC BY-SA 4.0 to GPLv3 and is now released under GPLv3
https://creativecommons.org/2015/10/08/cc-by-sa-4-0-now-one-way-compatible-with-gplv3/

------------------------------------------------------------------

All other code contained within OrX is licensed GPLv3

Copyright © 2019 DoctorDavinci

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
