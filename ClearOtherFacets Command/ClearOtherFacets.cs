// ClearOtherFacets Command v1.0.1
// Author: Felladrin
// Started: 2010-12-20
// Updated: 2016-01-20

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Felladrin.Commands
{
    public static class ClearOtherFacets
    {
        public static void Initialize()
        {
            CommandSystem.Register("ClearOtherFacets", AccessLevel.Administrator, new CommandEventHandler(OnCommandClearOtherFacets));
        }

        [Usage("ClearOtherFacets")]
        [Description("Deletes all items and mobiles that are not in your facet. Players and their inventory will not be deleted.")]
        public static void OnCommandClearOtherFacets(CommandEventArgs e)
        {
            Map map = e.Mobile.Map;

            if (map == null || map == Map.Internal)
            {
                e.Mobile.SendMessage("You may not run that command here.");
                return;
            }

            var list = new List<IEntity>();

            foreach (Item item in World.Items.Values)
                if (item.Map != map && item.Parent == null)
                    list.Add(item);

            foreach (Mobile m in World.Mobiles.Values)
                if (m.Map != map && !m.Player)
                    list.Add(m);

            if (list.Count > 0)
                e.Mobile.SendGump( new WarningGump(1060635, 30720, String.Format("You are about to delete all the {0} object{1} from other facets.\n\n{2} will remain intact.\n\nDo you really wish to continue?", list.Count, list.Count == 1 ? "" : "s", map), 0xFFC000, 360, 260, new WarningGumpCallback(DeleteListCallback), list));
            else
                e.Mobile.SendMessage("There were no objects found to delete.");
        }

        public static void DeleteListCallback(Mobile from, bool okay, object state)
        {
            if (okay)
            {
                var list = (List<IEntity>)state;

                CommandLogging.WriteLine(from, "{0} {1} deleting {2} object{3}", from.AccessLevel, CommandLogging.Format(from), list.Count, list.Count == 1 ? "" : "s");

                NetState.Pause();

                for (int i = 0; i < list.Count; ++i)
                    list[i].Delete();

                NetState.Resume();

                from.SendMessage("You have deleted {0} object{1}.", list.Count, list.Count == 1 ? "" : "s");
            }
            else
            {
                from.SendMessage("You have chosen not to delete those objects.");
            }
        }
    }
}