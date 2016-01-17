// Alt Command v1.1.0
// Author: Felladrin
// Started: 2013-07-30
// Updated: 2016-01-17

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
            Usage = "Alt <propertyName> [...]";
            Description = "Alternates between True or False one or more property by name of a targeted object.";
        }

        public override void Execute(CommandEventArgs e, object obj)
        {
            if (e.Length >= 1)
            {
                for (int i = 0; i < e.Length; ++i)
                {
                    string result = Properties.GetValue(e.Mobile, obj, e.GetString(i));

                    if (result == "Property not found." || result == "Property is write only." || result.StartsWith("Getting this property"))
                    {
                        LogFailure(result);
                    }
                    else if (result.EndsWith("= True"))
                    {
                        string setResult = Properties.SetValue(e.Mobile, obj, e.GetString(i), "False");

                        if (setResult == "Property has been set.")
                            AddResponse(setResult);
                        else
                            LogFailure(setResult);
                    }
                    else if (result.EndsWith("= False"))
                    {
                        string setResult = Properties.SetValue(e.Mobile, obj, e.GetString(i), "True");

                        if (setResult == "Property has been set.")
                            AddResponse(setResult);
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
                LogFailure("Format: Alt <propertyName>");
            }
        }
    }
}