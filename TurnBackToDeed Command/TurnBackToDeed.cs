// TurnBackToDeed Command v1.1.0
// Author: Felladrin
// Started: 2008-01-13
// Updated: 2016-01-21

using Server;
using Server.Commands;
using Server.Targeting;

namespace Felladrin.Commands
{
    public static class TurnBackToDeed
    {
        public static void Initialize()
        {
            CommandSystem.Register("TurnBackToDeed", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("TurnBackToDeed")]
        [Description("Turns back to deed a furniture from your house.")]
        static void OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new DismantleTarget();
            e.Mobile.SendMessage("Select the furnitures you want to turn back to deed. Press ESC when you finish.");
        }

        class DismantleTarget : Target
        {
            public DismantleTarget() : base(2, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                var chopable = targeted as IChopable;

                if (chopable != null)
                {
                    chopable.OnChop(from);
                }
                else
                {
                    from.SendMessage("That object can't be turned back to deed.");
                }

                from.Target = new DismantleTarget();
            }
        }
    }
}