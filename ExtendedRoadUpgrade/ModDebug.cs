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
    }
}
