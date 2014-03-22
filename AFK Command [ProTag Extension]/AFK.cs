//   ___|========================|___
//   \  |  Written by Felladrin  |  /   This script was released on RunUO Community under the GPL licensing terms.
//    > |      August 2013       | <
//   /__|========================|__\   [AFK Command] [ProTag Extension] Current version: 1.0 (August 14, 2013)

using System;
using Server.Mobiles;

namespace Server.Commands
{
    public class AFK
    {
        public static void Initialize()
        {
            CommandSystem.Register("AFK", AccessLevel.Player, new CommandEventHandler(AFK_OnCommand));
            EventSink.Speech += new SpeechEventHandler(OnSpeech);
        }

        [Usage("AFK [<message>]")]
        [Description("Puts your char in 'Away From Keyboard' mode.")]
        private static void AFK_OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (isAFK(pm))
            {
                SetBack(pm);
            }
            else
            {
                if (e.Length == 0)
                {
                    SetAFK(pm, "Away From Keyborad");
                }
                else
                {
                    SetAFK(pm, e.ArgString);
                }

                AnnounceAFK(pm);
            }
        }

        private static void OnSpeech(SpeechEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
            {
                PlayerMobile pm = e.Mobile as PlayerMobile;

                if (isAFK(pm))
                    SetBack(pm);
            }
        }

        private static void AnnounceAFK(PlayerMobile pm)
        {
            if (!isAFK(pm))
                return;

            if (pm.Location.ToString() != GetAFKLocation(pm) || pm.NetState == null || pm.Deleted)
            {
                SetBack(pm);
                return;
            }

            TimeSpan ts = GetAFKTimeSpan(pm);

            pm.Emote("*{0}*", GetAFKMessage(pm));

            if (ts.Hours != 0)
            {
                pm.Emote("*AFK for {0} hour{1} and {2} minute{3}*", ts.Hours.ToString(), (ts.Hours > 1 ? "s" : ""), ts.Minutes.ToString(), (ts.Minutes > 1 ? "s" : ""));
            }
            else if (ts.Minutes != 0)
            {
                pm.Emote("*AFK for {0} minute{1}*", ts.Minutes.ToString(), (ts.Minutes > 1 ? "s" : ""));
            }
            else if (ts.Seconds != 0)
            {
                pm.Emote("*AFK for {0} seconds*", ts.Seconds.ToString());
            }

            Timer.DelayCall(TimeSpan.FromSeconds(10), delegate { AnnounceAFK(pm); });
        }

        private static void SetAFK(PlayerMobile pm, string message)
        {
            ProTag.Set(pm, "AFK Message", message);
            ProTag.Set(pm, "AFK Location", pm.Location.ToString());
            ProTag.Set(pm, "AFK Time", DateTime.Now.ToString());
            pm.Emote("*Is now AFK*");
        }

        private static void SetBack(PlayerMobile pm)
        {
            ProTag.Del(pm, "AFK Message");
            ProTag.Del(pm, "AFK Location");
            ProTag.Del(pm, "AFK Time");
            pm.Emote("*Returns to the game*");
        }

        private static bool isAFK(PlayerMobile pm)
        {
            return (ProTag.Get(pm, "AFK Time") != null);
        }

        private static string GetAFKMessage(PlayerMobile pm)
        {
            return ProTag.Get(pm, "AFK Message");
        }

        private static string GetAFKLocation(PlayerMobile pm)
        {
            return ProTag.Get(pm, "AFK Location");
        }

        private static TimeSpan GetAFKTimeSpan(PlayerMobile pm)
        {
            TimeSpan time;

            try
            {
                time = DateTime.Now - DateTime.Parse(ProTag.Get(pm, "AFK Time"));
            }
            catch
            {
                time = TimeSpan.Zero;
            }

            return time;
        }
    }
}