// ResetSkill Command v1.0.1
// Author: Felladrin
// Started: 2016-01-02
// Updated: 2016-01-21

using System;
using Server;
using Server.Commands;

namespace Felladrin.Commands
{
    public static class ResetSkill
    {
        public static class Config
        {
            public static bool Enabled = true; // Is this command enabled?
        }

        public static void Initialize()
        {
            if (Config.Enabled)
                CommandSystem.Register("ResetSkill", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("ResetSkill <skill name>")]
        [Description("Used to set a given skill back to 0.")]
        public static void OnCommand(CommandEventArgs arg)
        {
            Mobile m = arg.Mobile;

            string skillNames = string.Join(", ", Enum.GetNames(typeof(SkillName)));

            if (arg.Length != 1)
            {
                m.SendMessage("Usage: SetSkill <skill name>. List of skill names: {0}.", skillNames);
            }
            else
            {
                SkillName skillName;
                if (Enum.TryParse(arg.GetString(0), true, out skillName))
                {
                    Skill skill = m.Skills[skillName];
                    if (skill != null)
                    {
                        skill.Base = 0;
                        m.SendMessage("You've successfully reset your {0}.", skill.Name);
                    }
                }
                else
                {
                    m.SendMessage("You have specified an invalid skill to set. List of skill names: {0}.", skillNames);
                }
            }
        }
    }
}
