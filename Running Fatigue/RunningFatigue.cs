//   ___|========================|___
//   \  |  Written by Felladrin  |  /   This script was released on RunUO Community under the GPL licensing terms.
//    > |      August 2013       | <
//   /__|========================|__\   [Running Fatigue] - Current version: 1.0 (August 12, 2013)

namespace Server.Mobiles
{
    public class RunningFatigue
    {
        public static void Initialize()
        {
            EventSink.Movement += new MovementEventHandler(EventSink_Movement);
        }

        public static void EventSink_Movement(MovementEventArgs e)
        {
            if (!(e.Mobile is PlayerMobile) || e.Mobile.AccessLevel > AccessLevel.Player || e.Mobile.Mount != null || !e.Mobile.Alive)
                return;

            if ((e.Direction & Direction.Running) != 0)
            {
                PlayerMobile pm = e.Mobile as PlayerMobile;

                int steps;

                if (pm.Skills.Focus.Value < 20)
                    steps = 8;
                else if (pm.Skills.Focus.Value < 40)
                    steps = 7;
                else if (pm.Skills.Focus.Value < 60)
                    steps = 6;
                else if (pm.Skills.Focus.Value < 80)
                    steps = 4;
                else
                    steps = 3;

                if ((pm.StepsTaken % steps) == 0)
                {
                    --pm.Stam;

                    if (pm.Stam == 1)
                        pm.PlaySound(pm.Female ? 816 : 1090);
                }
            }
        }
    }
}
