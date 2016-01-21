# Auto Split Gold

This is a plug&play script that automatically splits gold among the party members.

So whenever a party member and gets some gold from a corpse, the gold is equally split and all members receive their share. The rest of the division is kept with the sharer. Also, if a member is overloaded, his share is kept with the sharer.

Players not in party won't be affected.

## Install

1\. Drop this script anywhere inside your Scripts folder.  
2\. On `Scripts/Items/Misc/Corpses/Corpse.cs` find the method:

        CheckLift(Mobile from, Item item, ref LRReason reject)

   Then, **above** its **last 'return' statement**, add the following line:

        if (Felladrin.Automations.AutoSplitGold.Split(from, item)) return false;
