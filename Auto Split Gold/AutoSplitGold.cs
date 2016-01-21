// Auto Split Gold v1.1.0
// Author: Felladrin
// Started: 2013-07-12
// Updated: 2016-01-20

// Istallation:
// On Scripts/Items/Misc/Corpses/Corpse.cs find the method:
// CheckLift(Mobile from, Item item, ref LRReason reject)
// Then, above its last 'return' statement, add the following line:
// if (Felladrin.Automations.AutoSplitGold.Split(from, item)) return false;

using Server;
using Server.Engines.PartySystem;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Felladrin.Automations
{
    public static class AutoSplitGold
    {
        public static bool Split(Mobile from, Item item)
        {
            if (!(item is Gold) || from.Party == null || item.Amount < ((Party)from.Party).Members.Count)
                return false;

            var party = Party.Get(from);

            int share = item.Amount / party.Members.Count;

            foreach (var info in party.Members)
            {
                var partyMember = info.Mobile as PlayerMobile;

                if (partyMember == null || partyMember.Backpack == null)
                    continue;

                var receiverGold = partyMember.Backpack.FindItemByType<Gold>();

                if (receiverGold != null)
                    receiverGold.Amount += share;
                else
                    partyMember.Backpack.DropItem(new Gold(share));

                partyMember.PlaySound(item.GetDropSound());

                if (partyMember == from)
                {
                    from.SendMessage("You take some gold from the corpse and share with the party: {0} for each.", share);

                    int rest = item.Amount % party.Members.Count;

                    if (rest > 0)
                    {
                        var sharerGold = from.Backpack.FindItemByType<Gold>();

                        if (sharerGold != null)
                            sharerGold.Amount += rest;
                        else
                            from.Backpack.DropItem(new Gold(rest));

                        from.SendMessage("You keep the {0} gold left over.", rest);
                    }
                }
                else
                {
                    partyMember.SendMessage("{0} takes some gold from the corpse and shares with the party: {1} for each.", from.Name, share);

                    if (WeightOverloading.IsOverloaded(partyMember))
                    {
                        receiverGold.Amount -= share;

                        var sharerGold = from.Backpack.FindItemByType<Gold>();

                        if (sharerGold != null)
                            sharerGold.Amount += share;
                        else
                            from.Backpack.DropItem(new Gold(share));

                        partyMember.SendMessage("But {0} keeps your share, because you are overloaded.", (from.Female ? "she" : "he"));

                        from.SendMessage("You keep {0}'s share, who is overloaded.", partyMember.Name);
                    }
                }
            }

            item.Delete();

            return true;
        }
    }
}
