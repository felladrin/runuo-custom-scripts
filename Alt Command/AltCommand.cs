//   ___|========================|___
//   \  |  Written by Felladrin  |  /   This script was released on RunUO Forums under the GPL licensing terms.
//    > |       July 2013        | <
//   /__|========================|__\   [Alternate Command] - Current version: 1.0 (July 30, 2013)

namespace Server.Commands.Generic
{
    public class AltCommand : BaseCommand
    {
        public static void Initialize()
        {
            TargetCommands.Register(new AltCommand());
        }

        public AltCommand()
        {
            AccessLevel = AccessLevel.Counselor;
            Supports = CommandSupport.All;
            Commands = new string[] { "Alt" };
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