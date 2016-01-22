// Rest Command v1.1.0
// Author: Felladrin
// Started: 2011-01-06
// Updated: 2016-01-21

using Server;
using Server.Commands;
using Server.Targeting;
using Server.Spells.Necromancy;
using Server.Spells.Fourth;
using Server.Items;

namespace Felladrin.Commands
{
    public static class Rest
    {
        public static void Initialize()
        {
            CommandSystem.Register("Rest", AccessLevel.Counselor, new CommandEventHandler(OnCommand));
        }

        [Usage("Rest")]
        [Description("Completely restores a mobile. It fills hits, mana, stam, hunger, thirst and cure poison, paralysis, wounds and curses. Also resurrects the target if needed.")]
        static void OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new RestTarget();
        }

        class RestTarget : Target
        {
            public RestTarget() : base(-1, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                var m = targeted as Mobile;

                if (m == null)
                {
                    from.SendMessage("This is not a mobile!");
                    return;
                }

                CommandLogging.WriteLine(from, "{0} {1} completely restoring {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(m));

                if (!m.Alive)
                    m.Resurrect();

                m.Hits = m.HitsMax;
                m.Mana = m.ManaMax;
                m.Stam = m.StamMax;
                m.Hunger = 20;
                m.Thirst = 20;
                m.Poison = null;
                m.Paralyzed = false;

                StatMod mod;

                mod = m.GetStatMod("[Magic] Str Offset");
                if (mod != null && mod.Offset < 0)
                    m.RemoveStatMod("[Magic] Str Offset");

                mod = m.GetStatMod("[Magic] Dex Offset");
                if (mod != null && mod.Offset < 0)
                    m.RemoveStatMod("[Magic] Dex Offset");

                mod = m.GetStatMod("[Magic] Int Offset");
                if (mod != null && mod.Offset < 0)
                    m.RemoveStatMod("[Magic] Int Offset");

                EvilOmenSpell.TryEndEffect(m);
                StrangleSpell.RemoveCurse(m);
                CorpseSkinSpell.RemoveCurse(m);
                CurseSpell.RemoveEffect(m);
                MortalStrike.EndWound(m);
                BloodOathSpell.RemoveCurse(m);
                MindRotSpell.ClearMindRotScalar(m);

                BuffInfo.RemoveBuff(m, BuffIcon.Clumsy);
                BuffInfo.RemoveBuff(m, BuffIcon.FeebleMind);
                BuffInfo.RemoveBuff(m, BuffIcon.Weaken);
                BuffInfo.RemoveBuff(m, BuffIcon.Curse);
                BuffInfo.RemoveBuff(m, BuffIcon.MassCurse);
                BuffInfo.RemoveBuff(m, BuffIcon.MortalStrike);
                BuffInfo.RemoveBuff(m, BuffIcon.Mindrot);

                from.SendMessage("{0} has been completely restored!", m.Name);
            }
        }
    }
}
