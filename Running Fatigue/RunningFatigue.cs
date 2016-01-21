// Running Fatigue v1.1.0
// Author: Felladrin
// Started: 2013-08-12
// Updated: 2016-01-21

using System;
using Server;
using Server.Mobiles;

namespace Felladrin.Automations
{
    public static class RunningFatigue
    {
        public static void Apply(Mobile from)
        {
            var pm = from as PlayerMobile;

            if (pm != null)
            {
                pm.StepsTaken++;

                bool isRunning = (pm.Direction & Direction.Running) != 0;

                int stepsPerStam = Math.Max(10 - (int)(pm.Skills.Focus.Value / 7), 3);

                bool movedEnough = (pm.StepsTaken % stepsPerStam) == 0;

                if (!pm.Mounted && isRunning && movedEnough)
                {
                    from.Stam--;
                }
            }
        }
    }
}