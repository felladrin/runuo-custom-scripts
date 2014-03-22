## Introduction ##

Something I like in some games is that you can't run all day long, even if you are an athlete. That's why the mounts are so important.

**With this script your players get tired regardless of their level of Focus.**

![](http://i.imgur.com/MLma4xf.png)

## Installation ##

Just drop it somewhere in your Scripts folder.

## Some Notes ##

As it can unbalance the UO Combat, it's recommended only for RP shards.

The system only affect players. No staff members will be affected.

Mounts are not affected by this system. They can still run all they long. So do the players, while mounted.

If you want to get rid of the message "You are too fatigued to move" when you reach 0 stamina, you can edit `WeightOverloading.cs`. Just comment/remove this part:

	if ( from.Stam == 0 )
	{
	    from.SendLocalizedMessage( 500110 ); // You are too fatigued to move.
	    e.Blocked = true;
	    return;
	}
  