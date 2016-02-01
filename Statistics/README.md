# Statistics

This is an automated script that collects and displays shards statistics in-game and on console.

In-game command: **[Statistics**

![](screenshot.png)

All those statistics are publicly accessible, so you can integrated with other scripts, like displaying the statistics on web status page.

## Install

Just drop this script anywhere inside your Scripts folder.

## Configuration

You can change Configs on the of the script:

    bool Enabled = true;                            // Is this system enabled?
    bool ConsoleReport = true;                      // Should we report statistics on console?
    TimeSpan Interval = TimeSpan.FromMinutes(1);    // How often should we update statistics?
    AccessLevel CanSeeStats = AccessLevel.Player;   // What's the level required to see statistics in-game?
    AccessLevel CanUpdateStats = AccessLevel.Seer;  // What's the level required to update statistics in-game?

## For Developers

This is the list of the current statistics you can use on your scripts:

    TimeSpan ShardAge
    TimeSpan Uptime
    TimeSpan TotalGameTime
    DateTime LastRestart
    DateTime LastStatsUpdate
    int ActiveAccounts
    int ActiveStaffMembers
    int ActiveGuilds
    int ActiveParties
    int PlayersInParty
    int PlayerHouses
    int PlayerGold
    int PlayersOnline
    int StaffOnline

To use them, after you've installed the script, all you need to do is to refer them like this:

    Statistics.PlayerHouses

In this example, you'll get the number of player houses. You can assign to a variable, print a message with it or whatever you want.

Let's see a more complete example. Let's say you want to tell the player the Total Game Time of the shard:

    Player.SendMessage("The game time sum of all players in this shard is {0} days, {1} hours, {2} minutes and {3} seconds!", Statistics.TotalGameTime.Days, Statistics.TotalGameTime.Hours, Statistics.TotalGameTime.Minutes, Statistics.TotalGameTime.Seconds);
