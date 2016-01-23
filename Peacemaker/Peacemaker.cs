// Peacemaker v1.6.0
// Author: Felladrin
// Started: 2007-07-08
// Updated: 2016-01-23

using Server;
using Server.Items;
using Server.Mobiles;

namespace Felladrin.Mobiles
{
    [CorpseName("the corpse of a peacemaker")]
    public class BasePeacemaker : BaseCreature
    {
        public BasePeacemaker(AIType aiType, int rangeFight) : base(aiType, FightMode.Closest, 10, rangeFight, 0.2, 0.4)
        {
            Title = "the peacemaker";

            Fame = 1000;
            Karma = 1000;

            SpeechHue = Utility.RandomDyedHue();

            Female = Utility.RandomBool();

            Hue = Utility.RandomSkinHue();

            Utility.AssignRandomHair(this);

            if (Female)
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");

                if (Utility.RandomBool())
                    Utility.AssignRandomFacialHair(this, HairHue);
            }

            SetResistance(ResistanceType.Physical, 55, 70);
            SetResistance(ResistanceType.Fire, 55, 70);
            SetResistance(ResistanceType.Cold, 55, 70);
            SetResistance(ResistanceType.Poison, 55, 70);
            SetResistance(ResistanceType.Energy, 55, 70);

            AddItem(new StuddedChest());
            AddItem(new StuddedArms());
            AddItem(new StuddedGloves());
            AddItem(new StuddedGorget());
            AddItem(new StuddedLegs());
            AddItem(new Sandals());

            PackGold(100, 200);
        }

        public override void OnBeforeSpawn( Point3D location, Map m ) { IsParagon = false; }

        public override bool AlwaysAttackable{ get { return true; } }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);
            aggressor.Criminal = true;
        }

        public override bool OnBeforeDeath()
        {
            if (Combatant is PlayerMobile)
                Combatant.Kills += 1;
        	
            return base.OnBeforeDeath();
        }

        public override double WeaponAbilityChance { get { return 0.7; } }

        public override bool IsEnemy(Mobile m)
        {
            if (m is BasePeacemaker || m is BaseVendor)
                return false;
        	
            if (m.Criminal)
                return true;
        	
            var baseCreature = m as BaseCreature;

            if (baseCreature != null)
            {
                if (baseCreature.Karma < 0 && baseCreature.FightMode != FightMode.Aggressor && !baseCreature.Controlled)
                    return true;
        		
                if (baseCreature.AlwaysMurderer)
                    return true;
            }
        	
            var playerMobile = m as PlayerMobile;

            if (playerMobile != null)
            {
                if (playerMobile.ShortTermMurders > 0)
                    return true;
            }

            return false;
        }

        public override bool HandlesOnSpeech(Mobile from) { return true; }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (e.Handled || !e.Mobile.InRange(Location, 18))
                return;

            if (e.Speech.ToLower().Contains("guard") || e.Speech.ToLower().Contains("help") || e.Speech.ToLower().Contains("peacemaker"))
            {
                Direction = GetDirectionTo(e.Mobile);

                if (e.Mobile.Combatant != null && IsEnemy(e.Mobile.Combatant))
                {
                    Say(speech[Utility.Random(speech.Length)]);
                    Warmode = true;
                    Combatant = e.Mobile.Combatant;
                }
                else if (IsEnemy(e.Mobile))
                {
                    Say(speech[Utility.Random(speech.Length)]);
                    Warmode = true;
                    Combatant = e.Mobile;
                }
                else
                {
                    Emote("looks around");
                    Warmode = false;
                    Combatant = null;
                }
            }
        }

        static string[] speech =
        {
            "To the fight!",
            "To arms!",
            "Attack!",
            "The battle is on!",
            "To your weapons!",
            "I have my eye on my enemy!",
            "Time to die!",
            "Nothing walks away!",
            "I have sight of my enemy!",
            "You will not prevail!",
            "To my side!",
            "We must defend our land!",
            "Fight for our people!",
            "I see them!",
            "Destroy them all!"
        };

        public BasePeacemaker(Serial serial) : base(serial) { }

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

    public class FighterPeacemaker : BasePeacemaker
    {
        [Constructable]
        public FighterPeacemaker() : base(AIType.AI_Melee, 1)
        {
            SetStr(220, 300);
            SetDex(40, 60);
            SetInt(40, 60);

            Item weapon;
            switch (Utility.Random(6))
            {
                case 0:
                    weapon = new Broadsword();
                    break;
                case 1:
                    weapon = new Cutlass();
                    break;
                case 2:
                    weapon = new Katana();
                    break;
                case 3:
                    weapon = new Longsword();
                    break;
                case 4:
                    weapon = new Scimitar();
                    break;
                default:
                    weapon = new VikingSword();
                    break;
            }
            AddItem(weapon);

            AddItem(new MetalShield());

            SetDamageType(ResistanceType.Physical, 100);

            SetSkill(SkillName.Tactics, 70, 95);
            SetSkill(SkillName.Swords, 70, 100);
            SetSkill(SkillName.Fencing, 65, 100);
            SetSkill(SkillName.MagicResist, 80, 110);
            SetSkill(SkillName.Macing, 75, 100);
            SetSkill(SkillName.Wrestling, 65, 100);
            SetSkill(SkillName.Parry, 70, 100);
            SetSkill(SkillName.Healing, 65, 75);
            SetSkill(SkillName.Anatomy, 80, 90);
        }

        public FighterPeacemaker(Serial serial) : base(serial) { }

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

    public class ArcherPeacemaker : BasePeacemaker
    {
        [Constructable]
        public ArcherPeacemaker() : base(AIType.AI_Archer, 8)
        {
            SetStr(70, 90);
            SetDex(100, 150);
            SetInt(20, 35);

            Item weapon;
            switch (Utility.Random(4))
            {
                case 0:
                    weapon = new BarbedLongbow();
                    break;
                case 1:
                    weapon = new CompositeBow();
                    break;
                case 2:
                    weapon = new JukaBow();
                    break;
                default:
                    weapon = new Bow();
                    break;
            }
            AddItem(weapon);

            SetSkill(SkillName.Tactics, 70, 95);
            SetSkill(SkillName.Archery, 70, 100);
            SetSkill(SkillName.Fencing, 65, 100);
            SetSkill(SkillName.MagicResist, 80, 110);
            SetSkill(SkillName.Macing, 75, 100);
            SetSkill(SkillName.Wrestling, 65, 100);
            SetSkill(SkillName.Healing, 65, 75);
            SetSkill(SkillName.Anatomy, 80, 90);
        }


        public ArcherPeacemaker(Serial serial) : base(serial) { }

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

    public class MagePeacemaker : BasePeacemaker
    {
        [Constructable]
        public MagePeacemaker() : base(AIType.AI_Mage, 5)
        {
            SetStr(40, 60);
            SetDex(40, 60);
            SetInt(220, 300);

            BaseWeapon weapon;
            switch (Utility.Random(2))
            {
                case 0:
                    weapon = new Scepter();
                    break;
                default:
                    weapon = new MagicWand();
                    break;
            }
            weapon.Attributes.SpellChanneling = 1;
            AddItem(weapon);

            SetDamageType(ResistanceType.Physical, 0);

            if (Utility.RandomBool())
                SetDamageType(ResistanceType.Cold, 60);
            else
                SetDamageType(ResistanceType.Fire, 60);

            if (Utility.RandomBool())
                SetDamageType(ResistanceType.Energy, 40);
            else
                SetDamageType(ResistanceType.Poison, 40);

            SetSkill(SkillName.EvalInt, 90, 100);
            SetSkill(SkillName.Magery, 90, 100);
            SetSkill(SkillName.Necromancy, 0, 110);
            SetSkill(SkillName.SpiritSpeak, 90, 110);
            SetSkill(SkillName.MagicResist, 90, 120);
            SetSkill(SkillName.Tactics, 50, 70);
            SetSkill(SkillName.Macing, 60, 80);
        }

        public MagePeacemaker(Serial serial) : base(serial) { }

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