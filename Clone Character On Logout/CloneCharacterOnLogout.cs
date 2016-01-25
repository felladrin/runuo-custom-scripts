// Clone Character On Logout v1.0.0
// Author: Felladrin
// Started: 2016-01-25
// Updated: 2016-01-25

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
            }
        }

        static void OnLogout(LogoutEventArgs e)
        {
            if (e.Mobile.AccessLevel != AccessLevel.Player)
                return;

            var characterOriginal = e.Mobile;
            var characterClone = new CharacterClone(characterOriginal);

            foreach (var itemOriginal in characterOriginal.Items)
                if (itemOriginal.Parent == characterOriginal)
                    characterClone.AddItem(new ItemClone(itemOriginal));
        }

        static void OnLogin(LoginEventArgs e)
        {
            if (e.Mobile.AccessLevel != AccessLevel.Player)
                return;

            foreach (var m in new List<Mobile>(World.Mobiles.Values))
                if (m is CharacterClone && ((CharacterClone)m).Original == e.Mobile)
                    m.Delete();
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

                Player = false;
            }

            public override void GetProperties(ObjectPropertyList list)
            {
                if (Original != null)
                {
                    Original.GetProperties(list);
                    return;
                }

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
                {
                    Original.GetProperties(list);
                    return;
                }

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
    }
}