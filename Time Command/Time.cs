// Time Command v1.1.0
// Author: Felladrin
// Started: 2013-08-18
// Updated: 2016-01-21

// Installation:
// Drop this script anywhere inside your Scripts folder.
// Delete the default Time Command file: Scripts/Commands/ShardTime.cs

using Server;
using Server.Commands;
using Server.Items;

namespace Felladrin.Commands
{
    public static class Time
    {
        public static void Initialize()
        {
            CommandSystem.Register("Time", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("Time")]
        [Description("Informs the in-game time of your current region and also the real-world time (UTC).")]
        public static void OnCommand(CommandEventArgs e)
        {
            var m = e.Mobile;
            int currentHour, currentMinute;
            Clock.GetTime(m.Map, m.X, m.Y, out currentHour, out currentMinute);
            m.SendMessage("It's {0} now in {1}.", System.DateTime.Parse(currentHour + ":" + currentMinute).ToString("HH:mm"), (m.Region.Name ?? m.Map.Name));
            m.SendMessage("Real-World UTC: {0}.", System.DateTime.UtcNow.ToString("HH:mm"));
        }
    }
}