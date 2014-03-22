//   ___|========================|___
//   \  |  Written by Felladrin  |  /   This script was released on RunUO Community under the GPL licensing terms.
//    > |      August 2013       | <
//   /__|========================|__\   [Chat Initializer] - Current version: 1.0 (August 12, 2013)
//
//   To avoid the message "Chat is not currently supported" on login, remove the file Scripts/Engines/Chat/Chatold.cs

using Server.Commands;
using Server.Accounting;

namespace Server.Engines.Chat
{
    class ChatInitializer
    {
        public static void Initialize()
        {
            CommandSystem.Register("Chat", AccessLevel.Player, new CommandEventHandler(OpenChat_OnCommand));
            EventSink.Login += new LoginEventHandler(EventSink_Login);
            EventSink.Logout += new LogoutEventHandler(EventSink_Logout);
            EventSink.Disconnected += new DisconnectedEventHandler(EventSink_Disconnected);
            EventSink.ChatRequest += new ChatRequestEventHandler(EventSink_ChatRequest);
        }

        [Usage("Chat")]
        [Description("Enables or Disables the Chat.")]
        private static void OpenChat_OnCommand(CommandEventArgs args)
        {
            Mobile player = args.Mobile;

            if (player.Account == null)
            {
                player.SendMessage(38, "You have no account! Report immediately to the staff!");
                return;
            }

            Account account = player.Account as Account;

            if (account.GetTag("ChatInitializer") != "OK")
            {
                account.SetTag("ChatInitializer", "OK");
                player.SendMessage(68, "Chat: ENABLED");
                DisplayChatTo(player);
            }
            else
            {
                account.SetTag("ChatInitializer", "NO");
                player.SendMessage(38, "Chat: DISABLED");
                HideChatFrom(player);
            }
        }

        private static void EventSink_Login(LoginEventArgs args)
        {
            Mobile player = args.Mobile;

            if (player.Account == null)
                return;

            Account account = player.Account as Account;

            DefineChatName(player);

            if (account.GetTag("ChatInitializer") == "OK")
            {
                DisplayChatTo(player);
            }
        }

        private static void EventSink_Logout(LogoutEventArgs args)
        {
            Mobile player = args.Mobile;
            HideChatFrom(player);
        }

        private static void EventSink_Disconnected(DisconnectedEventArgs args)
        {
            Mobile player = args.Mobile;
            HideChatFrom(player);
        }

        private static void EventSink_ChatRequest(ChatRequestEventArgs e)
        {
            Mobile player = e.Mobile;

            if (player.Account == null)
                return;

            Account account = player.Account as Account;

            if (account.GetTag("ChatInitializer") != "OK")
            {
                account.SetTag("ChatInitializer", "OK");
                player.SendMessage(68, "Chat: ENABLED");
                DisplayChatTo(player);
            }
        }

        private static void DisplayChatTo(Mobile player)
        {
            if (player.Account == null)
                return;

            Account account = player.Account as Account;
            string accountChatName = account.GetTag("ChatName");
            ChatSystem.SendCommandTo(player, ChatCommand.OpenChatWindow, accountChatName);
            ChatUser.AddChatUser(player);
        }

        private static void HideChatFrom(Mobile player)
        {
            if (player.Account == null)
                return;

            Account account = player.Account as Account;
            ChatSystem.SendCommandTo(player, ChatCommand.CloseChatWindow);
            ChatUser user = ChatUser.GetChatUser(player);
            ChatUser.RemoveChatUser(user);
        }

        private static void DefineChatName(Mobile player)
        {
            if (player.Account == null)
                return;

            Account account = player.Account as Account;
            string accountChatName = (player.RawName).Replace(" ", "");
            account.SetTag("ChatName", accountChatName);
        }
    }
}
