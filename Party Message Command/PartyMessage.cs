// Party Message Command v1.1.0
// Author: Felladrin
// Started: 2015-12-19
// Updated: 2016-01-22

using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Engines.PartySystem;

namespace Felladrin.Commands
{
    public static class PartyMessage
    {
        public static void Initialize()
        {
            CommandSystem.Register("P", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("P <message>")]
        [Description("Sends a message to your party. If no message is set, lists the party members names.")]
        static void OnCommand(CommandEventArgs e)
        {
            var from = e.Mobile;
            var message = e.ArgString;
            var party = Party.Get(from);
            
            if (from.Party == null)
            {
                from.SendLocalizedMessage(3000211); // You are not in a party.
                return;
            }

            if (message.Length > 0)
            {
                party.SendPublicMessage(from, message);
                return;
            }

            var leader = (from == party.Leader) ? "You are" : party.Leader.Name + " is";

            var fellows = new List<string>();

            foreach (PartyMemberInfo pmi in party.Members)
                if (from != pmi.Mobile)
                    fellows.Add(pmi.Mobile.Name);

            from.SendMessage("Your have {0} fellow{1} in your party: {2}. {3} the leader.", fellows.Count, (fellows.Count > 1 ? "s" : ""), string.Join(", ", fellows), leader);
        }
    }
}