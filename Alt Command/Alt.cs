// Alt Command v1.1.1
// Author: Felladrin
// Started: 2013-07-30
// Updated: 2016-01-21

using Server;
using Server.Commands;
using Server.Commands.Generic;

namespace Felladrin.Commands
{
    public class Alt : BaseCommand
    {
        public static void Initialize()
        {
            TargetCommands.Register(new Alt());
        }

        public Alt()
        {
            AccessLevel = AccessLevel.Counselor;
            Supports = CommandSupport.All;
            Commands = new [] { "Alt" };
            ObjectTypes = ObjectTypes.Both;
            Usage = "Alt <property name> [...]";
            Description = "Alternates between True or False one or more property by name of a targeted object.";
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            if (e.Length >= 1)
            {
                for (int i = 0; i < e.Length; ++i)
                {
                    string result = Properties.GetValue(e.Mobile, obj, e.GetString(i));

                    if (result.EndsWith("not found.") || result.EndsWith("is write only.") || result.StartsWith("Getting this property"))
                    {
                        LogFailure(result);
                    }
                    else if (result.EndsWith("= True"))
                    {
                        string setResult = Properties.SetValue(e.Mobile, obj, e.GetString(i), "False");

                        if (setResult == "Property has been set.")
                            AddResponse("The property '" + e.GetString(i) + "' has been set to 'false'.");
                        else
                            LogFailure(setResult);
                    }
                    else if (result.EndsWith("= False"))
                    {
                        string setResult = Properties.SetValue(e.Mobile, obj, e.GetString(i), "True");

                        if (setResult == "Property has been set.")
                            AddResponse("The property '" + e.GetString(i) + "' has been set to 'true'.");
                        else
                            LogFailure(setResult);
                    }
                    else
                    {
                        LogFailure(e.GetString(i) + " is not a boolean property.");
                    }
                }
            }
            else
            {
                LogFailure("Format: Alt <property name>");
            }
        }
    }
}