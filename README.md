OrX Kontinuum

OrX brings Geo Caching, Scuba Diving, the joy of W(ind/S) and more to KSP


--Feature List--

------------------------------------------------------------------
-- OrX Scuba Kerb System
------------------------------------------------------------------

The OrX Scuba Kerb system gives your kerbals the ability to explore the depths of any ocean on any planet

When you are EVA and splashed, the Scuba Kerb menu will automatically open (if you are the active vessel) ... it will automatically close if you are no longer splashed or no longer EVA

Balast can be controlled by hotkeys while your Kerbal is splashed ... Q to trim up, E to trim down, X to reset ballast to zero and Z to emergency dive

The Oxygen slider displays your Kerbals total Oxygen amount which depletes over time while underwater

If you are at the surface of the ocean or on land your Oxygen will replenish if in an atmosphere that contains Oxygen


------------------------------------------------------------------
--OrX W[ind/S] -- (HIGHLY EXPERIMENTAL - CURRENTLY UNAVAILABLE)
------------------------------------------------------------------

By filtering a low AC current through a signal generator using the W[ind/S] formula and passing the signal through a Ferrofluid composed of Buckeyballs, Magnetite and Soy sandwiched between multiple layers of Carbon Nanotube sheeting (a SAIL ... S-ratio Accumulated Inductance Laminate), oscillations are generated within the SAIL causing it to move

MATH:

	W = Webers' Law
	i = an imaginary number
	n = factorial
	d = derivitives
	S = S-ratio 

W[ind/S] = W defines the derivitaves of the factorial of i divded by the S-ratio of a given material

S-ratio is a rock magnetic parameter employed to provide a relative measure of the contributions of low and high coercivity material to a sample's Saturation Isothermal Remanent Magnetization (SIRM)

Another kind of IRM can be obtained by first giving the magnet a saturation remanence in one direction and then applying and removing a magnetic field in the opposite direction. This is called demagnetization remanence or DC demagnetization remanence.
Yet another kind of remanence can be obtained by demagnetizing the saturation remanence in an ac field. This is called AC demagnetization remanence or alternating field demagnetization remanence.

Webers' Law states that a derivitave of a factorial of an imaginary number divided by the S-ratio of a given material is a constant in relation to the ratio of the increment threshold to the background intensity ... PLEASE NOTE: When you measure increment thresholds (the threshold here being oscillation amplitude) on varied intensity backgrounds, the thresholds increase in proportion to the background (the background in this case being the magnetic flux of the planetary body)

For manual control of W[ind/S], open the OrX Wind menu and you will see some sliders for controlling the wind intensity, variability and tease timing (how often things change) as well as an entry field to manually select the w(ind/S) heading (0 to 360 degrees)

There is also a button to turn the W(ind/S) on or off in the menu

The amount of force applied to a SAIL is directly proportional to it's exposed surface area divided by 2 divided by the angle at which the wind vector hits the part up transform (0 to 90 degrees ... higher angle means lower force)

The Weather Simulation is a controller for controlling the W[ind/S] direction based on a number of factors and is turned on from within the OrX W[ind/S] menu .... PLEASE NOTE: This is experimental and is in the process of being written so consider it a use at your own risk


------------------------------------------------------------------
--OrX HoloCache System--
------------------------------------------------------------------

The OrX HoloCache System brings GeoCaching to KSP ... Now you can leave a trail of breadcrumbs spanning the solar system (and beyond?)

Now Kerbo-not's can go to a location and create a 'HoloCache' that will save the location coordinates (including SOI and other pertinent data) to a config file (.orx file) ... Orbital HoloCache's are unavailable at this time due to reasons
PLEASE NOTE: If within an atmosphere the HoloCache must be landed or splashed before you can proceed with its creation as well as YOU MUST BE EVA TO CREATE A HOLOCACHE

The .orx file created is saved inside of GameData/OrX/HoloCache/ ... Players can share the .orx files contained within the GameData/OrXHoloCache directory with other KSP players and the coordinates will be available to be loaded into OrX Kontinuum if they have OrX installed (copy shared .orx files to GameData/OrXHoloCache/)

The creator of a HoloCache can also add a craft file that is unlocked and saved to the users save game directory when the HoloCache is opened if the HoloCache type is a Geo-Cache ... If the HoloCache is a Challenge type then the blueprints will be saved when the challenge has been completed (SEE BELOW FOR INFO ON CHALLENGES)

In addition to being able to add a craft file to the HoloCache, the creator can also save all local vessel within a selectable range (1km spherical radius limit controlled via a slider in the HoloCache creation menu) ... All local vessels saved to the HoloCache will have its location and orientation saved for later spawning
PLEASE NOTE: Local vessels saved to a HoloCache will be spawned when the HoloCache spawns in their original position as well as in the same orientation ... These vessels are only available in the flight scene and will not be available in the editor

Craft files contained in the .orx file have been put through encryption so as to make it difficult for anyone to 'hack' the HoloCache by config editing outside of the game ... If you want to hack the HoloCache you must use the OrX BlackHat (isn't a thing ... yet) or go through the trouble of decoding the .craft file manually (good luck ... lol)

To creat a HoloCache you must be EVA ... Go to the location where you want to create the HoloCache, go EVA and open the OrX Kontinuum menu (button with OrX written in green), click on 'Create HoloCache' and a HoloKron will spawn just forward and above your head as well as the OrX HoloCache Creator will appear
PLEASE NOTE: The HoloKron

In the 'Name:' text entry box enter a name for your HoloCache

In the 'Password:' text entry box enter a password for your HoloCache if you wish to lock it down (default is 'OrX')
PLEASE NOTE: The password is there to provide a means for a creator to lock their HoloCache from being added to since multiple locations and missions can be added to one .orx file (I actually have reasons for including this feature ... future plans)
 
Click on the HoloCache Type button to select from Geo-Caching or Challenge types

Click on the Challenge Type to change the type of challenge ... Currently there is only 'Dakar Racing' ('Outlaw Racing', "Drag Racing', 'Scuba' and 'W(ind/S)' types planned for the future)

Add a description of your GeoCache/Challenge in the description text entry section ... as you type in a text entry field a new text entry box will appear below it, press 'TAB' to jump to the new text entry field (maximum of 10 lines)
PLEASE NOTE: It is suggested that when typing into the description fields that you pay attention to the length of the line as there is no limit to the lines length and any text that extends beyond the width of the creator menu will not be shown in the challenge menu when a user opens the HoloCache 

Click on the 'Add Blueprints' check box to open the OrX Craft Browser ... Select a hangar in the OrX Craft Browser (SPH or VAB from your current game save ... Default is SPH) then click on a craft in the list to add that craft file as a blueprint ... click cancel to return to the previous menu

Click on the 'Save Local Vessels' check box to save all vessels within up to a 1km radius of the HoloCache (slider in menu)

If the HoloCache being created is a Geo-Cache, click on the 'SAVE HOLOCACHE' button to save the HoloCache

If your HoloCache is of the Challenge type then select 'ADD COORDS' to open the OrX Co-ordinate Editor menu to start adding stages to the challenge
PLEASE NOTE: When you click on "ADD COORDS" a new HoloKron will spawn and focus will be switched to it. The HoloKron that spawns can be moved with the WASD keys

- Pressing 'TAB' cycles through 3 different heights and speeds (Slow, Normal and Ludicrous)
- Throttle up raises and throttle down decreases the altitude
- Starting position as well as current position will be shown by an 'Unknown' marker while in the Map view

Click on 'ADD LOCATION' to add the current latitude and logitude coordinates to the coord list with the altitude being roughly 2 meters above the terrain at that location ... The HoloKron will settle on the ground and a new HoloKron will spawn which will then become the active vessel ... Rinse and Repeat

When you select 'SAVE AND EXIT' the Kerbal which was the active vessel at the start of the HoloCache creation will become the active vessel and the HoloCache will be saved

Select 'CANCEL' at any time to cancel the HoloCache creation

When you select 'Scan for HoloCache' in the OrX Kontinuum menu, the system will check your distance from every HoloCache in memory and if you are within 2km then the HoloCache will be spawned as well as each of the saved local vessels (if any) at their saved locations (save your mun base ????)

If you see a portal, it's a test, please ignore


------------------------------------------------------------------
------------------------------------------------------------------
INSTALLATION INSTRUCTIONS
------------------------------------------------------------------
------------------------------------------------------------------

In the release download (.zip file from the OrX releases section on GitHub) you will find a GameData directory (GameData/OrX)

Copy the OrX directory and it's contents to your KSP's GameData directory



------------------------------------------------------------------
------------------------------------------------------------------
License and Attribution
------------------------------------------------------------------
------------------------------------------------------------------

OrX incorporates code from Vessel Mover released under the MIT License ... To comply with the requirements of the MIT license, the following permission notice, applicable to those parts of the code only, is included below:

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

All credit goes to @BahamutoD for creating Vessel Mover as well as @Papa_Joe for maintaining and continuing its development.

https://github.com/BahamutoD/VesselMover/releases
https://forum.kerbalspaceprogram.com/index.php?/topic/123646-11-vesselmover-v15-vessel-spawning-toolbar-ui-apr-25/

------------------------------------------------------------------

OrX incorporates code from the Cloaking Device mod ... All credit goes to @wasml 
http://spacedock.info/mod/217/Cloaking%20Device

All code used from the Cloaking Device mod has been absorbed into OrX via one-way compatibility from CC BY-SA 4.0 to GPLv3 and is now released under GPLv3
https://creativecommons.org/2015/10/08/cc-by-sa-4-0-now-one-way-compatible-with-gplv3/

------------------------------------------------------------------

All other code contained within OrX Kontinuum is licensed GPLv3

Copyright © 2019 DoctorDavinci

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
