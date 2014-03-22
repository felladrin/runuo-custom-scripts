//   ___|========================|___
//   \  |  Written by Felladrin  |  /   This script was released on RunUO Forums under the GPL licensing terms.
//    > |       March 2010       | <
//   /__|========================|__\   [Gobal Donation Box] - Current version: 2.1 (August 29, 2013)

using System;
using System.Collections;

using Server.Items;
using Server.Mobiles;
using Server.Commands;
using Server.Targeting;
using Server.Accounting;

namespace Server.Commands
{
    public class DonationSystem
    {
        public static class Config
        {
            public static bool OnlyYoungs = false;                                 // Change this to true if you want only youngs to be able to open the box.
            public static int DonationsLimit = 0;                                  // Choose the maximum number of times each player can open the box. Leave it as 0 (zero) to make it unlimited.
            public static bool CanBeGloballyAccessed = false;                      // Can players use commands to get items from the box wherever they are?
            public static bool AutoOrganizerEnabled = true;                        // Should the items in the box be automatically organized in categories?
            public static TimeSpan AutoOrganizerDelay = TimeSpan.FromMinutes(10);  // How often should the box be automatically organized?
        }

        public static void Initialize()
        {
            CommandSystem.Register("Donate", AccessLevel.Player, new CommandEventHandler(Donate_OnCommand));
            CommandSystem.Register("Donations", Config.CanBeGloballyAccessed == true ? AccessLevel.Player : AccessLevel.Administrator, new CommandEventHandler(Donations_OnCommand));

            if (Config.AutoOrganizerEnabled)
                new AutoOrganizer().Start();
        }

        [Usage("Donate")]
        [Description("Used to donate items from your backpack.")]
        public static void Donate_OnCommand(CommandEventArgs e)
        {
            if (box == null)
                VerifyExistence();

            e.Mobile.Target = new SelectItem();
            e.Mobile.SendMessage(Utility.RandomMinMax(2, 600), "Select the items you want to donate.");
            e.Mobile.SendMessage(67, "Press ESC when you finish.");
        }

        [Usage("Donations")]
        [Description("Opens the Donation Box, wherever you are, and allow you to get items via target.")]
        public static void Donations_OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (box == null)
                VerifyExistence();

            if (pm.AccessLevel >= AccessLevel.Administrator)
            {
                box.MoveToWorld(pm.Location, pm.Map);
                box.Open(pm);
            }
            else
            {
                if (VerifyLimitations(pm))
                {
                    box.MoveToWorld(pm.Location, pm.Map);
                    box.Open(pm);
                    pm.SendMessage(Utility.RandomMinMax(2, 600), "Select the items you want to take from the box.");
                    pm.SendMessage(67, "Press ESC when you finish.");
                    pm.Target = new TakeDonation();
                }
            }
        }

        public static DonationBox box;

        public static void VerifyExistence()
        {
            bool exist = false;

            foreach (Item item in World.Items.Values)
            {
                if (item is DonationBox)
                {
                    box = item as DonationBox;
                    exist = true;
                }
            }

            if (!exist)
            {
                if (Config.CanBeGloballyAccessed)
                    box = new DonationBox(true);
                else
                    box = new DonationBox();
            }
        }

        private class SelectItem : Target
        {
            public SelectItem() : base(-1, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item item = targeted as Item;

                    if (item is DonationBackpack)
                    {
                        box.DropItem(item);
                        from.Target = new SelectItem();
                    }
                    else if (from.AccessLevel >= AccessLevel.Administrator && item.Movable != false)
                    {
                        box.DropItem(item);
                        from.Target = new SelectItem();
                    }
                    else if (item.IsChildOf(from.Backpack) && item.Movable != false)
                    {
                        box.DropItem(item);
                        Emotionate(from);
                        from.Target = new SelectItem();
                    }
                    else
                    {
                        from.SendMessage(67, "You can only donate items from your backpack.");
                        from.Target = new SelectItem();
                    }
                }
                else
                {
                    from.SendMessage(33, "You can only donate items!");
                    from.Target = new SelectItem();
                }
            }
        }

        public static void Emotionate(Mobile from)
        {
            from.SendMessage(Utility.RandomMinMax(2, 600), "Thanks for your donation!");
            from.PlaySound(from.Female ? 823 : 1097); from.PlaySound(1051);
            from.Emote("*donates an item*");
        }

        public static bool VerifyLimitations(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm.AccessLevel == AccessLevel.Player)
            {
                if (Config.OnlyYoungs && !(pm.Young))
                {
                    pm.SendMessage(32, "Only the young players can get items from the donation box.");
                    return false;
                }

                if (Config.DonationsLimit != 0)
                {
                    int DontationsTaken;

                    try { DontationsTaken = int.Parse(((Account)pm.Account).GetTag("DonationBox")); }
                    catch { DontationsTaken = 1; }

                    if (DontationsTaken > Config.DonationsLimit)
                    {
                        pm.SendMessage(Utility.RandomMinMax(2, 600), "You have already taken many items, please let the other new players enjoy it too.");
                        return false;
                    }
                    else
                    {
                        if ((Config.DonationsLimit - DontationsTaken) > 0)
                            pm.SendMessage(Utility.RandomMinMax(2, 600), "You can use the donation box {0} more time{1}.", (Config.DonationsLimit - DontationsTaken), (Config.DonationsLimit - DontationsTaken) == 1 ? "" : "s");
                        else
                            pm.SendMessage(Utility.RandomMinMax(2, 600), "After you close the donation box, you won't be able to use it anymore, so take a good look on the items, get those you think you'll need, and then start donating to help other players.");
                    }

                    ((Account)pm.Account).SetTag("DonationBox", (DontationsTaken + 1).ToString());
                }
            }

            return true;
        }

        private class TakeDonation : Target
        {
            public TakeDonation() : base(-1, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    Item item = targeted as Item;

                    if (item is DonationBackpack)
                    {
                        ((DonationBackpack)item).Open(from);
                        from.Target = new TakeDonation();
                    }
                    else if (item.Parent is DonationBackpack || item.Parent is DonationBox)
                    {
                        from.AddToBackpack(item);
                        from.Target = new TakeDonation();
                    }
                    else
                    {
                        from.SendMessage(67, "You can only use this command to take items from the donation box.");
                        from.Target = new TakeDonation();
                    }
                }
                else
                {
                    from.SendMessage(33, "You can only use this command to take items from the donation box.");
                    from.Target = new TakeDonation();
                }
            }
        }

        public class AutoOrganizer : Timer
        {
            public AutoOrganizer() : base(TimeSpan.Zero, Config.AutoOrganizerDelay) { }

            protected override void OnTick()
            {
                if (box == null)
                    VerifyExistence();

                ArrayList itemlist = new ArrayList();

                foreach (Item item in box.Items)
                {
                    if (!(item is DonationBackpack))
                        itemlist.Add(item);
                    else
                    {
                        foreach (Item subitem in item.Items)
                        {
                            if (subitem is Container)
                            {
                                foreach (Item iteminside in ((Container)subitem).Items)
                                    itemlist.Add(iteminside);
                            }
                            else
                                itemlist.Add(subitem);
                        }
                    }
                }

                foreach (Item item in itemlist)
                {
                    bool exist = false;

                    string name;

                    if (item is BaseRanged)
                        name = "Ranged Weapons";
                    else if (item is BaseAxe)
                        name = "Axes";
                    else if (item is BaseKnife)
                        name = "Knives";
                    else if (item is BaseBashing)
                        name = "Maces";
                    else if (item is BasePoleArm)
                        name = "PoleArms";
                    else if (item is BaseSpear)
                        name = "Spears and Forks";
                    else if (item is BaseStaff)
                        name = "Staves";
                    else if (item is BaseSword)
                        name = "Swords";
                    else if (item is BaseWeapon || item is BaseMeleeWeapon)
                        name = "Other Weapons";
                    else if (item is BaseWand)
                        name = "Wands";
                    else if (item is BaseHat)
                        name = "Hats";
                    else if (item is BaseShield)
                        name = "Shields";
                    else if (item is BaseArmor)
                        name = "Armors";
                    else if (item is Gold || item is BankCheck)
                        name = "Gold";
                    else if (item is BaseClothing || item is Cloth)
                        name = "Clothes";
                    else if (item is BaseTool || item is Lockpick)
                        name = "Tools";
                    else if (item is Food || item is BaseBeverage)
                        name = "Food";
                    else if (item is Bolt || item is Arrow)
                        name = "Ammunition";
                    else if (item is BaseReagent)
                        name = "Reagents";
                    else if (item is BaseJewel)
                        name = "Jewels";
                    else if (item is SpellScroll || item is Spellbook)
                        name = "Spells";
                    else if (item is BaseOre || item is BaseScales || item is BaseIngot)
                        name = "Blacksmithing Resources";
                    else if (item is BaseHides || item is BaseLeather)
                        name = "Tailor Resources";
                    else if (item is BaseAddonDeed || item is BaseAddon)
                        name = "Addons";
                    else if (item is BaseBook)
                        name = "Books";
                    else if (item is BaseLight)
                        name = "Lights";
                    else if (item is BaseContainer)
                        name = "Containers";
                    else if (item is BasePotion || item is Bandage)
                        name = "Potions";
                    else if (item is BaseInstrument)
                        name = "Instruments";
                    else
                        name = "Miscellaneous";

                    ArrayList packlist = new ArrayList(box.FindItemsByType(typeof(DonationBackpack)));

                    foreach (DonationBackpack backpack in packlist)
                    {
                        if (backpack.Name == name)
                        {
                            backpack.DropItem(item);
                            exist = true;
                        }

                        if (backpack.TotalItems == 0)
                            backpack.Delete();
                    }

                    if (!exist)
                    {
                        DonationBackpack newdb = new DonationBackpack();
                        newdb.Name = name;
                        newdb.DropItem(item);
                        box.DropItem(newdb);
                    }
                }
            }
        }
    }
}

namespace Server.Items
{
    [Flipable(0x2811, 0x2812)]
    public class DonationBox : BaseContainer
    {
        public DonationBox(bool global)
            : base(0x1011)
        {
            Name = "The Donation Box";
            Movable = false;
            LiftOverride = true;
            Weight = 0;
        }

        public DonationBox()
            : base(0x2811)
        {
            Name = "The Donation Box";
            Movable = false;
            LiftOverride = true;
            Weight = 0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add("Donations: {0}", TotalItems);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (DonationSystem.VerifyLimitations(from))
                base.OnDoubleClick(from);
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            DonationSystem.Emotionate(from);
            return base.OnDragDrop(from, item);
        }

        public override void OnDelete()
        {
            DonationSystem.box = null;

            foreach (Mobile pm in World.Mobiles.Values)
            {
                if (pm is PlayerMobile)
                {
                    try
                    {
                        if (((Account)pm.Account).GetTag("DonationBox") != null)
                            ((Account)pm.Account).RemoveTag("DonationBox");
                    }
                    catch { }
                }
            }
        }

        public override int DefaultMaxWeight { get { return 0; } }

        public override int DefaultMaxItems { get { return 0; } }

        public override int DefaultGumpID { get { return 0x43; } }

        public override int DefaultDropSound { get { return 0x42; } }

        public override bool DisplayWeight { get { return false; } }

        public override bool Decays { get { return false; } }

        public DonationBox(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class DonationBackpack : Backpack
    {
        public DonationBackpack()
        {
            Movable = true;
            LiftOverride = true;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add("Containing {0} Item{1}", TotalItems, (TotalItems > 1 ? "s" : ""));
        }

        public override int DefaultMaxWeight { get { return 0; } }

        public override int DefaultMaxItems { get { return 0; } }

        public override bool DisplayWeight { get { return false; } }

        public override bool Decays { get { return false; } }

        public DonationBackpack(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
