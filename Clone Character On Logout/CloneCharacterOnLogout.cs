// Clone Character On Logout v1.1.0
// Author: Felladrin
// Started: 2016-01-25
// Updated: 2016-02-02

using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Mobiles;

namespace Felladrin.Automations
{
    public static class CloneCharacterOnLogout
    {
        public static class Config
        {
            public static bool Enabled = true;      // Is this system enabled?
            public static bool CanWander = true;    // Can the clones wander freely around the world or should they be frozen?
            public static bool CanTeach = true;     // Can other player train skills with this clone?
        }

        public static void Initialize()
        {
            if (Config.Enabled)
            {
                EventSink.Logout += OnLogout;
                EventSink.Login += OnLogin;
                CheckFirstRun();
            }
        }

        static void OnLogout(LogoutEventArgs e)
        {
            if (e.Mobile.AccessLevel == AccessLevel.Player)
                CreateCloneOf(e.Mobile);
        }

        static void OnLogin(LoginEventArgs e)
        {
            if (e.Mobile.AccessLevel == AccessLevel.Player)
                DeleteClonesOf(e.Mobile);
        }

        static void CreateCloneOf(Mobile m)
        {
            var characterClone = new CharacterClone(m);

            foreach (var itemOriginal in m.Items)
                if (itemOriginal.Parent == m && itemOriginal.Layer != Layer.Mount)
                    characterClone.AddItem(new ItemClone(itemOriginal));

            if (m.Mounted)
                new MountClone((BaseMount)m.Mount).Rider = characterClone;
        }

        static void DeleteClonesOf(Mobile m)
        {
            foreach (var mobile in new List<Mobile>(World.Mobiles.Values))
                if (mobile is CharacterClone && ((CharacterClone)mobile).Original == m)
                    mobile.Delete();
        }

        static void CheckFirstRun()
        {
            foreach (var mobile in World.Mobiles.Values)
                if (mobile is CharacterClone)
                    return;

            foreach (var mobile in new List<Mobile>(World.Mobiles.Values))
                if (mobile is PlayerMobile && mobile.Alive && mobile.AccessLevel == AccessLevel.Player)
                    CreateCloneOf(mobile);
        }

        public class CharacterClone : BaseCreature
        {
            [CommandProperty(AccessLevel.GameMaster)]
            public Mobile Original { get; set; }

            public CharacterClone(Mobile original) : base(Config.CanWander ? AIType.AI_Vendor : AIType.AI_Use_Default, FightMode.None, 10, 1, 0.2, 0.2)
            {
                Original = original;

                foreach (var property in (typeof(Mobile)).GetProperties())
                    if (property.CanRead && property.CanWrite)
                        property.SetValue(this, property.GetValue(Original, null), null);

                for (int i = 0, l = Original.Skills.Length; i < l; ++i)
                    Skills[i].Base = Original.Skills[i].Base;

                Player = false;

                if (Map == Map.Internal)
                    Map = LogoutMap;
            }

            public override void GetProperties(ObjectPropertyList list)
            {
                if (Original != null)
                    Original.GetProperties(list);
                else
                    base.GetProperties(list);
            }

            public override bool IsInvulnerable{ get{ return true; } }

            public override bool CanRegenHits { get { return false; } }

            public override bool CanTeach { get { return Config.CanTeach; } }

            public CharacterClone(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0);
                writer.Write(Original);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                reader.ReadInt();
                Original = reader.ReadMobile();

                if (Original == null)
                    Delete();
            }
        }

        public class ItemClone : Item
        {
            [CommandProperty(AccessLevel.GameMaster)]
            public Item Original { get; set; }

            public ItemClone(Item original)
            {
                Original = original;

                foreach (var property in (typeof(Item)).GetProperties())
                    if (property.CanRead && property.CanWrite)
                        property.SetValue(this, property.GetValue(original, null), null);

                Movable = false;
            }

            public override void GetProperties(ObjectPropertyList list)
            {
                if (Original != null)
                    Original.GetProperties(list);
                else
                    base.GetProperties(list);
            }

            public ItemClone(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0);
                writer.Write(Original);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                reader.ReadInt();
                Original = reader.ReadItem();

                if (Original == null)
                    Delete();
            }
        }

        public class MountClone : BaseMount
        {
            public MountClone(BaseMount original) : base(original.Name,  original.BodyValue, original.ItemID, original.AI, original.FightMode, original.RangePerception, original.RangeFight, original.ActiveSpeed, original.PassiveSpeed)
            {
                foreach (var property in (typeof(BaseMount)).GetProperties())
                    if (property.CanRead && property.CanWrite && property.Name != "Rider")
                        property.SetValue(this, property.GetValue(original, null), null);
            }

            public MountClone(Serial serial) : base(serial) { }

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
}