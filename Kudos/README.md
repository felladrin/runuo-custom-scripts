# Kudos - Exchangeable Game Time Reward

This is an automatic system that gives an item (Kudos) for all players online every minute (this time is configurable). They can exchange their Kudos for rewards using some other scripts from the RunUO community, like the [Complete Customizable Vendor][1].

## Installation

Just drop this script somewhere in your Scripts folder.

## Configuration

You can configure this system at the top of the script.

	bool Enabled = true;                              // Is the kudos auto distribution enabled?
	int MinutesOnline = 1;                            // Every X minutes we give kudos to all players online.
	bool DropOnBank = true;                           // Should we place the kudos on character's bankbox (true) or backpack (false)?
	AccessLevel MaxAccessLevel = AccessLevel.Player;  // Any character with this access and below receives kudos.

## Screenshots

![]][2]<br/>
_Exchanging kudos at the customizable vendor_

![]][3]<br/>
_By default, players receive their kudos on their bankbox_

## Exchange price suggestion ##

- **Tiny Items:** From 10 to 1k kudos
- **Small Items:** From 1K to 2K kudos
- **Medium Items:** From 2K to 3K kudos
- **Big Items:** From 3K to 4K kudos
- **Huge Items:** From 4K to 5K kudos

   [1]: http://www.runuo.com/community/threads/runu-o-2-0-rc1-rc2-the-complete-customizable-vendor.91051/
   [2]: screenshot1.png
   [3]: screenshot2.png
