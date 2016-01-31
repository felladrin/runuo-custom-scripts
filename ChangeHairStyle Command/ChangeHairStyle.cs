// ChangeHairStyle Command v1.0.0
// Author: Felladrin
// Started: 2016-01-31
// Updated: 2016-01-31

using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Felladrin.Commands
{
    public static class ChangeHairStyle
    {
        public static class Config
        {
            public static bool Enabled = true;                  // Is this command enabled?
            public static int PriceForHairHue = 50;             // What's the price for changing hair hue?
            public static int PriceForHairStyle = 500;          // What's the price for changing hair style?
            public static int PriceForFacialHairStyle = 300;    // What's the price for changing facial hair style?
            public static bool DisplayRegularHues = true;       // Should we display regular hues (true) or bright hues (folse) on the hair hue gump?
        }

        public static void Initialize()
        {
            if (Config.Enabled)
                CommandSystem.Register("ChangeHairStyle", AccessLevel.Player, new CommandEventHandler(OnCommand));
        }

        [Usage("ChangeHairStyle")]
        [Description("Used to change yout hair style.")]
        static void OnCommand(CommandEventArgs e)
        {
            var from = e.Mobile;

            from.CloseGump(typeof(ChangeHairHueGump));
            from.CloseGump(typeof(ChangeHairstyleGump));

            from.SendGump(new ChangeHairHueGump(from, Config.PriceForHairHue, true, true, (Config.DisplayRegularHues ? ChangeHairHueEntry.RegularEntries : ChangeHairHueEntry.BrightEntries)));

            if (Config.PriceForHairHue > 0)
                from.SendMessage(65, "You'll be charged {0} gold, directly from your bank, if you choose to change your hair hue.", Config.PriceForHairHue);
            
            if (from.Race == Race.Human)
            {
                from.SendGump(new ChangeHairstyleGump(from, Config.PriceForHairStyle, false, ChangeHairstyleEntry.HairEntries));

                if (Config.PriceForHairStyle > 0)
                    from.SendMessage(67, "You'll be charged {0} gold, directly from your bank, if you choose to change your hair style.", Config.PriceForHairStyle);

                if (!from.Female)
                {
                    from.SendGump(new ChangeHairstyleGump(from, Config.PriceForFacialHairStyle, true, ChangeHairstyleEntry.BeardEntries));

                    if (Config.PriceForFacialHairStyle > 0)
                        from.SendMessage(66, "You'll be charged {0} gold, directly from your bank, if you choose to change your facial hair style.", Config.PriceForFacialHairStyle);
                }
            }
        }

        public class ChangeHairstyleGump : Gump
        {
            readonly Mobile m_From;
            readonly int m_Price;
            readonly bool m_FacialHair;
            readonly ChangeHairstyleEntry[] m_Entries;

            public ChangeHairstyleGump(Mobile from, int price, bool facialHair, ChangeHairstyleEntry[] entries) : base(50, 50)
            {
                m_From = from;
                m_Price = price;
                m_FacialHair = facialHair;
                m_Entries = entries;

                int tableWidth = (m_FacialHair ? 2 : 3);
                int tableHeight = ((entries.Length + tableWidth - (m_FacialHair ? 1 : 2)) / tableWidth);
                const int offsetWidth = 123;
                int offsetHeight = (m_FacialHair ? 70 : 65);

                AddPage(0);

                AddBackground(0, 0, 81 + (tableWidth * offsetWidth), 105 + (tableHeight * offsetHeight), 2600);

                AddButton(45, 45 + (tableHeight * offsetHeight), 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(77, 45 + (tableHeight * offsetHeight), 90, 35, 1006044, false, false); // Ok

                AddButton(81 + (tableWidth * offsetWidth) - 180, 45 + (tableHeight * offsetHeight), 4005, 4007, 0, GumpButtonType.Reply, 0);
                AddHtmlLocalized(81 + (tableWidth * offsetWidth) - 148, 45 + (tableHeight * offsetHeight), 90, 35, 1006045, false, false); // Cancel

                if (!facialHair)
                    AddHtmlLocalized(50, 15, 350, 20, 1018353, false, false); // <center>New Hairstyle</center>
                else
                    AddHtmlLocalized(55, 15, 200, 20, 1018354, false, false); // <center>New Beard</center>

                for (int i = 0; i < entries.Length; ++i)
                {
                    int xTable = i % tableWidth;
                    int yTable = i / tableWidth;

                    if (entries[i].GumpID != 0)
                    {
                        AddRadio(40 + (xTable * offsetWidth), 70 + (yTable * offsetHeight), 208, 209, false, i);
                        AddBackground(87 + (xTable * offsetWidth), 50 + (yTable * offsetHeight), 50, 50, 2620);
                        AddImage(87 + (xTable * offsetWidth) + entries[i].X, 50 + (yTable * offsetHeight) + entries[i].Y, entries[i].GumpID);
                    }
                    else if (!facialHair)
                    {
                        AddRadio(40 + ((xTable + 1) * offsetWidth), 240, 208, 209, false, i);
                        AddHtmlLocalized(60 + ((xTable + 1) * offsetWidth), 240, 85, 35, 1011064, false, false); // Bald
                    }
                    else
                    {
                        AddRadio(40 + (xTable * offsetWidth), 70 + (yTable * offsetHeight), 208, 209, false, i);
                        AddHtmlLocalized(60 + (xTable * offsetWidth), 70 + (yTable * offsetHeight), 85, 35, 1011064, false, false); // Bald
                    }
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (m_FacialHair && (m_From.Female || m_From.Body.IsFemale))
                    return;

                if (m_From.Race == Race.Elf)
                {
                    m_From.SendMessage("This isn't implemented for elves yet.  Sorry!");
                    return;
                }

                if (info.ButtonID == 1)
                {
                    int[] switches = info.Switches;

                    if (switches.Length > 0)
                    {
                        int index = switches[0];

                        if (index >= 0 && index < m_Entries.Length)
                        {
                            ChangeHairstyleEntry entry = m_Entries[index];

                            var playerMobile = m_From as PlayerMobile;
                            if (playerMobile != null)
                                playerMobile.SetHairMods(-1, -1);

                            int hairID = m_From.HairItemID;
                            int facialHairID = m_From.FacialHairItemID;

                            if (entry.ItemID == 0)
                            {
                                if (m_FacialHair ? (facialHairID == 0) : (hairID == 0))
                                    return;

                                if (Banker.Withdraw(m_From, m_Price))
                                {
                                    if (m_FacialHair)
                                    {
                                        m_From.FacialHairItemID = 0;

                                        if (Config.PriceForFacialHairStyle > 0)
                                            m_From.SendLocalizedMessage(1060398, Config.PriceForFacialHairStyle.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                                    }
                                    else
                                    {
                                        m_From.HairItemID = 0;

                                        if (Config.PriceForHairStyle > 0)
                                            m_From.SendLocalizedMessage(1060398, Config.PriceForHairStyle.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                                    }
                                }
                                else
                                    m_From.SendLocalizedMessage(1042293); // You cannot afford my services for that style.
                            }
                            else
                            {
                                if (m_FacialHair)
                                {
                                    if (facialHairID > 0 && facialHairID == entry.ItemID)
                                        return;
                                }
                                else
                                {
                                    if (hairID > 0 && hairID == entry.ItemID)
                                        return;
                                }

                                if (Banker.Withdraw(m_From, m_Price))
                                {
                                    if (m_FacialHair)
                                    {
                                        m_From.FacialHairItemID = entry.ItemID;

                                        if (Config.PriceForFacialHairStyle > 0)
                                            m_From.SendLocalizedMessage(1060398, Config.PriceForFacialHairStyle.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                                    }
                                    else
                                    {
                                        m_From.HairItemID = entry.ItemID;

                                        if (Config.PriceForHairStyle > 0)
                                            m_From.SendLocalizedMessage(1060398, Config.PriceForHairStyle.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                                    }
                                }
                                else
                                    m_From.SendLocalizedMessage(1042293); // You cannot afford my services for that style.
                            }
                        }
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(1013009); // You decide not to change your hairstyle.
                    }
                }
                else
                {
                    m_From.SendLocalizedMessage(1013009); // You decide not to change your hairstyle.
                }
            }
        }


        public class ChangeHairstyleEntry
        {
            readonly int m_ItemID;
            readonly int m_GumpID;
            int m_X, m_Y;

            public int ItemID{ get { return m_ItemID; } }

            public int GumpID{ get { return m_GumpID; } }

            public int X{ get { return m_X; } }

            public int Y{ get { return m_Y; } }

            public ChangeHairstyleEntry(int gumpID, int x, int y, int itemID)
            {
                m_GumpID = gumpID;
                m_X = x;
                m_Y = y;
                m_ItemID = itemID;
            }

            public static readonly ChangeHairstyleEntry[] HairEntries =
            {
                new ChangeHairstyleEntry(50700, 70 - 137, 20 - 60, 0x203B),
                new ChangeHairstyleEntry(60710, 193 - 260, 18 - 60, 0x2045),
                new ChangeHairstyleEntry(50703, 316 - 383, 25 - 60, 0x2044),
                new ChangeHairstyleEntry(60708, 70 - 137, 75 - 125, 0x203C),
                new ChangeHairstyleEntry(60900, 193 - 260, 85 - 125, 0x2047),
                new ChangeHairstyleEntry(60713, 320 - 383, 85 - 125, 0x204A),
                new ChangeHairstyleEntry(60702, 70 - 137, 140 - 190, 0x203D),
                new ChangeHairstyleEntry(60707, 193 - 260, 140 - 190, 0x2049),
                new ChangeHairstyleEntry(60901, 315 - 383, 150 - 190, 0x2048),
                new ChangeHairstyleEntry(0, 0, 0, 0)
            };

            public static readonly ChangeHairstyleEntry[] BeardEntries =
            {
                new ChangeHairstyleEntry(50800, 120 - 187, 30 - 80, 0x2040),
                new ChangeHairstyleEntry(50904, 243 - 310, 33 - 80, 0x204B),
                new ChangeHairstyleEntry(50906, 120 - 187, 100 - 150, 0x204D),
                new ChangeHairstyleEntry(50801, 243 - 310, 95 - 150, 0x203E),
                new ChangeHairstyleEntry(50802, 120 - 187, 173 - 220, 0x203F),
                new ChangeHairstyleEntry(50905, 243 - 310, 165 - 220, 0x204C),
                new ChangeHairstyleEntry(50808, 120 - 187, 242 - 290, 0x2041),
                new ChangeHairstyleEntry(0, 0, 0, 0)
            };
        }

        public class ChangeHairHueEntry
        {
            readonly string m_Name;
            readonly int[] m_Hues;

            public string Name{ get { return m_Name; } }

            public int[] Hues{ get { return m_Hues; } }

            public ChangeHairHueEntry(string name, int[] hues)
            {
                m_Name = name;
                m_Hues = hues;
            }

            public ChangeHairHueEntry(string name, int start, int count)
            {
                m_Name = name;

                m_Hues = new int[count];

                for (int i = 0; i < count; ++i)
                    m_Hues[i] = start + i;
            }

            public static readonly ChangeHairHueEntry[] BrightEntries =
            {
                new ChangeHairHueEntry("*****", 12, 10),
                new ChangeHairHueEntry("*****", 32, 5),
                new ChangeHairHueEntry("*****", 38, 8),
                new ChangeHairHueEntry("*****", 54, 3),
                new ChangeHairHueEntry("*****", 62, 10),
                new ChangeHairHueEntry("*****", 81, 2),
                new ChangeHairHueEntry("*****", 89, 2),
                new ChangeHairHueEntry("*****", 1153, 2)
            };

            public static readonly ChangeHairHueEntry[] RegularEntries =
            {
                new ChangeHairHueEntry("*****", 1602, 26),
                new ChangeHairHueEntry("*****", 1628, 27),
                new ChangeHairHueEntry("*****", 1502, 32),
                new ChangeHairHueEntry("*****", 1302, 32),
                new ChangeHairHueEntry("*****", 1402, 32),
                new ChangeHairHueEntry("*****", 1202, 24),
                new ChangeHairHueEntry("*****", 2402, 29),
                new ChangeHairHueEntry("*****", 2213, 6),
                new ChangeHairHueEntry("*****", 1102, 8),
                new ChangeHairHueEntry("*****", 1110, 8),
                new ChangeHairHueEntry("*****", 1118, 16),
                new ChangeHairHueEntry("*****", 1134, 16)
            };
        }

        public class ChangeHairHueGump : Gump
        {
            readonly Mobile m_From;
            readonly int m_Price;
            readonly bool m_Hair;
            readonly bool m_FacialHair;
            readonly ChangeHairHueEntry[] m_Entries;

            public ChangeHairHueGump(Mobile from, int price, bool hair, bool facialHair, ChangeHairHueEntry[] entries) : base(300, 50)
            {
                m_From = from;
                m_Price = price;
                m_Hair = hair;
                m_FacialHair = facialHair;
                m_Entries = entries;

                AddPage(0);

                AddBackground(100, 10, 350, 370, 2600);
                AddBackground(120, 54, 110, 270, 5100);

                AddHtmlLocalized(155, 25, 240, 30, 1011013, false, false); // <center>Hair Color Selection Menu</center>

                AddHtmlLocalized(150, 330, 220, 35, 1011014, false, false); // Dye my hair this color!
                AddButton(380, 330, 4005, 4007, 1, GumpButtonType.Reply, 0);

                for (int i = 0; i < entries.Length; ++i)
                {
                    ChangeHairHueEntry entry = entries[i];

                    AddLabel(130, 59 + (i * 22), entry.Hues[0] - 1, entry.Name);
                    AddButton(207, 60 + (i * 22), 5224, 5224, 0, GumpButtonType.Page, 1 + i);
                }

                for (int i = 0; i < entries.Length; ++i)
                {
                    ChangeHairHueEntry entry = entries[i];
                    int[] hues = entry.Hues;
                    string name = entry.Name;

                    AddPage(1 + i);

                    for (int j = 0; j < hues.Length; ++j)
                    {
                        AddLabel(278 + ((j / 16) * 80), 52 + ((j % 16) * 17), hues[j] - 1, name);
                        AddRadio(260 + ((j / 16) * 80), 52 + ((j % 16) * 17), 210, 211, false, (j * entries.Length) + i);
                    }
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (info.ButtonID == 1)
                {
                    int[] switches = info.Switches;

                    if (switches.Length > 0)
                    {
                        int index = switches[0] % m_Entries.Length;
                        int offset = switches[0] / m_Entries.Length;

                        if (index >= 0 && index < m_Entries.Length)
                        {
                            if (offset >= 0 && offset < m_Entries[index].Hues.Length)
                            {
                                if (m_Hair && m_From.HairItemID > 0 || m_FacialHair && m_From.FacialHairItemID > 0)
                                {
                                    if (!Banker.Withdraw(m_From, m_Price))
                                    {
                                        m_From.SendLocalizedMessage(1042293); // You cannot afford my services for that style.
                                        return;
                                    }

                                    int hue = m_Entries[index].Hues[offset];

                                    if (m_Hair)
                                        m_From.HairHue = hue;

                                    if (m_FacialHair)
                                        m_From.FacialHairHue = hue;

                                    if (Config.PriceForHairHue > 0)
                                        m_From.SendLocalizedMessage(1060398, Config.PriceForHairHue.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                                }
                                else
                                    m_From.SendLocalizedMessage(502623); // You have no hair to dye and you cannot use this.
                            }
                        }
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(1013009); // You decide not to change your hairstyle.
                    }
                }
                else
                {
                    m_From.SendLocalizedMessage(1013009); // You decide not to change your hairstyle.
                }
            }
        }
    }
}