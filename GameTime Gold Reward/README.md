## Introduction ##

This script, intended for family shards, gives a reward in gold for players being online.

## Features ##

- Drag & Drop.
- Easy configuration.
- Automatically makes bankchecks after certain quantity of gold rewarded. (It only makes checks for gold rewarded by this system. Gold that the player has got by other ways will remain intact.)

## Configuration ##


    AccessLevel RewardAccessLevel = AccessLevel.Player; // Any character with this access and below receives the reward.
     
    int GoldQuantity = 200; // How much gold should we reward?
     
    int MinutesOnline = 3; // Every X minutes we give the reward.
     
    bool DropOnBank = true; // Should we deposit the gold on character's bankbox (true) or backpack (false)?
     
    bool MakeBankChecks = true; // Should we convert the gold rewarded to bank checks to free space on characters's bank/backpack?
     
    int MakeCheckAfterRewarded = 5000; // At what quantity should we start convert the gold to checks?

## Usage ##

To install just drop the script anywhere in your Scripts folder, and to uninstall simply delete it.

Open the script, change the settings to fit your shard and restart the server.

It's done. Your players should now receive the reward for being online.