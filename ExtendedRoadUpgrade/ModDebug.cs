using System;
using System.Collections.Generic;

using ColossalFramework;

namespace ExtendedRoadUpgrade {
    public static class ModDebug {
        const string prefix = "ExtendedRoadUpgrade: ";
        static bool debuggingEnabled = false;

        // Print messages to the in-game console that opens with F7
        public static void Log(object s) {
            if (!debuggingEnabled) return;
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, prefix + ObjectToString(s));
        }

        public static void Error(object s) {
            if (!debuggingEnabled) return;
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Error, prefix + ObjectToString(s));
        }

        public static void Warning(object s) {
            if (!debuggingEnabled) return;
            DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Warning, prefix + ObjectToString(s));
        }

        static string ObjectToString(object s) {
            return s != null ? s.ToString() : "(null)";
        }

        public static void LogHierarchy(UnityEngine.GameObject root, int depth = 0) {
            string prefix = "";
            for (int i = 0; i < depth; ++i) prefix += "  ";
            Log(prefix + "GameObject: " + root.name);
            foreach (var component in root.GetComponents<UnityEngine.Component>()) {
                Log(prefix + "Component: " + component.GetType().Name);
            }

            foreach (UnityEngine.Transform child in root.transform) {
                LogHierarchy(child.gameObject, depth + 1);
            }
        }
    }
}
