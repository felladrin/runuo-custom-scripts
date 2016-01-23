// Global Chat v1.4.0
// Author: Felladrin
// Started: 2013-07-03
// Updated: 2016-01-06

using System.Collections.Generic;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Felladrin.Engines
{
    public static class GlobalChat
    {
        public static class Config
        {
            public static bool Enabled = true;               // Is this system enabled?
            public static bool OpenHistoryOnLogin = true;    // Should we display the history when player logs in?
            public static bool AutoColoredNames = true;      // Should we auto color the players names?
            public static int HistorySize = 50;              // How many messages should we keep in the history?
            public static int MessageHue = 1154;             // What is the hue of the chat messages?
        }

        public static void Initialize()
        {
            if (Config.Enabled)
            {
                CommandSystem.Register("ChatToggle", AccessLevel.Player, new CommandEventHandler(OnCommandToggle));
                CommandSystem.Register("ChatHistory", AccessLevel.Player, new CommandEventHandler(OnCommandHistory));
                CommandSystem.Register("C", AccessLevel.Player, new CommandEventHandler(OnCommandChat));
                EventSink.Login += OnLogin;
            }
        }

        static readonly List<string> History = new List<string>();

        static HashSet<int> DisabledPlayers = new HashSet<int>();

        [Usage("ChatToggle")]
        [Description("Enables or Disables the Chat.")]
        static void OnCommandToggle(CommandEventArgs e)
        {
            var pm = e.Mobile as PlayerMobile;
            var acc = pm.Account as Account;

            if (acc.GetTag("Chat") == null || acc.GetTag("Chat") == "Enabled")
            {
                DisabledPlayers.Add(pm.Serial.Value);
                acc.SetTag("Chat", "Disabled");
                pm.SendMessage(38, "You have disabled the chat for your account.");

                if (pm.HasGump(typeof(ChatHistoryGump)))
                    pm.CloseGump(typeof(ChatHistoryGump));
            }
            else
            {
                DisabledPlayers.Remove(pm.Serial.Value);
                acc.SetTag("Chat", "Enabled");
                pm.SendMessage(68, "You have enabled the chat for your account.");
                pm.SendGump(new ChatHistoryGump());
            }
        }

        [Usage("ChatHistory")]
        [Description("Opens the Chat History.")]
        static void OnCommandHistory(CommandEventArgs e)
        {
            var pm = e.Mobile as PlayerMobile;

            if (DisabledPlayers.Contains(pm.Serial.Value))
            {
                pm.SendMessage(38, "Chat is currently disabled for your account. Type [ChatToggle to enable it.");
                return;
            }

            if (pm.HasGump(typeof(ChatHistoryGump)))
                pm.CloseGump(typeof(ChatHistoryGump));

            pm.SendGump(new ChatHistoryGump());
        }

        [Usage("C <message>")]
        [Description("Broadcasts a message to all players online. If no message is provided, it opens the Chat History.")]
        static void OnCommandChat(CommandEventArgs e)
        {
            var pm = e.Mobile as PlayerMobile;

            if (DisabledPlayers.Contains(pm.Serial.Value))
            {
                pm.SendMessage(38, "Chat is currently disabled for your account. Type [ChatToggle to enable it.");
                return;
            }

            if (e.ArgString.Length == 0)
            {
                if (pm.HasGump(typeof(ChatHistoryGump)))
                    pm.CloseGump(typeof(ChatHistoryGump));

                pm.SendGump(new ChatHistoryGump());
            }
            else
            {
                Broadcast(e.Mobile, e.ArgString);
            }
        }

        static void OnLogin(LoginEventArgs e)
        {
            var pm = e.Mobile as PlayerMobile;
            var acc = pm.Account as Account;

            if (acc.GetTag("Chat") == "Disabled")
            {
                DisabledPlayers.Add(pm.Serial.Value);
                pm.SendMessage("Chat is Disabled for your account.");
            }
            else
            {
                pm.SendMessage("Chat is Enabled for your account.");

                if (Config.OpenHistoryOnLogin)
                    pm.SendGump(new ChatHistoryGump());
            }
        }

        static void Broadcast(Mobile sender, string message)
        {
            if (History.Count > Config.HistorySize)
                History.RemoveAt(0);

            History.Add(string.Format("[{0}] <basefont color=#{1}>{2}<basefont color=white> {3}", System.DateTime.UtcNow.ToString("HH:mm"), (Config.AutoColoredNames ? (sender.Name.GetHashCode()>>8).ToString() : "FFF"), sender.Name, Utility.FixHtml(message)));

            foreach (NetState ns in NetState.Instances)
            {
                var player = ns.Mobile as PlayerMobile;

                if (player == null || DisabledPlayers.Contains(player.Serial.Value))
                    continue;

                player.SendMessage(Config.MessageHue, string.Format("<{0}> {1}", sender.Name, message));

                if (player.HasGump(typeof(ChatHistoryGump)))
                {
                    player.CloseGump(typeof(ChatHistoryGump));
                    player.SendGump(new ChatHistoryGump());
                }
            }
        }

        static string GenerateHistoryHTML()
        {
            if (History.Count == 0)
                return "No messages were sent since the last restart.";

            string HTML = "";

            foreach (string msg in History)
                HTML = msg + " <br/>" + HTML;

            return HTML;
        }

        public class ChatHistoryGump : Gump
        {
            public ChatHistoryGump() : base(110, 100)
            {
                Closable = true;
                Dragable = true;
                Disposable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(0, 0, 420, 250, 5054);
                AddImageTiled(10, 10, 400, 20, 2624);
                AddAlphaRegion(10, 10, 400, 20);
                AddLabel(15, 10, 73, "Global Chat History - Viewing the last " + Config.HistorySize + " messages");
                AddImageTiled(10, 40, 400, 200, 2624);
                AddAlphaRegion(10, 40, 400, 200);
                AddHtml(15, 40, 395, 200, GenerateHistoryHTML(), false, true);
            }
        }
    }
}