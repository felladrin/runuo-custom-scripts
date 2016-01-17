// Auto Sheathe Weapon v1.4.0
// Author: Felladrin
// Started: 2013-08-14
// Updated: 2016-01-17

// Installation:
// In PlayerMobile.cs find the method OnWarmodeChanged() and add into it the line:
// Felladrin.Automations.AutoSheatheWeapon.From(this);

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;
using Server.Network;

namespace Felladrin.Automations
{
    static class AutoSheatheWeapon
    {
        public static class Config
        {
            public static bool Enabled = true;                // Is this system enabled?
            public static bool SendOverheadMessage = true;    // Should we send a overhead message to the player about the auto-sheathe?
            public static bool AllowPlayerToggle = true;      // Should we allow player to use a command to toggle the auto-sheathe?
        }

        static Type[] ItemTypesToKeepEquiped = {
            typeof(BaseShield),
            typeof(BaseLight),
            typeof(Spellbook)
        };

        static Dictionary<int, Item> PlayerWeapons = new Dictionary<int, Item>();

        static List<int> DisabledPlayers = new List<int>();

        public static void Initialize()
        {
            if (Config.Enabled)
            {
                EventSink.Logout += OnPlayerLogout;

                if (Config.AllowPlayerToggle)
                    CommandSystem.Register("AutoSheathe", AccessLevel.Player, new CommandEventHandler(OnToggleAutoSheathe));
            }
        }

        [Usage("AutoSheathe")]
        [Description("Enables or disables the weapon auto-sheathe feature.")]
        static void OnToggleAutoSheathe(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            int key = m.Serial.Value;

            if (DisabledPlayers.Contains(key))
            {
                DisabledPlayers.Remove(key);
                m.SendMessage(68, "You have enabled the weapon auto-sheathe feature.");
            }
            else
            {
                DisabledPlayers.Add(key);
                m.SendMessage(38, "You have disabled the weapon auto-sheathe feature.");
            }
        }

        static void OnPlayerLogout(LogoutEventArgs args)
        {
            PlayerWeapons.Remove(args.Mobile.Serial.Value);
        }

        static bool AllowedToKeep(object item)
        {
            Type t = item.GetType();

            for (int i = 0; i < ItemTypesToKeepEquiped.Length; ++i)
                if (ItemTypesToKeepEquiped[i].IsAssignableFrom(t))
                    return true;

            return false;
        }

        public static void From(Mobile m)
        {
            if (m.Backpack == null || !m.Alive)
                return;

            int key = m.Serial.Value;

            if (Config.AllowPlayerToggle && DisabledPlayers.Contains(key))
                return;

            Item weapon = m.FindItemOnLayer(Layer.OneHanded);

            if (weapon == null || !weapon.Movable)
                weapon = m.FindItemOnLayer(Layer.TwoHanded);

            Item lastWeapon = null;

            if (PlayerWeapons.ContainsKey(key))
                lastWeapon = PlayerWeapons[key];

            if (m.Warmode)
            {
                if ((weapon == null || AllowedToKeep(weapon)) && lastWeapon != null && lastWeapon.IsChildOf(m.Backpack) && lastWeapon.Movable && lastWeapon.Visible && !lastWeapon.Deleted)
                {
                    m.EquipItem(lastWeapon);

                    if (Config.SendOverheadMessage)
                        m.LocalOverheadMessage(MessageType.Emote, m.EmoteHue, false, "*Unsheathes Weapon*");
                }
            }
            else
            {
                if (weapon != null && !AllowedToKeep(weapon))
                {
                    m.Backpack.DropItem(weapon);
                    PlayerWeapons[key] = weapon;

                    if (Config.SendOverheadMessage)
                        m.LocalOverheadMessage(MessageType.Emote, m.EmoteHue, false, "*Sheathes Weapon*");
                }
            }
        }
    }
}
