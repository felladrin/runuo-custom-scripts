// History Command v1.0.1
// Author: Felladrin
// Started: 2016-01-06
// Updated: 2016-01-09

using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Commands;
using Server.Engines.Help;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Felladrin.Commands
{
    public static class History
    {
        public static class Config
        {
            public static bool Enabled = true;               // Is this command enabled?
            public static bool AutoRefreshEnabled = true;    // Is the auto refresh enabled?
            public static bool AutoColoredNames = true;      // Should we auto color the players names?
            public static bool OpenGumpOnLogin = true;       // Should we display the gump when player logs in?
            public static int MaxMessagesPerPage = 19;       // How many messages should we display per page?
        }

        public static void Initialize()
        {
            if (Config.Enabled && SpeechLog.Enabled)
            {
                CommandSystem.Register("History", AccessLevel.Player, new CommandEventHandler(OnCommand));

                if (Config.OpenGumpOnLogin)
                {
                    EventSink.Login += OnLogin;
                }
            }
        }

        [Usage("History")]
        [Description("Opens a gump with the history of everything you've said and heard from other players.")]
        static void OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
            SpeechLog log = pm.SpeechLog ?? new SpeechLog();

            pm.CloseGump(typeof(HistoryGump));

            if (Config.AutoRefreshEnabled)
            {
                pm.CloseGump(typeof(HistoryAutoRefreshGump));
                pm.SendGump(new HistoryAutoRefreshGump(pm, log));
            }
            else
            {
                pm.SendGump(new HistoryGump(pm, log));
            }
        }

        public static void OnLogin(LoginEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
            SpeechLog log = pm.SpeechLog ?? new SpeechLog();

            if (Config.AutoRefreshEnabled)
            {
                pm.SendGump(new HistoryAutoRefreshGump(pm, log));
            }
            else
            {
                pm.SendGump(new HistoryGump(pm, log));
            }
        }

        public static void Refresh(Mobile m)
        {
            PlayerMobile pm = m as PlayerMobile;

            if (pm != null && pm.HasGump(typeof(HistoryAutoRefreshGump)))
            {
                pm.CloseGump(typeof(HistoryAutoRefreshGump));
                pm.SendGump(new HistoryAutoRefreshGump(pm, pm.SpeechLog ?? new SpeechLog()));
            }
        }

        public class HistoryGump : Gump
        {
            public virtual string Title { get { return "History - Auto Refresh: OFF"; } }

            Mobile m_Player;
            List<SpeechLogEntry> m_Log;
            int m_Page;

            public HistoryGump(Mobile player, SpeechLog log) : this(player, new List<SpeechLogEntry>(log)) { }

            HistoryGump(Mobile player, List<SpeechLogEntry> log) : this(player, log, 0) { }

            HistoryGump(Mobile player, List<SpeechLogEntry> log, int page) : base(500, 30)
            {
                m_Player = player;
                m_Log = log;
                m_Page = page;

                AddImageTiled(0, 0, 300, 425, 0xA40);
                AddAlphaRegion(1, 1, 298, 423);

                string playerName = player.Name;

                AddHtml(10, 10, 280, 20, String.Format("<basefont color=#A0A0FF><center>{0}</i></center></basefont>", Title), false, false);

                int lastPage = (log.Count - 1) / Config.MaxMessagesPerPage;

                string sLog;

                if (page < 0 || page > lastPage || m_Log.Count == 0)
                {
                    sLog = "You didn't say anything yet, nor heard any other player. Don't be shy, say hi!";
                }
                else
                {
                    int bottom = log.Count - page * Config.MaxMessagesPerPage;
                    int top = Math.Max( bottom - Config.MaxMessagesPerPage, 0 );

                    StringBuilder builder = new StringBuilder();

                    for (int i = bottom-1; i >= top; i--)
                    {
                        SpeechLogEntry entry = log[i];

                        Mobile m = entry.From;

                        string color = (Config.AutoColoredNames ? (m.Name.GetHashCode() >> 8).ToString() : "FFF");
                        string name = m.Name;
                        string speech = entry.Speech;
                        string time = entry.Created.ToString("HH:mm");

                        if (i != bottom-1)
                        {
                            builder.Append("<br/>");
                        }

                        builder.AppendFormat("[{0}] <basefont color=#{1}>{2}<basefont color=white> {3} ", time, color, name, Utility.FixHtml(speech));
                    }

                    sLog = builder.ToString();
                }

                AddHtml(10, 40, 280, 350, sLog, false, true);

                if (page > 0)
                {
                    AddButton(10, 395, 0xFAE, 0xFB0, 1, GumpButtonType.Reply, 0); // Previous page
                }

                AddLabel(45, 395, 0x481, String.Format("Current page: {0}/{1}", page + 1, lastPage + 1));

                if (page < lastPage)
                {
                    AddButton(261, 395, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0); // Next page
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;

                switch (info.ButtonID)
                {
                    case 1: // Previous page
                        {
                            if (m_Page - 1 >= 0)
                            {
                                from.SendGump(new HistoryGump(m_Player, m_Log, m_Page - 1));
                            }

                            break;
                        }
                    case 2: // Next page
                        {
                            if ((m_Page + 1) * Config.MaxMessagesPerPage < m_Log.Count)
                            {
                                from.SendGump(new HistoryGump(m_Player, m_Log, m_Page + 1));
                            }

                            break;
                        }
                }
            }
        }

        public class HistoryAutoRefreshGump : HistoryGump
        {
            public override string Title { get { return "History - Auto Refresh: ON"; } }
            public HistoryAutoRefreshGump(Mobile player, SpeechLog log) : base(player, log) {}
        }
    }
}