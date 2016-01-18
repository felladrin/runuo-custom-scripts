// Max Followers Based On Intelligence v1.2.0
// Author: Felladrin
// Started: 2013-08-23
// Updated: 2016-01-18

// Installation:
// Open PlayerMobile.cs and find, in the ValidateEquipment_Sandbox() method, the following line:
// Mobile from = this;
// Below that line, add the following line:
// Felladrin.Automations.MaxFollowersBasedOnIntelligence.Evaluate(from);

using Server;

namespace Felladrin.Automations
{
    public static class MaxFollowersBasedOnIntelligence
    {
        public static class Config
        {
            public static int IntelligenceCap = 125;      // What's the intelligence cap for players?
            public static int MinFollowersAllowed = 2;    // What's the minimum number of followers they can have?
            public static int MaxFollowersAllowed = 8;    // What's the maximum number of followers they can have?
        }

        public static void Evaluate(Mobile m)
        {
            if (Config.MaxFollowersAllowed - Config.MinFollowersAllowed > 0)
            {
                int intelPerFollower = Config.IntelligenceCap / Config.MaxFollowersAllowed;

                int result = m.Int / intelPerFollower + 1;

                if (result > Config.MaxFollowersAllowed)
                {
                    m.FollowersMax = Config.MaxFollowersAllowed;
                }
                else if (result < Config.MinFollowersAllowed)
                {
                    m.FollowersMax = Config.MinFollowersAllowed;
                }
                else
                {
                    m.FollowersMax = result;
                }
            }
        }
    }
}