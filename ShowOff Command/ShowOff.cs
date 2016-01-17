// ShowOff Command v1.0.1
// Author: Felladrin
// Started: 2013-08-11
// Updated: 2016-01-16

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Network;

namespace Felladrin.Commands
{
    public static class ShowOff
    {
        public static class Config
        {
            public static bool Enabled = true;                         // Is this command enabled?
            public static TimeSpan Delay = TimeSpan.FromSeconds(5);    // How long should we keep showing off each player?
        }

        public static void Initialize()
        {
            if (Config.Enabled)
            {
                CommandSystem.Register("ShowOff", AccessLevel.Counselor, new CommandEventHandler(OnCommand));
                EventSink.Login += OnLogin;
            }
        }

        static List<Mobile> m_TargetList = new List<Mobile>();

        [Usage("ShowOff")]
        [Description("Cycles through online players showing off each of them for a few seconds.")]
        static void OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m.BodyMod != 897)
            {
                CollectTargets();

                if (m_TargetList.Count == 0)
                {
                    m.SendMessage(33, "To use this command, there must be at least one player online. Staff members don't count.");
                }
                else
                {
                    m.SendMessage(67, "You start the show off. To stop, use this command again.");
                    m.BodyMod = 897;
                    Cycle(m);
                }
            }
            else
            {
                m.SendMessage(67, "You stop the show off.");
                m.BodyMod = 0;
            }
        }

        static void OnLogin(LoginEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m.BodyMod == 897)
                m.BodyMod = 0;
        }

        static void Cycle(Mobile staff)
        {
            if (staff.BodyMod != 897)
                return;
            
            if (m_TargetList.Count == 0)
            {
                CollectTargets();

                if (m_TargetList.Count == 0)
                {
                    staff.SendMessage(33, "No more players online to follow.");
                    staff.BodyMod = 0;
                    return;
                }
            }

            Mobile player = m_TargetList[Utility.Random(m_TargetList.Count)];

            m_TargetList.Remove(player);

            if (!player.Deleted && player.NetState != null && player.NetState.Running)
            {
                staff.Hidden = true;
                
                staff.MoveToWorld(player.Location, player.Map);

                Timer.DelayCall(Config.Delay, delegate
                    {
                        if (!staff.Deleted && staff.NetState != null && staff.NetState.Running)
                        {
                            Cycle(staff);
                        }
                    });
            }
            else
            {
                Cycle(staff);
            }
        }

        static void CollectTargets()
        {
            foreach (NetState ns in NetState.Instances)
            {
                PlayerMobile m = ns.Mobile as PlayerMobile;

                if (m != null && m.AccessLevel == AccessLevel.Player)
                {
                    m_TargetList.Add(m);
                }
            }
        }
    }
}