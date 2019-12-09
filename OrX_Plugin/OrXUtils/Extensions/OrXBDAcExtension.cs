using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OrX
{
    internal static class OrXBDAcExtension
    {
        internal static Type PartExtensions;
        private static MethodInfo DamageMethod;
        private static MethodInfo MaxDamageMethod;
        private static bool isInstalled;

        internal static Type vsExtensions;
        private static PropertyInfo _vswitcher;

        static object vsGUI;

        static OrXBDAcExtension()
        {
            try
            {
                PartExtensions = AssemblyLoader.loadedAssemblies
                     .Where(a => a.name.Contains("BDArmory.Core")).SelectMany(a => a.assembly.GetExportedTypes())
                     .SingleOrDefault(t => t.FullName == "BDArmory.Core.Extension.PartExtensions");
                DamageMethod = PartExtensions.GetMethod("Damage");
                MaxDamageMethod = PartExtensions.GetMethod("MaxDamage");
                isInstalled = true;
            }
            catch (Exception e)
            {
                isInstalled = false;
            }
        }

        public static void SetBDAcVSGUI()
        {
            try
            {
                Debug.Log("[OrX VSExtention - SetBDAcVSGUI] === TRYING ===");

                vsExtensions = AssemblyLoader.loadedAssemblies
                     .Where(a => a.name.Contains("BDArmory")).SelectMany(a => a.assembly.GetExportedTypes())
                     .SingleOrDefault(t => t.FullName == "BDArmory.UI.BDArmorySetup");
                Debug.Log("[OrX VSExtention - SetBDAcVSGUI] === BD ARMORY SETUP FOUND ===");

                _vswitcher = vsExtensions.GetProperty("showVSGUI");
                Debug.Log("[OrX VSExtention - SetBDAcVSGUI] === FOUND showVSGUI: " + _vswitcher.Name + " ===");

                vsGUI = _vswitcher.GetValue(_vswitcher);
                Debug.Log("[OrX VSExtention - SetBDAcVSGUI] === VS GUI OPEN: " + vsGUI.ToString() + " ===");
                if (vsGUI.ToString() == "True")
                {
                    Debug.Log("[OrX VSExtention - SetBDAcVSGUI] === CLOSING VS GUI ===");
                    _vswitcher.SetValue(vsGUI, false);
                }
            }
            catch (Exception e)
            {
                Debug.Log("[OrX VSExtention - SetBDAcVSGUI] === ERROR === " + e);
            }
        }

        public static void ResetPAW(this Part part)
        {
            IEnumerator<UIPartActionWindow> paw = UnityEngine.Object.FindObjectsOfType(typeof(UIPartActionWindow)).Cast<UIPartActionWindow>().GetEnumerator();
            while (paw.MoveNext())
            {
                if (paw.Current == null) continue;
                if (paw.Current.part == part)
                {
                    paw.Current.displayDirty = true;
                }
            }
            paw.Dispose();
        }

        internal static bool BDArmoryIsInstalled()
        {
            return isInstalled;
        }

        internal static float Damage(this Part part)
        {
            return Convert.ToSingle(DamageMethod.Invoke(null, new object[] {part}));
        }

        internal static float MaxDamage(this Part part)
        {
            return Convert.ToSingle(MaxDamageMethod.Invoke(null, new object[] { part }));
        }
    }
}
