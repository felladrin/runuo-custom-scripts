// Kudos v1.4.0
// Author: Felladrin
// Created at 2013-07-23
// Updated at 2016-01-18

using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Felladrin.Items
{
    public class Kudos : Item
    {
        public static class Config
        {
            public static bool Enabled = true;                              // Is the kudos auto distribution enabled?
            public static int MinutesOnline = 1;                            // Every X minutes we give kudos to all players online.
            public static bool DropOnBank = true;                           // Should we place the kudos on character's bankbox (true) or backpack (false)?
            public static AccessLevel MaxAccessLevel = AccessLevel.Player;  // Any character with this access and below receives kudos.
        }

        public static void Initialize()
        {
            if (Config.Enabled)
                new KudosAutoDistributionTimer().Start();
        }

        [Constructable]
        public Kudos(int amount)
        {
            Name = "Kudos";
            ItemID = 0xF11;
            Hue = 0x47E;
            Stackable = true;
            Amount = amount;
            Weight = 0;
        }

        [Constructable]
        public Kudos() : this(1) { }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Exchangeable Game Time Reward");
        }

        public override bool DisplayWeight { get { return false; } }

        public Kudos(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }

        class KudosAutoDistributionTimer : Timer
        {
            public KudosAutoDistributionTimer() : base(TimeSpan.Zero, TimeSpan.FromMinutes(Config.MinutesOnline))
            {
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                foreach (NetState state in NetState.Instances)
                {
                    PlayerMobile m = state.Mobile as PlayerMobile;

                    if (m != null && m.AccessLevel <= Config.MaxAccessLevel)
                    {
                        if (Config.DropOnBank && m.BankBox != null)
                        {
                            Item kudos = m.BankBox.FindItemByType(typeof(Kudos));

                            if (kudos != null)
                            {
                                kudos.Amount++;
                            }
                            else
                            {
                                m.BankBox.DropItem(new Kudos());
                            }
                        }
                        else if (m.Backpack != null)
                        {
                            Item kudos = m.Backpack.FindItemByType(typeof(Kudos));

                            if (kudos != null)
                            {
                                kudos.Amount++;
                            }
                            else
                            {
                                m.Backpack.DropItem(new Kudos());
                            }
                        }
                    }
                }
            }
        }
    }
}
