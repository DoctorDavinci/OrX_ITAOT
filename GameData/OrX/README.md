------------------------------------------------------------------
------------------------------------------------------------------
--Davinci's HoloCache System--
------------------------------------------------------------------
------------------------------------------------------------------

The Davinci's HoloCache System brings GeoCaching to KSP ... Now players can go to a location and create a 'HoloCache' that will allow a user to name the location, provide 3 lines of text to describe the location, record the gps location of the HoloCache (as well as up to 10 vessels within a 2km radius) then save that info to a config file ... Any crafts that have a GPS location stored in the HoloCache are also saved as craft files for later spawning

The the config and craft files created are saved to GameData/DHC/'Users HoloCache Name' ... Players can share the directories inside of the GameData/DHC directory with other players and the coordinates will be available to be loaded into the HoloCache control menu in the flight scene if they have DHC installed (copy shared HoloCache directories to GameData/DHC)

To load a HoloCache ... Open the DHC Control menu (green DHC with angled yellow script button) then click on 'Reload HoloCache Data' and any Holocache directory contained within GameData/OrXHoloCache will be added to the list in the menu ... Click on a HoloCache in the list to have a red dot drawn on the world telling you where that location is visually

To creat a HoloCache ... Go to the location you want to create the HoloCache and open the DHC Control menu, then click on 'Spawn Empty HoloCache' and a HoloCache will spawn after which a menu will pop up allowing you to name your HoloCache and provide 3 lines of text to describe the location ... PLEASE NOTE: HoloCache names must have no spaces or special characters in them (only letters and numbers, nothing else or the HoloCache System will get cranky)

Click on the 'SAVE VESSELS' button to tell the HoloCache to save any vessels that are within a 2 km radius of the (maximum of 10 vessels can be saved)

Click on the 'Save HoloCache' button to save the HoloCache 

When you select a HoloCache location in the HoloCache Control menu, the system will check your distance from that point and if you are within the physics load range of your KSP save then the HoloCache will be spawned (Maximum HoloCache spawn range capped at 15km due to reasons)

HoloCaches and the vessels they spawn are spawned in an unknown status ... Once the HoloCache spawns, a menu will show up with the description entered at the time that the HoloCache was saved and then the HoloCache part will start to spawn any vessels that were saved when the HoloCache was made if the 'SAVE VESSELS' feature was used



------------------------------------------------------------------
------------------------------------------------------------------
License and Attribution
------------------------------------------------------------------
------------------------------------------------------------------

DHC incorporates a modified version of InFlightShipSave which is licensed under CC BY-NC-SA 4 (applicable to those parts of the code only).

All credit goes to @Claw for InFlightShipSave

https://github.com/ClawKSP/InflightShipSave
https://creativecommons.org/licenses/by-nc-sa/4.0/

------------------------------------------------------------------

DHC also incorporates a modified version of the vessel spawning system released with Vessel Mover under the MIT License ... To comply with the requirements of the MIT license, the following permission notice, applicable to those parts of the code only, is included below:

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

All credit goes to @BahamutoD for creating Vessel Mover as well as @Papa_Joe for maintaining and continuing its development.

https://github.com/BahamutoD/VesselMover/releases
https://forum.kerbalspaceprogram.com/index.php?/topic/123646-11-vesselmover-v15-vessel-spawning-toolbar-ui-apr-25/

------------------------------------------------------------------

The DHC incorporates code from the GPS system in BD Armory Continued and is licensed under CC-BY-SA 2.0. 
(applicable to those parts of the code only).

Please read about the license at https://creativecommons.org/licenses/by-sa/2.0/

All credit goes to @BahamutoD for creating BD Armory and many thanks go to the BD Armory Continued team for maintaining and continuing development of this amazing mod

------------------------------------------------------------------

All other code contained within DHC is licensed as do what you want with it

Copyright © 2019 DoctorDavinci

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
