## Introduction ##

This is an automatic system to make characters sheathe their weapons when they change from war to peace mode. It will also reequip their last used weapon when they get back to war mode.

## Installation ##

You can put this into your shard in 2 easy steps:

1\. Drop this script somewhere in your Scripts folder.

2\. Open **PlayerMobile.cs** and find:

	public override void OnWarmodeChanged()
	{
	    if (!Warmode)
	        Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(RecoverAmmo));
	}

There, you just need to **add** the function `AutoSheatheWeapon.From(this)`.
Now your method should look like this:

	public override void OnWarmodeChanged()
	{
	    if (!Warmode)
	        Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(RecoverAmmo));
	 
	    AutoSheatheWeapon.From(this);
	}

## Configuration ##

You can configure the items that should not be unequiped when changing warmode. By default, the system will keep all kind of shields and spellbooks. You can easily add or remove type of items to keep equiped on the top of the script:

	private static Type[] ItemTypesToKeepEquiped = new Type[]
	{
	    typeof(BaseShield),
	    typeof(Spellbook)
	};

You can also enable or disable some features at the top of the script:

	public static bool SendOverheadMessage = true; // Should we send a overhead message to the player about the auto-sheathe?
	public static bool AllowPlayerToggle = true;   // Should we allow player to use a command to toggle the auto-sheathe?
  