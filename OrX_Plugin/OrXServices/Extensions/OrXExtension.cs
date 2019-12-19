using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OrX
{
    internal static class OrXExtension
    {
        internal static Type DKI;
        public static bool devKitInstalled;

        static OrXExtension()
        {
            try
            {
                DKI = AssemblyLoader.loadedAssemblies
                     .Where(a => a.name.Contains("OrX.DevKit")).SelectMany(a => a.assembly.GetExportedTypes())
                     .SingleOrDefault(t => t.FullName == "OrX.DevKit");

                if (DKI != null)
                {
                    Debug.Log("[OrX Log] === OrX Dev Kit is installed ===");
                    devKitInstalled = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[OrX Log] === OrX Dev Kit not installed ... DENIED ===");
                devKitInstalled = false;
            }
        }

        internal static bool OrXDevKitIsInstalled()
        {
            return devKitInstalled;
        }
    }
}
