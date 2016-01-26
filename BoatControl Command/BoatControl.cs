// BoatControl Command v1.1.0
// Author: Felladrin
// Started: 2013-10-27
// Updated: 2016-01-26
// Credits: The gump was created by Haazen as part of his Tiller Bell script released on RunUO Forums in 2005.

using Server;
using Server.Commands;
using Server.Gumps;
using Server.Multis;
using Server.Network;

namespace Felladrin.Commands
{
    public static class BoatControl
    {
        public static void Initialize()
        {
            CommandSystem.Register("BoatControl", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("BoatControl")]
        [Description("Shows a gump to control a boat.")]
        static void OnCommand(CommandEventArgs e)
        {
            var from = e.Mobile;

            if (BaseBoat.FindBoatAt(from, from.Map) == null)
            {
                from.SendMessage(33, "You need to be on a boat to use this command!");
                return;
            }

            from.CloseGump(typeof(BoatControlGump));
            from.SendGump(new BoatControlGump(from));
        }

        public class BoatControlGump : Gump
        {
            readonly Mobile m_From;

            public BoatControlGump(Mobile from) : base(40, 40)
            {
                m_From = from;

                AddPage(0);
                AddBackground(0, 0, 140, 155, 83);
                AddImageTiled(8, 9, 126, 135, 1416);
                AddAlphaRegion(8, 9, 126, 135);
                AddLabel(50, 120, 0x34, "Anchor");
                AddButton(20, 124, 0x15E6, 0x15E6, 1, GumpButtonType.Reply, 0);  //Drop
                AddButton(107, 124, 0x15E0, 0x15E0, 2, GumpButtonType.Reply, 0); //Raise
                AddButton(60, 10, 0x26AC, 0x26AC, 3, GumpButtonType.Reply, 0);   //Forward
                AddButton(60, 90, 0x26B2, 0x26B2, 4, GumpButtonType.Reply, 0);   //Back
                AddButton(20, 50, 0x26B5, 0x26B5, 5, GumpButtonType.Reply, 0);   //Left
                AddButton(100, 50, 0x26AF, 0x26AF, 6, GumpButtonType.Reply, 0);  //Right
                AddButton(62, 53, 0x2C93, 0x2C93, 7, GumpButtonType.Reply, 0);   //Stop
                AddButton(20, 90, 0x5786, 0x5786, 8, GumpButtonType.Reply, 0);   //TurnLeft
                AddButton(100, 90, 0x5781, 0x5781, 9, GumpButtonType.Reply, 0);  //TurnRight
                AddButton(62, 31, 0x2621, 0x2621, 10, GumpButtonType.Reply, 0);  //OneForward
                AddButton(62, 73, 0x2625, 0x2625, 11, GumpButtonType.Reply, 0);  //OneBack
                AddButton(40, 52, 0x2627, 0x2627, 12, GumpButtonType.Reply, 0);  //OneLeft
                AddButton(83, 52, 0x2623, 0x2623, 13, GumpButtonType.Reply, 0);  //OneRight
                AddButton(39, 29, 0x13F4, 0x24C0, 14, GumpButtonType.Reply, 0);  //LeftForward
                AddButton(92, 29, 0x13F2, 0x24BE, 15, GumpButtonType.Reply, 0);  //RightForward
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                var boat = BaseBoat.FindBoatAt(m_From, m_From.Map);

                if (boat == null)
                {
                    m_From.SendMessage(33, "You need to be on a boat to use this command!");
                    return;
                }

                if (info.ButtonID >= 1 && info.ButtonID <= 15)
                {
                    m_From.SendGump(new BoatControlGump(m_From));
                }

                switch (info.ButtonID)
                {
                    case 1: // Drop Anchor
                        boat.LowerAnchor(true);
                        break;
                    case 2: // Raise Anchor
                        boat.RaiseAnchor(true);
                        break;
                    case 3: // Forward
                        boat.StartMove(Direction.North, true);
                        break;
                    case 4: // Back
                        boat.StartMove(Direction.South, true);
                        break;
                    case 5: // Left
                        boat.StartMove(Direction.West, true);
                        break;
                    case 6: // Right
                        boat.StartMove(Direction.East, true);
                        break;
                    case 7: // Stop
                        boat.StopMove(true);
                        break;
                    case 8: // TurnLeft
                        boat.StartTurn(-2, true);
                        break;
                    case 9: // TurnRight
                        boat.StartTurn(2, true);
                        break;
                    case 10: // OneForward
                        boat.OneMove(Direction.North);
                        break;
                    case 11: // OneBack
                        boat.OneMove(Direction.South);
                        break;
                    case 12: // OneLeft
                        boat.OneMove(Direction.West);
                        break;
                    case 13: // OneRight
                        boat.OneMove(Direction.East);
                        break;
                    case 14: // LeftForward
                        boat.StartMove(Direction.Up, true);
                        break;
                    case 15: // RightForward
                        boat.StartMove(Direction.Right, true);
                        break;
                }
            }
        }
    }
}