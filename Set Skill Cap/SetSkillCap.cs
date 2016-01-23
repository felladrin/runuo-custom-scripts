// Set Skill Cap v1.0.2
// Author: Felladrin
// Started: 2015-12-20
// Updated: 2016-01-22

using Server;

namespace Felladrin.Automations
{
    public static class SetSkillCap
    {
        public static class Config
        {
            public static bool Enabled = true;             // Is this system enabled?
            public static int IndividualSkillCap = 100;    // Cap for each skill. Default: 100.
            public static int TotalSkillCap = 700;         // Cap for the sum of all skills. Default: 700.
        }

        public static void Initialize()
        {
            if (Config.Enabled)
            {
                Config.IndividualSkillCap *= 10;
                Config.TotalSkillCap *= 10;
                EventSink.Login += OnLogin;
            }
        }
        
        static void OnLogin(LoginEventArgs args)
        {
            Mobile m = args.Mobile;
            
            if (m.AccessLevel > AccessLevel.Player)
            	return;
            
            Skills skills = m.Skills;

            foreach (Skill skill in m.Skills)
                skill.CapFixedPoint = Config.IndividualSkillCap;
                
            for (int i = 0; i < skills.Length; ++i)
                if (skills[i].BaseFixedPoint > Config.IndividualSkillCap)
                    skills[i].BaseFixedPoint = Config.IndividualSkillCap;
            
            m.SkillsCap = Config.TotalSkillCap;
            
            for (int j = 0; (m.SkillsTotal > m.SkillsCap) && (j < skills.Length); ++j)
            {
            	double diff = ((m.SkillsTotal - m.SkillsCap) / 10) + 1;
                
                if (skills[SkillName.Focus].Base > 0)
                {
                    skills[SkillName.Focus].Base -= diff;
                    
                    if (skills[SkillName.Focus].Base < 0)
                        skills[SkillName.Focus].Base = 0;
                    
                    continue;
                }
                
                if (skills[SkillName.Meditation].Base > 0)
                {
                    skills[SkillName.Meditation].Base -= diff;
                    
                    if (skills[SkillName.Meditation].Base < 0)
                        skills[SkillName.Meditation].Base = 0;
                    
                    continue;
                }
                
                int lowestSkillId = -1;
                double lowestSkillBase = Config.IndividualSkillCap;
                
                for ( int i = 0; i < skills.Length; ++i )
                {
                    if (skills[i].Base > 0 && skills[i].Base < lowestSkillBase)
                    {
                        lowestSkillId = i;
                        lowestSkillBase = skills[i].Base;
                    }
                }
                
                if (lowestSkillId == -1)
                    break;
                
                skills[lowestSkillId].Base -= diff;
                
                if (skills[lowestSkillId].Base < 0)
                    skills[lowestSkillId].Base = 0;
            }
        }
    }
}
