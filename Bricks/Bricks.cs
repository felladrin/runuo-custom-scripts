// Bricks v1.1.1
// Author: Felladrin
// Started: 2013-08-02
// Updated: 2016-01-23
// Credits: Inspired by the homonymous script released by Gacoperz on RunUO Forums in 2007.

using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;
using Server.Regions;
using Server.Targeting;

namespace Felladrin.Engines.Bricks
{
    public static class BricksDemolitionCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("BricksDemolition", AccessLevel.GameMaster, new CommandEventHandler(OnCommandBricksDemolition));
        }

        [Usage("BricksDemolition")]
        [Description("Wipes all Bricks Items from the world.")]
        static void OnCommandBricksDemolition(CommandEventArgs e)
        {
            var deletedItems = 0;

            foreach (Item i in new List<Item>(World.Items.Values))
            {
                if (i is Brick || i is BaseProtoBrick || i is ProtoBrickBox || i is BrickLifter || i is BrickFillister || i is BricklayerBox)
                {
                    i.Delete();
                    deletedItems++;
                }
            }

            if (deletedItems > 0)
                e.Mobile.SendMessage("All the {0} Bricks Items have been removed from the world.", deletedItems);
            else
                e.Mobile.SendMessage("There are no Bricks Items in the world.");
        }
    }

    public class BaseProtoBrick : Item, IDyable
    {
        int MinItemID;
        int MaxItemID;

        public override bool Decays { get { return false; } }

        public BaseProtoBrick(string name, int itemID)
            : base(itemID)
        {
            Name = name;
            MinItemID = itemID;
            MaxItemID = itemID + 20;
            Weight = 0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add("Single Click to Transform & Double Click to Duplicate");
        }

        public override void OnAosSingleClick(Mobile from)
        {
            ItemID++;

            if (ItemID > MaxItemID)
                ItemID = MinItemID;
        }

        public override void OnSingleClick(Mobile from)
        {
            OnAosSingleClick(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.AddToBackpack(new Brick(ItemID, Hue, from));
        }

        public override bool OnDroppedToWorld(Mobile from, Point3D p)
        {
            if (from.Region.IsPartOf(typeof(TownRegion)))
            {
                from.SendMessage(33, "You cannot place bricks on town regions.");
                return false;
            }

            return base.OnDroppedToWorld(from, p);
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Parent != null)
            {
                Hue = sender.DyedHue;
                return true;
            }

            from.SendMessage("You cannot dye bricks from the ground.");
            return false;
        }

        public BaseProtoBrick(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.WriteEncodedInt(MinItemID);
            writer.WriteEncodedInt(MaxItemID);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            MinItemID = reader.ReadEncodedInt();
            MaxItemID = reader.ReadEncodedInt();
        }
    }

    public class WornSandProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public WornSandProtoBrick() : base("Worn Sand Brick", 0x03EE) { }

        public WornSandProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class MarbleProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public MarbleProtoBrick() : base("Marble Brick", 0x0709) { }

        public MarbleProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class StoneProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public StoneProtoBrick() : base("Stone Brick", 0x071E) { }

        public StoneProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class LightWoodProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public LightWoodProtoBrick() : base("Light Wood Brick", 0x0721) { }

        public LightWoodProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class WoodProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public WoodProtoBrick() : base("Wood Brick", 0x0738) { }

        public WoodProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class LightStoneProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public LightStoneProtoBrick() : base("Light Stone Brick", 0x0750) { }

        public LightStoneProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class SandStoneProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public SandStoneProtoBrick() : base("Sand Stone Brick", 0x076C) { }

        public SandStoneProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class DarkStoneProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public DarkStoneProtoBrick() : base("Dark Stone Brick", 0x0788) { }

        public DarkStoneProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class BrickProtoBrick : BaseProtoBrick
    {
        [Constructable]
        public BrickProtoBrick() : base("Brick Brick", 0x07A3) { }

        public BrickProtoBrick(Serial serial) : base(serial) { }

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
    }

    public class Brick : Item, IDyable
    {
        public Brick(int itemID, int hue, Mobile owner)
        {
            Name = "Brick";
            ItemID = itemID;
            Owner = owner;
            Hue = hue;
            Weight = 0;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get; set; }

        public override bool Decays { get { return false; } }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add("Single Click to Delete & Double Click to Lockdown");
        }

        public override void OnAosSingleClick(Mobile from)
        {
            if (!Movable)
            {
                from.SendMessage("You need to release this block and move it to your backpack to delete it. Double-click to release.");
                return;
            }

            Delete();
        }

        public override void OnSingleClick(Mobile from)
        {
            OnAosSingleClick(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Parent != null)
            {
                from.SendMessage("That must be on the ground for you to lock it down.");
                return;
            }

            if (from != Owner)
            {
                from.SendMessage("Only the creator of this brick can interact with it.");
                return;
            }

            Movable = !Movable;
        }

        public override bool OnDroppedToWorld(Mobile from, Point3D p)
        {
            if (from.Region.IsPartOf(typeof(TownRegion)))
            {
                from.SendMessage(33, "You cannot place bricks on town regions.");
                return false;
            }

            return base.OnDroppedToWorld(from, p);
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            if (RootParent is Mobile && from != RootParent)
                return false;

            if (Movable || (from == Owner && !Movable))
            {
                Hue = sender.DyedHue;
                return true;
            }

            return false;
        }

        public Brick(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(Owner);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            Owner = reader.ReadMobile();
        }
    }

    public sealed class ProtoBrickBox : LargeCrate
    {
        [Constructable]
        public ProtoBrickBox()
        {
            Name = "Box of Bricks";
            Weight = 0;

            DropItem(new WornSandProtoBrick());
            DropItem(new MarbleProtoBrick());
            DropItem(new StoneProtoBrick());
            DropItem(new LightWoodProtoBrick());
            DropItem(new WoodProtoBrick());
            DropItem(new LightStoneProtoBrick());
            DropItem(new SandStoneProtoBrick());
            DropItem(new DarkStoneProtoBrick());
            DropItem(new BrickProtoBrick());
            DropItem(new BrickLifter());
            DropItem(new BrickFillister());
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add("Event Construction Set");
        }

        public override bool OnDroppedToWorld(Mobile from, Point3D p)
        {
            if (from.Region.IsPartOf(typeof(TownRegion)))
            {
                from.SendMessage(33, "You cannot place bricks on town regions.");
                return false;
            }

            return base.OnDroppedToWorld(from, p);
        }

        public override bool Decays { get { return false; } }

        public ProtoBrickBox(Serial serial) : base(serial) { }

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
    }

    public class BrickLifter : Item
    {
        [Constructable]
        public BrickLifter()
        {
            Name = "Brick Lifter";
            ItemID = 7867;
            Weight = 0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add("Use it to move a brick up");
        }

        public override bool OnDroppedToWorld(Mobile from, Point3D p)
        {
            if (from.Region.IsPartOf(typeof(TownRegion)))
            {
                from.SendMessage(33, "You cannot place bricks on town regions.");
                return false;
            }

            return base.OnDroppedToWorld(from, p);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.Target = new InternalTarget();
        }

        class InternalTarget : Target
        {
            public InternalTarget() : base(-1, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Brick && ((Brick)targeted).Owner == from)
                {
                    var brick = targeted as Brick;
                    brick.Location = new Point3D(brick.Location, brick.Z + 1);
                }
                else
                {
                    from.SendMessage("You can only use it on bricks, and only on those you have built.");
                }

                from.Target = new InternalTarget();
            }
        }

        public override bool Decays { get { return false; } }

        public BrickLifter(Serial serial) : base(serial) { }

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
    }

    public class BrickFillister : Item
    {
        [Constructable]
        public BrickFillister()
        {
            Name = "Brick Fillister";
            ItemID = 13048;
            Weight = 0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add("Use it to move a brick down");
        }

        public override bool OnDroppedToWorld(Mobile from, Point3D p)
        {
            if (from.Region.IsPartOf(typeof(TownRegion)))
            {
                from.SendMessage(33, "You cannot place bricks on town regions.");
                return false;
            }

            return base.OnDroppedToWorld(from, p);
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.Target = new InternalTarget();
        }

        class InternalTarget : Target
        {
            public InternalTarget() : base(-1, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Brick && ((Brick)targeted).Owner == from)
                {
                    var brick = targeted as Brick;
                    brick.Location = new Point3D(brick.Location, brick.Z - 1);
                }
                else
                {
                    from.SendMessage("You can only use it on bricks, and only on those you have built.");
                }

                from.Target = new InternalTarget();
            }
        }

        public override bool Decays { get { return false; } }

        public BrickFillister(Serial serial) : base(serial) { }

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
    }

    [Flipable(0x2811, 0x2812)]
    public sealed class BricklayerBox : Item
    {
        [Constructable]
        public BricklayerBox()
        {
            Name = "Bricklayer's Box";
            Movable = false;
            ItemID = 0x2811;
            Weight = 0;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(Name);
            list.Add("Get your Box of Bricks here!");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Backpack.FindItemByType(typeof(ProtoBrickBox)) != null)
            {
                from.SendMessage(67, "You've already got your box!");
                return;
            }

            from.AddToBackpack(new ProtoBrickBox());
        }

        public override bool Decays { get { return false; } }

        public BricklayerBox(Serial serial) : base(serial) { }

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
    }
}