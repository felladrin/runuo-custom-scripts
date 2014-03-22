//   ___|========================|___
//   \  |  Written by Felladrin  |  /   This script was released on RunUO Forums under the GPL licensing terms.
//    > |      August 2013       | <
//   /__|========================|__\   [Global Bulletin Board] - Current version: 0.9 (August 22, 2013)

using Server.Items;
using Server.Network;
using Server.Commands;

namespace Server.Custom.Engines
{
    public class GlobalBulletinBoardSystem
    {
        public static GlobalBulletinBoard GBB;

        public static void Initialize()
        {
            CommandSystem.Register("BB", AccessLevel.Player, new CommandEventHandler(BB_OnCommand));
        }

        [Usage("BB")]
        [Description("Opens the Global Bulletin Board.")]
        public static void BB_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            VerifyExistence();

            bool hasChangedMap = false;

            if (GBB.Map != from.Map)
                hasChangedMap = true;

            GBB.MoveToWorld(new Point3D(from.X, from.Y, from.Z - 100), from.Map);

            if (hasChangedMap)
            {
                Timer.DelayCall(System.TimeSpan.FromSeconds(1), delegate { GBB.OnDoubleClick(from); });
            }
            else
            {
                GBB.OnDoubleClick(from);
            }
        }

        public class GlobalBulletinBoard : BaseBulletinBoard
        {
            public GlobalBulletinBoard() : base(0x1E5E)
            {
                BoardName = Misc.ServerList.ServerName + " Bulletin Board";
                Location = new Point3D(1504, 1626, 20);
                Map = Map.Felucca;
            }

            public override void OnDoubleClick(Mobile from)
            {
                NetState state = from.NetState;

                state.Send(new BBDisplayBoard(this));
                if (state.ContainerGridLines)
                    state.Send(new ContainerContent6017(from, this));
                else
                    state.Send(new ContainerContent(from, this));
            }

            public GlobalBulletinBoard(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }

        public static void VerifyExistence()
        {
            if (GBB != null && !GBB.Deleted)
                return;

            foreach (Item item in World.Items.Values)
            {
                if (item is GlobalBulletinBoard)
                {
                    GBB = item as GlobalBulletinBoard;
                    return;
                }
            }

            GBB = new GlobalBulletinBoard();
        }
    }
}