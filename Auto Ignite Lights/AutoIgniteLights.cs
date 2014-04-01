//   ___|========================|___
//   \  |  Written by Felladrin  |  /   This script was released on RunUO Forums under the GPL licensing terms.
//    > |      August 2013       | <
//   /__|========================|__\   [Auto Ignite Lights] - Current version: 1.0 (August 18, 2013)

namespace Server.Items
{
    public class AutoIgniteLights
    {
        public static class Settings
        {
            public static bool Enabled = true; // Is this system enabled?
            public static int IgniteHour = 18; // At what hour should we ignite the lights?
            public static int DouseHour = 7; // At what hour should we douse the lights?
            public static int CheckInterval = 5; // How often, in minutes, should we check the lights?
        }

        public static void Initialize()
        {
            if (Settings.Enabled)
                CheckLights();
        }

        public static void CheckLights()
        {
            foreach (Item item in World.Items.Values)
            {
                if (item is BaseLight && item.ParentEntity == null)
                {
                    BaseLight light = item as BaseLight;

                    int currentHour, currentMinute;

                    Clock.GetTime(light.Map, light.X, light.Y, out currentHour, out currentMinute);

                    if (currentHour > Settings.IgniteHour || currentHour < Settings.DouseHour)
                    {
                        light.Ignite();
                    }
                    else if (currentHour > Settings.DouseHour || currentHour < Settings.IgniteHour)
                    {
                        light.Douse();
                    }
                }
            }

            Timer.DelayCall(System.TimeSpan.FromMinutes(Settings.CheckInterval), delegate { CheckLights(); });
        }
    }
}