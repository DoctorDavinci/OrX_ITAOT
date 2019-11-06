using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OrX
{
    internal static class OrXPRExtension
    {
        internal static Type preExtentions;
        private static MethodInfo PREon;
        private static MethodInfo PREoff;
        public static bool _present;
        public static bool _preon;

        static OrXPRExtension()
        {
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
                _present = false;
            }
        }

        internal static void PreOn(string _modName)
        {
            if (_present)
            {
                PREon.Invoke(null, new object[] { _modName });
                _preon = true;
            }
        }

        internal static void PreOff(string _modName)
        {
            if (_present)
            {
                PREoff.Invoke(null, new object[] { _modName });
                _preon = false;
            }
        }
    }
}
