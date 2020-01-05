OrX Kontinuum - Is There Anybody Out There

FORUM DEVELOPMENT THREAD: https://forum.kerbalspaceprogram.com/index.php?/topic/188859-173-orx-kontinuum-development-thread-geo-caching-scuba-diving-winds-challenge-creation-and-more/&tab=comments#comment-3686281

OrX Kontinuum brings Geo Caching, Scuba Diving, the joy of W[ind/S] and more to KSP ..... It's purpose is to give you more reasons to explore


--Feature List--

OrX Craft Tuning ... gives you the ability to launch a vessel from the editor and tweak its various parts and save your changes to a separate craft file while in the flight scene
- You can now test and adjust your vessel then save your best setup without having to jump in and out of the editor (and remembering what your changes were :rolleyes:) 

OrX Boost Flaps ... Adds the option to control the deploy state of elevons being used as Boost Flaps (no additional part required)
- When a rover is in the air or all it's wheels are NOT touching the ground, the boost flaps will be deployed if the option has been enabled
- Adjustable slider to control the deploy speed of the boost flaps ... can be adjusted on the fly while driving
- Pitch, Yaw and Roll control will be automatically disabled if an elevon has been selected as a boost flap
PLEASE NOTE: You must toggle any given elevon you intend to use as a boost flap to activated in it's part action window 

Dakar Racing challenge type is now available to be created and played ... Your times are saved to the scoreboad and are also available to be exported as a .scores file to be shared with other players

Ability to view any challenge type HoloKron from within the editor by using the OrX Kontinuum Editor Menu (Short Track Racing, Dakar Racing and BD Armory challenges respectively)

Implemented a 'Return to Start' function accessible from within the scoreboard menu after completing a race and the scoreboard is being viewed ... click on the button to have your craft automatically picked up and moved to the start position of the challenge

Adjusted the tergeting reticle to be more asthetically pleasing... it will appear on screen when scanning for challenge HoloKrons or when competing in a challenge indication your next intended destination

HoloKrons now open automatically when you are within 25 meters (100 meters for BD Armory challenges) 

OrX Kontinuum should work on any celestial body although this has yet to be tested ... All testing so far has been on Kerbin however the code was written with the intention of being able to create HoloKrons anywhere (even modded ones :wink:)

BD Armory challenge type is available to be created and played ... Detailed directions on how to create a BD Armory challenge will be coming in the near future (there's a lot of stuff in there so it might take a little while to get it all down in writing)

PLEASE NOTE: In order to create or play a BD Armory challenge you obviously must have BD Armory Continued installed

PLEASE ALSO NOTE: Physics range extender must be disabled or OrX Kontinuum will shut itself down and not allow access to any of the challenge features ... PRE isn't required for OrX Kontinuum to work with BD Armory however unless you relish the idea of seeing a big red and bold message diplsyed across your screen telling you that you need to install PRE then it's best to just leave it in and disable it from its menu (the Disable Mod button)


- GEO-CACHE HOLOKRON SPECIFIC INFORMATION - 

- Geo-Caches are unable to be viewed until they have been found and only in their immediate location ... the description window will appear once the player comes within 25 meters of the HoloKron

- When scanning for Geo-Caches a scanning menu will display the distance to the nearest Geo-Cache if there is one withn the current SOI (untested at further than 700 km)

- There is a scan delay calculated off of the distance from the Geo-Cache location and the players craft ... Use the distance displayed in the menu after each scan to determine if you are getting closer (if the distance to target goes down each scan then you're heading in the general direction of a Geo-Cache)

- Geo-Cache HoloKrons will spawn when you are within 5 km and will also display a targeting reticle on screen indicating its exact position as well as any craft saved into the HoloKron will be spawned and placed at their saved locations (save local vessels option selected while creating the Geo-Cache) 

Kind of a game of hide and seek :wink:


- OUTLAW RACING CHALLENGE SPECIFIC INFORMATION -

- To compete in an Outlaw Racing challenge a player must not be EVA
- When a player starts a challenge their craft will be picked up, rotated to face the first stage gate (the finish gate in the case of Dakar racing) and then will be placed at the exact coordinates of the HoloKron ... Everyone starts a challenge from the exact same position and facing in the same direction

- A menu will apear displaying the total challenge time, the previous stage time (if challenge is not Dakar racing) as well as the distance to the next checkpoint/stage gate ... times between checkpoint/stage gate are recorded to the scoreboard

- After a challengers craft has been placed the brake action group will be enabled (aka the parking brake) ... Timer starts when the challenger starts moving (don't foget to disable the parking brake :wink:)

- Short Track racing challenges have a 4km range maximum during creation

- Dakar racing challenges have a theoretical max distance of the radius of the celestial body the challenge is being created on

- When competing in a Dakar challenge the stage gate for the finish line will spawn and be placed when you are within 800 meters as well as being rotated to face the challenger

- Short Track racing stage gates have a fixed rotation that they face which is determined by the challenge creator and will be spawned then placed in that specific postion and rotation (as placed when the challenge was created) if a challenger choses to spawn the challenge

- Outlaw Racing challenges do not require that you spawn the challenge (option for Short Track racing) however it is recommended
 
- Stage gates are unable to be placed if you are within 25 meters of the challenge start location (the world position at which the challenge creation was started)

- You can only place 1 stage gate when creating a Dakar racing challenge ... Dakar racing is a race from point A to point B

- All stage gates spawned while creating a challenge will automatically phase out of existence after clicking on the save button 


- BD ARMORY CHALLENGE SPECIFIC FEATURES - 

- Earn salt by destroying parts on enemy craft ... The more parts you destroy the saltier you will be

- Spend salt to buy craft while in battle if you need some extra firepower (salt is displayed as Kerb-Bucks ... it will cost the same amount of displayed salt as the cost of the vessel you wish to spawn)

- Purchased craft will spawn including a pilot with the exception of any external command chairs ... this means if your craft is controlled by a kerbal in a command chair then it will be pilotless (so don't use command chairs :wink:)

- Salt total (minus salt spent purchasing additional craft) is saved to scoreboard as well as other info such as mods in the players game, cheats enabled, kill count, player MIA count, player vessel count, enemy vessel count etc....

- 60 km spawn range for airborne enemy craft

- 20 km spawn range for the HoloKron

- 10 to 15km spawn range for landed vessels contained in HoloKron

- Ability to fly under the radar by staying below the detection altitude displayed in the chaleenge menu

- If flying under the radar the distance at which you become visible will determine the delay for enemy weapon manager activation (guard mode) ... Get the jump on your enemies (but don't wait too long or you might become overwhelmed :wink:)

- To finish a BD Armory challenge you must get to within 100 meters of the HoloKron at which point your current salt level and other information will be saved to the scoreboard and a detailed results window will appear displaying how many craft the player used (including purchased craft), how many enemies the player was up against and much more information about their challenge entry

- if you do not have enough salt to purchase a craft then an on screen message will appear telling the player how much more salt they need to purchase the selected craft

- When spawning craft using the BD Armory Spawn Craft feature, be sure to have the team set to 'A' as well as be sure that the weapon manager 'Guard Mode' is disabled ... OrX Kontinuum will automatically change the team to 'B' and turn on the guard mode as well as start the engines when the craft is spawned during a challenge (so don't start the engines :wink:)

- When spawning craft for a BD Armory challenge the Spawn Menu will appear with options to place the spawned craft, set the radar altitude of the craft (for airborne enemies), a slider to select the radar altitude to set a craft at (min of 2 km ... max of 8 km)  and the option to kill the spawned craft (this will destroy the spawned craft and switch your focus back to your original craft)

- Use the WASD keys to move spawned craft around and chose where you wish to place/set altitude

- If you set the altitude of a spawned craft then the option to place the craft on the ground will no longer be available

- Clicking on Hold Position after setting the altitude of a craft will hold that craft in that exact position until the HoloKron is saved

- All craft spawned while creating a challenge will automatically phase out of existence after clicking on the save button (they are save inside the HoloKron ... imagine what that is like, locked in a box :wink:)

PLEASE NOTE: While competing in a challenge, when vessels spawn your focus will switch momentarily and vessel switching will be disabled ... if you're worried about crashing your vessel during the spawning then just enable the AI pilot via the BDA Vessel Switcher window (the 'P' button) until your focus switches back to your craft

PLEASE ALSO NOTE: If you wish to be able to spawn a craft file while competing in a BD Armory challenge then that specific craft file must have been loaded into the editor and launched at least once ... If this has not been done then a message will appear on screen telling the player that the craft they chose to purchase is priceless


ADDITIONAL NOTES

- If player is missing mods listed in the .orx file then a warning menu will appear with a list of the missing mods as well as giving you the option to continue or cancel

- Additional systems have been put in place for future plans such as downloading of .orx files directly from inside the game, creating a Kontinuum to connect players together within the OrX Kontinuum framework as well as laying the groundwork for more challenge types such as Naval Salvage, Hang Gliding, Scuba Kerb, W[ind/S] and more ....

- Beware Karmageddon :wink:


Sample Short Track racing challenges have been included in this release of ORX Kontinuum

- 1 Mile Drag Race ... simple straight line race down the KSC Runway - Start position is located at the KSC Runway spawning point

- Ring Around the KSC ... Just what it sounds like, a race around the perimeter of the KSC - Start position is located beside the KSC Runway spawning point, about 100 meters to the right

- Tracking Station Loop ... A race around the Tracking Station, through the RnD tunnel and then back to the runway - Start position is located at the KSC Runway spawning point

The Dakar 2020 challenge has also been included


------------------------------------------------------------------
OrX Scuba Kerb
------------------------------------------------------------------

OrX Scuba Kerb gives your kerbals the ability to explore the depths of the oceans ......

- When you are EVA and splashed as well as in an atmosphere containing oxygen, the Scuba Kerb menu will automatically open (if you are the active vessel) ... it will automatically close if you are no longer splashed or no longer EVA

- The Oxygen slider displays your Kerbals' total Oxygen amount which depletes over time while underwater

- If you are at the surface of the ocean or on land your Oxygen will replenish if in an atmosphere that contains Oxygen

- Ballast is controlled by hotkeys while your Kerbal is splashed ... Q to trim up, E to trim down, Z to emergency surface and X to emergency dive

- You can also hold your current depth by pressing the 'Tab' key

- While diving be aware of Nitrogen Narcosis and pay attention to your Martini level ... the higher the Martini level the closer you are to being drunk

- Your Martini level will slowly drop as your Kerbal adjusts to the pressure ... PLEASE NOTE: Don't go too deep too fast or the pressure might get to you (it gives you that sinking feeling)

- If your Martini level reaches 6 then your Kerbal is drunk and it will automatically attempt to hold its depth, however if you are sinking fast your drunk self may be unable to recover and will continue to sink into the depths in search of the Kraken

- Kerbals who have been lost to the sea can be revived if you can find a way to get them to the surface

- Beware the Bends ... You will experience decompression as you rise to the surface and gasses that have built up inside your Kerbal due to the pressure will be released as you ascend which could potentially cause your kerbal to go 'POP' (Not Implemented Yet)

 


------------------------------------------------------------------
--OrX W[ind/S] -- (HIGHLY EXPERIMENTAL)
------------------------------------------------------------------

By filtering a low AC current through a signal generator using the W[ind/S] formula in relation with the Tesla Equation ( TL = Mc^2 ) and then passing the resulting signal through a Ferro-fluid composed of Buckyballs, Magnetite and Soy sandwiched between multiple layers of Carbon Nanotube sheeting (a SAIL ... S-ratio Accumulated Inductance Laminate), oscillations are generated within the SAIL causing it to move

    W = Weber's Law
    i = imaginary number
    n = factorial
    d = derivatives
    S = S-ratio

 

- Weber's Law states that any given derivative of a factorial of an imaginary number divided by the S-ratio of a given material is a constant in relation to the ratio of the increment threshold to the background intensity ... PLEASE NOTE: When you measure increment thresholds (the threshold here being oscillation amplitude) on varied intensity backgrounds, the thresholds increase in proportion to the background (the background in this case being the magnetic flux lines of the planet)

- S-ratio is a rock magnetic parameter employed to provide a relative measure of the contributions of low and high coercivity material to a sample's Saturation Isothermal Remanent Magnetization (SIRM)

- Another kind of IRM can be obtained by first giving the magnet a saturation remanence in one direction and then applying and removing a magnetic field in the opposite direction. This is called demagnetization remanence or DC demagnetization remanence.

- Yet another kind of remanence can be obtained by demagnetizing the saturation remanence in an ac field. This is called AC demagnetization remanence or alternating field demagnetization remanence.

- For manual control of W[ind/S], open the OrX W[ind/S] menu and you will see some sliders for controlling the blow intensity, variability and tease timing (how often things change) as well as an entry field to manually select the W[ind/S] heading (0 to 360 degrees)

- There is also a button to turn the W[ind/S] on or off in the menu

- When W[ind/S] is enabled any part that is a lifting surface will be affected ... How much of an effect is directly related to the deflection lift coefficient of the part and the angle at which the part intersects with the W[ind/S] heading

- The Weather Simulation is a controller for controlling the W[ind/S] direction based on a number of factors and is turned on from within the OrX W[ind/S] menu .... PLEASE NOTE: This is experimental and is in the process of being written so consider it a use at your own risk


------------------------------------------------------------------
OrX HoloKron System
------------------------------------------------------------------

The OrX HoloKron System brings GeoCaching to KSP ... Now you can leave a trail of breadcrumbs spanning the solar system (and beyond?)

Now Kerbo-not's can go to a location and create a 'HoloKron' that will save the location coordinates (including SOI and other pertinent data) to a config file (.orx file) as well as save any vessels within a 1km range including their GPS coordinates for later spawning

- The creator of the HoloKron can add a craft file that is unlocked and saved to the users save game directory when the HoloKron is opened

- The .orx file created is saved inside of GameData/OrX/Export/ ... Players can share .orx files contained within the GameData/OrX/Export/ directory with other KSP players and the coordinates will be available if they have OrX installed (copy shared HoloKron files to GameData/OrX/Import/)

- Craft files contained in the .orx file have been put through encryption so as to make it difficult for anyone to 'hack' the HoloKron by config editing outside of the game ... If you want to hack the HoloKron you must use the OrX BlackHat (isn't a thing ... yet) or go through the trouble of decoding the .craft file manually (good luck ... lol)

- Consult The Kurgan Manual in game or visit the forum thread linked above for detailed instructions about the operation of OrX Kontinuum


------------------------------------------------------------------
ADDITIONAL NOTES
------------------------------------------------------------------

- OrX Kontinuum has no dependancies

- If you see a portal, it's just a Test .... Please Ignore

- HoloKrons should work with mods and DLC parts, however if the user of your .orx file does not have the mods or DLC parts in their game that are on the vessels stored in the file then the vessels will not load



------------------------------------------------------------------
INSTALLATION INSTRUCTIONS 
------------------------------------------------------------------

1 - In the release download linked above you will find a GameData directory

2 - Copy the OrX directory and it's contents inside of GameData to your KSP GameData directory

3 - Profit .....


------------------------------------------------------------------
SHARING ORX FILES
------------------------------------------------------------------

All that is needed is for the person you are sharing your.orx and .scores files with is for them to have OrX Kontinuum installed

Copy shared files to GameData/OrX/Import


------------------------------------------------------------------
License and Attribution
------------------------------------------------------------------

All models, unless otherwise stated below, are licensed All Rights Reserved and may not be redistributed or modified without explicit permission

Copyright © 2019 @SpannerMonkey(smce) / @DoctorDavinci

------------------------------------------------------------------

 

OrX Kontinuum incorporates code from Vessel Mover released under the MIT License and is contained within the 'OrX.spawn' Namespace ... To comply with the requirements of the MIT license, the following permission notice, applicable to those parts of the code only, is included below:

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

All credit goes to @BahamutoD for creating Vessel Mover

https://github.com/BahamutoD/VesselMover/releases
https://forum.kerbalspaceprogram.com/index.php?/topic/123646-11-vesselmover-v15-vessel-spawning-toolbar-ui-apr-25/

------------------------------------------------------------------

OrX Kontinuum incorporates code from the Cloaking Device mod ... All credit goes to @wasml
http://spacedock.info/mod/217/Cloaking Device

All code used from the Cloaking Device mod has been absorbed into OrX via one-way compatibility from CC BY-SA 4.0 to GPLv3 and is now released under GPLv3
https://creativecommons.org/2015/10/08/cc-by-sa-4-0-now-one-way-compatible-with-gplv3/

------------------------------------------------------------------

All other code contained within OrX Kontinuum is licensed GPLv3

Copyright © 2019 @DoctorDavinci

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program. If not, see http://www.gnu.org/licenses/
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 