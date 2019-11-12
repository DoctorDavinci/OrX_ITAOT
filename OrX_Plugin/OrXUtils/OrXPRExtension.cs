using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OrX
{
    internal static class OrXPRExtension
    {
        internal static Type preExtensions;
        private static MethodInfo LoadConfig;
        private static MethodInfo PREoff;
        private static bool _present;
        private static bool _preon;

        static OrXPRExtension()
        {
            /*
try
{
    preExtentions = AssemblyLoader.loadedAssemblies
         .Where(a => a.name.Contains("PhysicsRangeExtender")).SelectMany(a => a.assembly.GetExportedTypes())
         .SingleOrDefault(t => t.FullName == "PhysicsRangeExtender.PRExtentions");
    PREon = preExtentions.GetMethod("PreOn");
    PREoff = preExtentions.GetMethod("PreOff");
    _present = true;
}
catch (Exception e)
{
    //_present = false;
}

*/

            try
            {
                Debug.Log("[OrX PRExtention] === TRYING ===");

                preExtensions = AssemblyLoader.loadedAssemblies
                     .Where(a => a.name.Contains("PhysicsRangeExtender")).SelectMany(a => a.assembly.GetExportedTypes())
                     .SingleOrDefault(t => t.FullName == "PhysicsRangeExtender.PRESettings");

                Debug.Log("[OrX PRExtention] === PhysicsRangeExtender.PRESettings FOUND ===");
                OrXHoloKron.instance._preInstalled = true;

                //LoadConfig = preExtensions.inv("LoadConfig");
                Debug.Log("[OrX PRExtention] === FOUND LoadConfig ===");
                _present = true;
                //PREon = preExtensions.GetMethod("PreOn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Debug.Log("[OrX PRExtention] === PreOn METHOD FOUND ===");

            }
            catch (Exception e)
            {
                Debug.Log("[OrX PRExtention] === ERROR === " + e);

                //_present = false;
            }
        }
        internal static bool PreIsInstalled()
        {
            return _present;
        }

        internal static void PreOn(string _modName)
        {


            if (PreIsInstalled())
            {
                ConfigNode _preSettingsFile = ConfigNode.Load("GameData/PhysicsRangeExtender/settings.cfg");
                if (_preSettingsFile != null && _preon)
                {
                    OrXHoloKron.instance._preInstalled = true;

                    ConfigNode _preSettings = _preSettingsFile.GetNode("PreSettings");
                    foreach (ConfigNode.Value cv in _preSettings.values)
                    {
                        if (cv.name == "ModEnabled")
                        {
                            Debug.Log("[OrX PRExtention] === ModEnabled " + cv.value + " ... CHANGING ===");

                            cv.value = "True";

                            _preSettingsFile.Save("GameData/PhysicsRangeExtender/settings.cfg");

                            foreach (FieldInfo field in AssemblyLoader.loadedAssemblies
.Where(a => a.name.Contains("PhysicsRangeExtender")).SelectMany(a => a.assembly.GetExportedTypes())
.SingleOrDefault(t => t.FullName == "PhysicsRangeExtender.PRESettings").GetFields())
                            {
                                if (field.Name == "ModEnabled")
                                {
                                    field.SetValue(field, true);

                                    if (field.GetValue(field).ToString() == "True")
                                    {
                                        field.SetValue(field, false);
                                    }

                                }
                                //string fieldValue = field.GetValue(field).ToString();

                            }

                            //preExtensions.InvokeMember("LoadConfig", BindingFlags.Static, null, null, null);

                            //LoadConfig.Invoke(null, new object[] { });
                        }
                    }
                }
            }

            /*
            Debug.Log("[OrX PRExtention] === ENABLING PRE ===");
            FieldInfo[] fields = preExtensions.GetFields(); // Obtain all fields
            foreach (FieldInfo field in fields) // Loop through fields
            {
                string name = field.Name; // Get string name
                if (name == "ModEnabled")
                {
                    object temp = field.GetValue(preExtensions); // Get value
                    if (temp is bool) // if it is a bool.
                    {
                        Debug.Log("[OrX PRExtention] === TURNING ON PRE ===");

                        field.SetValue(preExtensions, true);
                    }
                }
            }
            */
        }

        internal static void PreOff(string _modName)
        {
            /*
            try
            {
                preExtentions = AssemblyLoader.loadedAssemblies
                     .Where(a => a.name.Contains("PhysicsRangeExtender")).SelectMany(a => a.assembly.GetExportedTypes())
                     .SingleOrDefault(t => t.FullName == "PhysicsRangeExtender.PRExtentions");
                PREon = preExtentions.GetMethod("PreOn");
                PREoff = preExtentions.GetMethod("PreOff");
                _present = true;
                Debug.Log("[OrX PRExtention] === FOUND PRE ===");

            }
            catch (Exception e)
            {
                Debug.Log("[OrX PRExtention] === ERROR === " + e);
            }

            */
            if (PreIsInstalled())
            {
                Debug.Log("[OrX PRExtention] === TRYING TO SHUT DOWN PRE ===");

                ConfigNode _preSettingsFile = ConfigNode.Load("GameData/PhysicsRangeExtender/settings.cfg");
                if (_preSettingsFile != null)
                {
                    OrXHoloKron.instance._preInstalled = true;

                    ConfigNode _preSettings = _preSettingsFile.GetNode("PreSettings");

                    string PREEnabled = _preSettings.GetValue("ModEnabled");
                    if (PREEnabled == "True")
                    {
                        foreach (ConfigNode.Value cv in _preSettings.values)
                        {
                            if (cv.name == "ModEnabled")
                            {
                                Debug.Log("[OrX PRExtention] === ModEnabled " + cv.value + " ... CHANGING ===");

                                cv.value = "False";

                                _preon = true;
                                _preSettingsFile.Save("GameData/PhysicsRangeExtender/settings.cfg");

                                foreach (FieldInfo field in AssemblyLoader.loadedAssemblies
                     .Where(a => a.name.Contains("PhysicsRangeExtender")).SelectMany(a => a.assembly.GetExportedTypes())
                     .SingleOrDefault(t => t.FullName == "PhysicsRangeExtender.PRESettings").GetFields())
                                {
                                    if (field.Name == "ModEnabled")
                                    {
                                        field.SetValue(field, false);

                                        if (field.GetValue(field).ToString() == "True")
                                        {
                                            field.SetValue(field, false);
                                        }

                                    }
                                    //string fieldValue = field.GetValue(field).ToString();

                                }



                                //preExtensions.InvokeMember("LoadConfig", BindingFlags.Static, null, null, null);
                                //LoadConfig.Invoke(null, new object[] { });
                            }
                        }
                    }
                }
            }


            
            FieldInfo[] fields = preExtensions.GetFields(BindingFlags.Static | BindingFlags.Instance); // Obtain all fields
            foreach (FieldInfo field in fields) // Loop through fields
            {
                try
                {
                    string name = field.Name; // Get string name
                    if (name == "ModEnabled")
                    {
                        Debug.Log("[OrX PRExtention] === FIELD " + name + " FOUND ===");

                        object temp = field.GetValue(preExtensions); // Get value
                        if (temp is bool) // if it is a bool.
                        {
                            Debug.Log("[OrX PRExtention] === VALUE = " + name + " ===");

                            field.SetValue(preExtensions, false);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("[OrX PRExtention] === FIELD ERROR === " + e);

                }
            }

            /*
            if (_present)
            {
                Debug.Log("[OrX PRExtention] === SHUTTING DOWN PRE ===");
                try
                {
                    if (PreIsInstalled())
                    {
                        PREoff.Invoke(null, new object[] { _modName });
                        _preon = false;
                    }
                }
                catch
                {

                }
            }
            else
            {
                Debug.Log("[OrX PRExtention] === SMOETHING WENT WRONG AGAIN ===");

            }
            */
        }
    }
}
