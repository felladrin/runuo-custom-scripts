## Auto Ignite Lights ##

Automatically ignites all game lights when night comes, and douse them at the morning.

Note: Lights on player's hand or backpack are not affected. Neither the lights stored on containers.

## Installation ##

Just drop this scipt somewhere on your Scripts folder.

## Configuration ##

You can change some settings on the top of the script.

	public static bool Enabled = true; // Is this system enabled?
	public static int IgniteHour = 18; // At what hour should we ignite the lights?
	public static int DouseHour = 7; // At what hour should we douse the lights?
	public static int CheckInterval = 5; // How often, in minutes, should we check the lights?