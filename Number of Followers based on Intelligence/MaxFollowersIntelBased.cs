//   ___|========================|___
//   \  |  Written by Felladrin  |  /   MaxFollowersIntelBased - Current version: 1.1 (August 24, 2013)
//    > |      August 2013       | <
//   /__|========================|__\   Description: Player's max number of followers based on intelligence.
//
//   Usage: Set the Config, on the first lines of this script, to suit your needs.
//
//   Open PlayerMobile.cs and find, in the ValidateEquipment_Sandbox method, the following line:
//
//   Mobile from = this;
//
//   Under that line, add the line bellow:
//
//   MaxFollowersIntelBased.Evaluate(from);

namespace Server.Mobiles
{
    public static class MaxFollowersIntelBased
    {
        public static class Config
        {
            public static int MaxIntAllowed = 125;      // What's the Intelligence limit for players?
            public static int MaxFollowersAllowed = 8;  // What's the maximum number of followers they can have?
            public static int MinFollowersAllowed = 2;  // What's the minimum number of followers they can have?
        }

        public static void Evaluate(Mobile m)
        {
            if (Config.MaxFollowersAllowed - Config.MinFollowersAllowed > 0)
            {
                int intelPerFollower = Config.MaxIntAllowed / Config.MaxFollowersAllowed;

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