// Summon Command v1.1.0
// Author: Felladrin
// Created at 2013-10-14
// Updated at 2016-01-17

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;
using Server.Spells;

namespace Felladrin.Commands
{
    public static class Summon
    {
        public static class Config
        {
            public static bool Enabled = true;                                    // Is this command enabled?
            public static bool CanBeUsedInCombat = false;                         // Can they use this command while in combat?
            public static bool CanBeUsedWhileDead = false;                        // Can they use this command while they are dead?
            public static TimeSpan UseDelay = TimeSpan.FromMinutes(1);            // How long players need to wait before using the command again?
            public static TimeSpan AutoRefuseDelay = TimeSpan.FromSeconds(30);    // How long should the gump be displayed for the summoned?
        }

        public static void Initialize()
        {
            if (Config.Enabled)
                CommandSystem.Register("Summon", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        static Dictionary<int, DateTime> LastUsed = new Dictionary<int, DateTime>();

        [Usage("Summon [filter]")]
        [Description("Summons a player to join you.")]
        static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            int key = from.Serial.Value;

            if (!Config.CanBeUsedInCombat && SpellHelper.CheckCombat(from))
            {
                from.SendMessage(38, "You can't do this while in middle a combat.");
                return;
            }

            if (!Config.CanBeUsedWhileDead && !from.Alive)
            {
                from.SendMessage(38, "You can't do this while you're dead.");
                return;
            }

            if (LastUsed.ContainsKey(key) && (DateTime.Now - Config.UseDelay) < LastUsed[key])
            {
                TimeSpan timeToWait = Config.UseDelay - (DateTime.Now - LastUsed[key]);
                from.SendMessage(38, "You still need to wait {0:N0} seconds before summoning another player.", timeToWait.TotalSeconds);
                return;
            }

            from.CloseGump(typeof(SummonGumpList));
            from.SendGump(new SummonGumpList(from, e.ArgString));
        }

        class SummonGump : Gump
        {
            Mobile m_Summoner;

            public SummonGump(Mobile from)
                : base(0, 0)
            {
                m_Summoner = from;
                Closable = false;
                Disposable = false;
                Dragable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(11, 360, 225, 200, 9270);
                AddButton(136, 524, 12018, 12020, 0, GumpButtonType.Reply, 0);
                AddButton(35, 524, 12000, 12002, 1, GumpButtonType.Reply, 0);
                AddLabel(44, 381, 149, @"You have been summoned!");
                AddHtml(36, 410, 175, 100, String.Format("You have been summoned by {0}. Do you want to join {1}?", from.Name, (from.Female ? "her" : "him")), true, false);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;

                switch (info.ButtonID)
                {
                    case 0:
                        m_Summoner.SendMessage("{0} refuses to join you at the moment.", from.Name);
                        break;
                    case 1:
                        from.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                        from.BoltEffect(0);
                        from.MoveToWorld(m_Summoner.Location, m_Summoner.Map);
                        from.BoltEffect(0);
                        from.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                        break;
                }
            }
        }

        class SummonGumpList : Gump
        {
            public static bool OldStyle = PropsConfig.OldStyle;

            public static readonly int GumpOffsetX = PropsConfig.GumpOffsetX;
            public static readonly int GumpOffsetY = PropsConfig.GumpOffsetY;

            public static readonly int TextHue = PropsConfig.TextHue;
            public static readonly int TextOffsetX = PropsConfig.TextOffsetX;

            public static readonly int OffsetGumpID = PropsConfig.OffsetGumpID;
            public static readonly int HeaderGumpID = PropsConfig.HeaderGumpID;
            public static readonly int EntryGumpID = PropsConfig.EntryGumpID;
            public static readonly int BackGumpID = PropsConfig.BackGumpID;
            public static readonly int SetGumpID = PropsConfig.SetGumpID;

            public static readonly int SetWidth = PropsConfig.SetWidth;
            public static readonly int SetOffsetX = PropsConfig.SetOffsetX, SetOffsetY = PropsConfig.SetOffsetY;
            public static readonly int SetButtonID1 = PropsConfig.SetButtonID1;
            public static readonly int SetButtonID2 = PropsConfig.SetButtonID2;

            public static readonly int PrevWidth = PropsConfig.PrevWidth;
            public static readonly int PrevOffsetX = PropsConfig.PrevOffsetX, PrevOffsetY = PropsConfig.PrevOffsetY;
            public static readonly int PrevButtonID1 = PropsConfig.PrevButtonID1;
            public static readonly int PrevButtonID2 = PropsConfig.PrevButtonID2;

            public static readonly int NextWidth = PropsConfig.NextWidth;
            public static readonly int NextOffsetX = PropsConfig.NextOffsetX, NextOffsetY = PropsConfig.NextOffsetY;
            public static readonly int NextButtonID1 = PropsConfig.NextButtonID1;
            public static readonly int NextButtonID2 = PropsConfig.NextButtonID2;

            public static readonly int OffsetSize = PropsConfig.OffsetSize;

            public static readonly int EntryHeight = PropsConfig.EntryHeight;
            public static readonly int BorderSize = PropsConfig.BorderSize;

            static bool PrevLabel = false, NextLabel = false;

            static readonly int PrevLabelOffsetX = PrevWidth + 1;
            static readonly int PrevLabelOffsetY = 0;

            static readonly int NextLabelOffsetX = -29;
            static readonly int NextLabelOffsetY = 0;

            static readonly int EntryWidth = 180;
            static readonly int EntryCount = 15;

            static readonly int TotalWidth = OffsetSize + EntryWidth + OffsetSize + SetWidth + OffsetSize;
            static readonly int TotalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (EntryCount + 1));

            static readonly int BackWidth = BorderSize + TotalWidth + BorderSize;
            static readonly int BackHeight = BorderSize + TotalHeight + BorderSize;

            Mobile m_Owner;
            List<Mobile> m_Mobiles;
            int m_Page;

            class InternalComparer : IComparer<Mobile>
            {
                public static readonly IComparer<Mobile> Instance = new InternalComparer();

                public InternalComparer()
                {
                }

                public int Compare(Mobile x, Mobile y)
                {
                    if (x == null || y == null)
                        throw new ArgumentException();

                    if (x.AccessLevel > y.AccessLevel)
                        return -1;
                    else if (x.AccessLevel < y.AccessLevel)
                        return 1;
                    else
                        return Insensitive.Compare(x.Name, y.Name);
                }
            }

            public SummonGumpList(Mobile owner, string filter)
                : this(owner, BuildList(owner, filter), 0)
            {
            }

            public SummonGumpList(Mobile owner, List<Mobile> list, int page)
                : base(GumpOffsetX, GumpOffsetY)
            {
                owner.CloseGump(typeof(SummonGumpList));

                m_Owner = owner;
                m_Mobiles = list;

                if (m_Mobiles.Count == 0)
                {
                    owner.SendMessage(38, "There are no players available to invite.");
                }
                else if (m_Mobiles.Count == 1)
                {
                    Mobile m = m_Mobiles[0];
                    m.SendGump(new SummonGump(owner));
                    object[] arg = { m };
                    Timer.DelayCall(Config.AutoRefuseDelay, new TimerStateCallback(CloseSummonGump), arg);
                    owner.SendMessage(68, "Invitation sent to {0}.", m.Name);
                    LastUsed[owner.Serial.Value] = DateTime.Now;
                }
                else
                {
                    Init(page);
                }
            }

            public static List<Mobile> BuildList(Mobile owner, string filter)
            {
                if (filter != null && (filter = filter.Trim()).Length == 0)
                    filter = null;
                else
                    filter = filter.ToLower();

                List<Mobile> list = new List<Mobile>();
                List<NetState> states = NetState.Instances;

                for (int i = 0; i < states.Count; ++i)
                {
                    Mobile m = states[i].Mobile;

                    if (m != null)
                    {
                        if (filter != null && (m.Name == null || m.Name.ToLower().IndexOf(filter) < 0))
                            continue;

                        if (m == owner || m.AccessLevel > AccessLevel.Player || !m.Alive)
                            continue;

                        list.Add(m);
                    }
                }

                list.Sort(InternalComparer.Instance);

                return list;
            }

            void Init(int page)
            {
                m_Page = page;

                int count = m_Mobiles.Count - (page * EntryCount);

                if (count < 0)
                    count = 0;
                else if (count > EntryCount)
                    count = EntryCount;

                int totalHeight = OffsetSize + ((EntryHeight + OffsetSize) * (count + 1));

                AddPage(0);

                AddBackground(0, 0, BackWidth, BorderSize + totalHeight + BorderSize, BackGumpID);
                AddImageTiled(BorderSize, BorderSize, TotalWidth - (OldStyle ? SetWidth + OffsetSize : 0), totalHeight, OffsetGumpID);

                int x = BorderSize + OffsetSize;
                int y = BorderSize + OffsetSize;

                int emptyWidth = TotalWidth - PrevWidth - NextWidth - (OffsetSize * 4) - (OldStyle ? SetWidth + OffsetSize : 0);

                if (!OldStyle)
                    AddImageTiled(x - (OldStyle ? OffsetSize : 0), y, emptyWidth + (OldStyle ? OffsetSize * 2 : 0), EntryHeight, EntryGumpID);

                AddLabel(x + TextOffsetX, y, TextHue, String.Format("Summon who? (Page {0}/{1})", page + 1, (m_Mobiles.Count + EntryCount - 1) / EntryCount));

                x += emptyWidth + OffsetSize;

                if (OldStyle)
                    AddImageTiled(x, y, TotalWidth - (OffsetSize * 3) - SetWidth, EntryHeight, HeaderGumpID);
                else
                    AddImageTiled(x, y, PrevWidth, EntryHeight, HeaderGumpID);

                if (page > 0)
                {
                    AddButton(x + PrevOffsetX, y + PrevOffsetY, PrevButtonID1, PrevButtonID2, 1, GumpButtonType.Reply, 0);

                    if (PrevLabel)
                        AddLabel(x + PrevLabelOffsetX, y + PrevLabelOffsetY, TextHue, "Previous");
                }

                x += PrevWidth + OffsetSize;

                if (!OldStyle)
                    AddImageTiled(x, y, NextWidth, EntryHeight, HeaderGumpID);

                if ((page + 1) * EntryCount < m_Mobiles.Count)
                {
                    AddButton(x + NextOffsetX, y + NextOffsetY, NextButtonID1, NextButtonID2, 2, GumpButtonType.Reply, 1);

                    if (NextLabel)
                        AddLabel(x + NextLabelOffsetX, y + NextLabelOffsetY, TextHue, "Next");
                }

                for (int i = 0, index = page * EntryCount; i < EntryCount && index < m_Mobiles.Count; ++i, ++index)
                {
                    x = BorderSize + OffsetSize;
                    y += EntryHeight + OffsetSize;

                    Mobile m = m_Mobiles[index];

                    AddImageTiled(x, y, EntryWidth, EntryHeight, EntryGumpID);
                    AddLabelCropped(x + TextOffsetX, y, EntryWidth - TextOffsetX, EntryHeight, GetHueFor(m), m.Deleted ? "(deleted)" : m.Name);

                    x += EntryWidth + OffsetSize;

                    if (SetGumpID != 0)
                        AddImageTiled(x, y, SetWidth, EntryHeight, SetGumpID);

                    if (m.NetState != null && !m.Deleted)
                        AddButton(x + SetOffsetX, y + SetOffsetY, SetButtonID1, SetButtonID2, i + 3, GumpButtonType.Reply, 0);
                }
            }

            static int GetHueFor(Mobile m)
            {
                switch (m.AccessLevel)
                {
                    case AccessLevel.Owner:
                    case AccessLevel.Developer:
                    case AccessLevel.Administrator:
                        return 0x516;
                    case AccessLevel.Seer:
                        return 0x144;
                    case AccessLevel.GameMaster:
                        return 0x21;
                    case AccessLevel.Counselor:
                        return 0x2;
                    default:
                        {
                            if (m.Kills >= 5)
                                return 0x21;
                            else if (m.Criminal)
                                return 0x3B1;

                            return 0x58;
                        }
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;

                switch (info.ButtonID)
                {
                    case 0: // Closed
                        {
                            return;
                        }
                    case 1: // Previous
                        {
                            if (m_Page > 0)
                                from.SendGump(new SummonGumpList(from, m_Mobiles, m_Page - 1));

                            break;
                        }
                    case 2: // Next
                        {
                            if ((m_Page + 1) * EntryCount < m_Mobiles.Count)
                                from.SendGump(new SummonGumpList(from, m_Mobiles, m_Page + 1));

                            break;
                        }
                    default:
                        {
                            int index = (m_Page * EntryCount) + (info.ButtonID - 3);

                            if (index >= 0 && index < m_Mobiles.Count)
                            {
                                Mobile m = m_Mobiles[index];

                                if (m.Deleted)
                                {
                                    from.SendMessage("That player has deleted their character.");
                                    from.SendGump(new SummonGumpList(from, m_Mobiles, m_Page));
                                }
                                else if (m.NetState == null)
                                {
                                    from.SendMessage("That player is no longer online.");
                                    from.SendGump(new SummonGumpList(from, m_Mobiles, m_Page));
                                }
                                else
                                {
                                    m.SendGump(new SummonGump(from));
                                    object[] arg = { m };
                                    Timer.DelayCall(Config.AutoRefuseDelay, new TimerStateCallback(CloseSummonGump), arg);
                                    from.SendMessage(68, "Invitation sent to {0}.", m.Name);
                                    LastUsed[from.Serial.Value] = DateTime.Now;
                                }
                            }

                            break;
                        }
                }
            }

            static void CloseSummonGump(object state)
            {
                object[] states = (object[])state;
                Mobile summoned = (Mobile)states[0];
                summoned.CloseGump(typeof(SummonGump));
            }
        }
    }
}