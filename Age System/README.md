## Introduction ##

Here is another release made for role-playing. It makes every character of shard to have an age and get older. This could be an way to introduce perma-death and family trees on the shard.

## Features ##

- Drag &amp; Drop
- Automatic age changing
- Attributes affected by age
- Display age in the name
- Population statistics
- Easy customization
- Clean Reset/Uninstall

## Installation ##

Drop AgeSystem.cs anywhere on your Scripts folder.
Login with administrator account and type [VerifyAge.
Doing this, every character will receive and Age Change Deed.
Use that to select your starting age, and tell the players to do the same.

## Configuration ##

You can change the system config at the top of the script:

    bool AutoRenewAgeEnabled = true; // Should the characters get older through time automatically?

    TimeSpan AutoRenewDelay = TimeSpan.FromDays( 15 ); // How many Earth Days are equivalent to One Year for characters?

    TimeSpan AutoRenewCheck = TimeSpan.FromMinutes( 30 ); // Check for new birthdays every 30 minutes.

    bool AgeStatModEnabled = true; // Character's stats (Str,Dex,Int) are affected by the age?

    double maxBonus = 15;  // What is the bonus when the characters are at their best condition?

    double topStrAge = 35; // At what age the characters have the best strength condition?

    double topDexAge = 20; // At what age the characters have the best dexterity condition?

    double topIntAge = 50; // At what age the characters have the best intelligence condition?

## Commands ##

**_Administrator's commands:_**

**NewAge** \- Makes all characters become one year older.

**SetAge** \- Sets the age of a character to the specified value.

**VerifyAge** \- Checks the age of all characters, sends a warning and a Age Change Dee to those who have not recorded their age, and shows statistics on the population's age.

**ClearAgeSystem** \- Removes all tags and items of the Age System from your shard. After that you can re-enable the system or delete the script from RunUO folder and restart the server.

**_Player's command:_**

**Age** \- Say your age to everyone around and toggles the age being shown in name. Also place an Age Change Deed in your backpack, if you don't have the age recorded yet.

![\[IMG\]][1]<br/>
_Not showing age._

![\[IMG\]][2]<br/>
_Showing the age in name._

## Items ##

There are two items in this package (which functions can be easily changed on script):

![\[IMG\]][3]

**Age Change Deed**: Allows the player to record their age. They must choose between a minimum and maximum value. (Default is Min: 18 Max: 40). It can't be moved, so the players can't give/sell it to others.

![\[IMG\]][4]

**Rejuvenation Potion**: Makes the character younger. The effect is variable. (Default: reduces from 1 to 5 years). May be given on quests, or you can place it on vendors.

Optional: Showing the age under the name

![\[IMG\]][5]

If you want the age to be permanently shown under the character's name, you should add a piece of code to _PlayerMobile.cs_.

Find the line: public override void GetProperties( ObjectPropertyList list )

Then add the following code right AFTER the "base.GetProperties( list );" line:

	if ( ((Account)this.Account).GetTag("Age of " + (this.RawName)) != null )
	    list.Add( "{0} Years Old", ((Account)this.Account).GetTag("Age of " + (this.RawName)) );

   [1]: http://i.imgur.com/TcaagdI.png
   [2]: http://i.imgur.com/7SMKekd.png
   [3]: http://i.imgur.com/jHVuZ9u.png
   [4]: http://i.imgur.com/d5Tm01r.png
   [5]: http://i.imgur.com/KOUAbbK.png
