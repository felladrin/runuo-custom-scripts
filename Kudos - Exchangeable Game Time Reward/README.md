## Introduction ##

This is an automatic system that gives an item (Kudos) for all players online every minute, by default. They can exchange their Kudos for rewards using some other scripts from the RunUO community, like the [Complete Customizable Vendor][1].

## Installation ##

Just drop this script somewhere in your Scripts folder.

## Configuration ##

You can configure this system at the top of the script.

	public static int MinutesOnline = 1;                            // Every X minutes we give kudos to all players online.
	public static bool DropOnBank = true;                           // Should we place the kudos on character's bankbox (true) or backpack (false)?
	public static AccessLevel MaxAccessLevel = AccessLevel.Player;  // Any character with this access and below receives kudos.

## Screenshots ##

![\[IMG\]][2]<br/>
_Exchanging kudos at the customizable vendor_

![\[IMG\]][3]<br/>
_By default, players receive their kudos on their bankbox_

## Exchange price suggestion ##

- **Tiny Items:** From 500 to 2k kudos
- **Small Items:** From 2K to 4K kudos
- **Medium Items:** From 4K to 6K kudos
- **Big Items:** From 6K to 8K kudos
- **Huge Items:** From 8K to 10K kudos

   [1]: http://www.runuo.com/community/threads/runu-o-2-0-rc1-rc2-the-complete-customizable-vendor.91051/
   [2]: http://i.imgur.com/7k2ybTv.png
   [3]: http://i.imgur.com/L8o4YS3.png
  