//   ___|========================|___
//   \  |  Written by Felladrin  |  /   This script was released on RunUO Community under the GPL licensing terms.
//    > |      August 2013       | <
//   /__|========================|__\   [ProTag] PlayerMobile Tag-Based Properties - Current version: 1.2 (August 14, 2013)

using Server.Mobiles;
using Server.Commands;
using Server.Targeting;
using Server.Accounting;

namespace Server
{
    public static class ProTag
    {
        public static void Initialize()
        {
            CommandSystem.Register("ProTag", AccessLevel.Counselor, new CommandEventHandler(ProTag_OnCommand));
        }

        [Usage("ProTag")]
        [Description("Sets, gets or deletes a ProTag from a targeted PlayerMobile.")]
        private static void ProTag_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (e.Length == 0)
            {
                m.SendMessage("Who do you want to get the ProTags from?");
                m.Target = new GetAllProTags();
            }
            else if (e.Length == 1 && e.GetString(0) == "ClearAll")
            {
                DeleteAllProTags(m);
            }
            else if (e.Length >= 2)
            {
                string subcommand = e.GetString(0);
                string property = e.GetString(1);

                switch (subcommand.ToLower())
                {
                    case "set":
                        if (e.Length == 3)
                        {
                            string value = e.GetString(2);
                            m.SendMessage("Who do you want to set this ProTag?");
                            m.Target = new SetProTag(property, value);
                        }
                        else
                        {
                            m.SendMessage("Usage: [ProTag Set <property> <value>");
                        }
                        break;
                    case "get":
                        m.SendMessage("Who do you want to get this ProTag from?");
                        m.Target = new GetProTag(property);
                        break;
                    case "del":
                        m.SendMessage("Who do you want to delete this ProTag from?");
                        m.Target = new DelProTag(property);
                        break;
                    default:
                        m.SendMessage("Usage: [ProTag [GET|SET|DEL] <property>");
                        break;
                }
            }
            else
            {
                m.SendMessage("Usage: [ProTag [GET|SET|DEL] <property>");
            }
        }

        private class GetAllProTags : Target
        {
            public GetAllProTags() : base(-1, false, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    PlayerMobile pm = targeted as PlayerMobile;

                    if (pm.Account == null)
                    {
                        from.SendMessage(38, "{0} has no account!", pm.Name);
                        return;
                    }

                    Account acc = pm.Account as Account;

                    if (acc.Tags.Count == 0)
                    {
                        from.SendMessage("There are no ProTags set for {0}.", pm.Name);
                        return;
                    }

                    bool noProTagsFound = true;

                    for (int i = 0; i < acc.Tags.Count; ++i)
                    {
                        AccountTag tag = acc.Tags[i];

                        if (tag.Name.StartsWith(ProTagPrefix(pm)))
                        {
                            from.SendMessage("{0} = {1}", tag.Name.Replace(ProTagPrefix(pm), ""), tag.Value);
                            noProTagsFound = false;
                        }
                    }

                    if (noProTagsFound)
                    {
                        from.SendMessage("There are no ProTags set for {0}.", pm.Name);
                    }
                }
                else
                {
                    from.SendMessage(38, "You can only use this command on PlayerMobiles.");
                }
            }
        }

        private class SetProTag : Target
        {
            string tagName;
            string tagValue;

            public SetProTag(string property, string value)
                : base(-1, false, TargetFlags.None)
            {
                tagName = property;
                tagValue = value;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    PlayerMobile pm = targeted as PlayerMobile;

                    if (pm.Account == null)
                    {
                        from.SendMessage(38, "{0} has no account!", pm.Name);
                        return;
                    }

                    Account acc = pm.Account as Account;
                    acc.SetTag(FormatTagName(pm, tagName), tagValue);
                    from.SendMessage(68, "Property '{0}' of {1} has been changed to '{2}' succesful.", tagName, pm.Name, tagValue);
                }
                else
                {
                    from.SendMessage(38, "You can only use this command on PlayerMobiles.");
                }
            }
        }

        private class GetProTag : Target
        {
            string tagName;

            public GetProTag(string property)
                : base(-1, false, TargetFlags.None)
            {
                tagName = property;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    PlayerMobile pm = targeted as PlayerMobile;

                    if (pm.Account == null)
                    {
                        from.SendMessage(38, "{0} has no account!", pm.Name);
                        return;
                    }

                    Account acc = pm.Account as Account;
                    string tagValue = acc.GetTag(FormatTagName(pm, tagName));

                    if (tagValue != null)
                    {
                        from.SendMessage(68, "Property '{0}' of {1} is currently set to '{2}'.", tagName, pm.Name, tagValue);
                    }
                    else
                    {
                        from.SendMessage("Property '{0}' of {1} does not exist.", tagName, pm.Name, tagValue);
                    }
                }
                else
                {
                    from.SendMessage(38, "You can only use this command on PlayerMobiles.");
                }
            }
        }

        private class DelProTag : Target
        {
            string tagName;

            public DelProTag(string property)
                : base(-1, false, TargetFlags.None)
            {
                tagName = property;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    PlayerMobile pm = targeted as PlayerMobile;

                    if (pm.Account == null)
                    {
                        from.SendMessage(38, "{0} has no account!", pm.Name);
                        return;
                    }

                    Account acc = pm.Account as Account;

                    string tagValue = acc.GetTag(FormatTagName(pm, tagName));

                    if (tagValue != null)
                    {
                        acc.RemoveTag(FormatTagName(pm, tagName));
                        from.SendMessage(68, "Property '{0}' of {1} has been deleted.", tagName, pm.Name);
                    }
                    else
                    {
                        from.SendMessage("Property '{0}' of {1} does not exist.", tagName, pm.Name, tagValue);
                    }
                }
                else
                {
                    from.SendMessage(38, "You can only use this command on PlayerMobiles.");
                }
            }
        }

        private static void DeleteAllProTags(Mobile from)
        {
            int deletedProTags = 0;

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is PlayerMobile)
                {
                    PlayerMobile pm = m as PlayerMobile;

                    if (pm.Account == null)
                        continue;

                    Account acc = pm.Account as Account;

                    for (int i = acc.Tags.Count - 1; i >= 0; --i)
                    {
                        if (i >= acc.Tags.Count)
                            continue;

                        AccountTag tag = acc.Tags[i];

                        if (tag.Name.StartsWith(ProTagPrefix(pm)))
                        {
                            acc.Tags.RemoveAt(i);
                            deletedProTags++;
                        }
                    }
                }
            }

            from.SendMessage(68, "A total of {0} ProTags have been deleted.", deletedProTags);
        }

        /// <summary>
        /// Modifies an existing property or adds a new one if it doesn't exist.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public static void Set(PlayerMobile pm, string property, string value)
        {
            if (pm.Account == null)
                return;

            Account acc = pm.Account as Account;

            acc.SetTag(FormatTagName(pm, property), value);
        }

        /// <summary>
        /// Gets the value of a property or NULL if the property isn't set.
        /// </summary>
        /// <param name="pm">PlayerMobile from who we want to get the property.</param>
        /// <param name="property">Property name.</param>
        /// <returns>The value of the property.</returns>
        public static string Get(PlayerMobile pm, string property)
        {
            if (pm.Account == null)
                return "No Account";

            Account acc = pm.Account as Account;

            string value = acc.GetTag(FormatTagName(pm, property));

            return value;
        }

        /// <summary>
        /// Deletes the specified property from the PlayerMobile.
        /// </summary>
        /// <param name="pm">PlayerMobile from who we want to delete the property.</param>
        /// <param name="property">Property name to remove.</param>
        public static void Del(PlayerMobile pm, string property)
        {
            if (pm.Account == null)
                return;

            Account acc = pm.Account as Account;

            acc.RemoveTag(FormatTagName(pm, property));
        }

        private static string FormatTagName(PlayerMobile pm, string tagName)
        {
            return ProTagPrefix(pm) + tagName.ToLower().Replace(" ", "_");
        }

        private static string ProTagPrefix(PlayerMobile pm)
        {
            return string.Format("ProTag_{0}_", pm.Serial.ToString());
        }
    }
}
