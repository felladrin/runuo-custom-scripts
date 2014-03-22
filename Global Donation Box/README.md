## Introduction ##

This is a Donation Box that can be accessed directly and indirectly. Its multi-mode allow you to choose if only-youngs or everyone can use the box, if there's a limit of items that can be taken, if players are able to access it if they are far away, or if it will work as a standard donation box. Also it will automatically organize the items into categories.

## Screenshots ##

![](http://i.imgur.com/nYhnII2.png)

![](http://i.imgur.com/Mn91Jxw.png)

## Features ##

- Drag &amp; Drop
- Multi-mode System
- Accessible from anywhere
- Unlimited items inside the box
- Auto-organized items inside the box
- Easy to donate and get donations
- Easy configuration and customization
- Clean reset and uninstall

## Installation ##

Just drop this script somewhere in your Scripts folder.

## Configuration ##

Open it and configure the settings at the top of the script:

	public static bool OnlyYoungs = false;                                 // Change this to true if you want only youngs to be able to open the box.
	public static int DonationsLimit = 0;                                  // Choose the maximum number of times each player can open the box. Leave it as 0 (zero) to make it unlimited.
	public static bool CanBeGloballyAccessed = false;                      // Can players use commands to get items from the box wherever they are?
	public static bool AutoOrganizerEnabled = true;                        // Should the items in the box be automatically organized in categories?
	public static TimeSpan AutoOrganizerDelay = TimeSpan.FromMinutes(10);  // How often should the box be automatically organized?

Then login with an Owner or Admin account and type `[Donations` to place the box at your position.

## Reset ##

If you have activated the Donations Limit, when you delete (ingame) the Donation Box, all the players will have their Donations Taken count reseted.
Note: All the items inside the box will be deleted.

## Uninstall ##

Just delete the script and restart the server.
Note: All the items inside the box will be deleted.

## Commands ##

> **[Donate**<br/>
Used to donate items from your backpack. Also to move the backpacks inside the Donation Box. (It sends an emote and plays a sound when you donate something).

> **[Donations**<br/>
Opens the Donation Box, wherever you are, and allow you to get items via target. (If you target a Donation Backpack, it will open the backpack, so you can target an item inside).

## Some Notes ##

Owners and Admins can donate items that are not on their backpacks using the [Donate command.

The AutoOrganizer makes a recursive check on the items inside the box, so if there are containers inside containers, its contents will be categorized, as all other items, until finally every container (except donations backpacks) to be empty. It may take some minutes to empty all containers, as it only take care of 2 subcontainers per check.

To donate an item and make the emotions, the players should drop the item on the box (visually), if they are close to it. If they drop into it, the item will be donated, but no message will appear.

If you start using it with `CanBeGloballyAccessed = true` and later change to `false` (or vice-versa), you'll need to delete de Donation Box, and then type `[Donations` again to create a new box. So if you want to keep the items already collected, get them to your backpack before deleting it.