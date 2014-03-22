## Introduction ##

This is a Plug &amp; Play Utility to help you showing/using statistics from the shard.

I've listed the most common statistics of the shard. If you have any suggestion about stats to be added, feel free to ask. You can also implement them by yourself, it's pretty easy.

## Functionality ##

By default, it will start collect statistics as soon you start the shard, and auto-update them every 5 minutes. Also, the statistics will be reported on console.

![\[IMG\]][1]

You can change this on the Config class, at the top of the script.

	bool Enabled = true;                            // Is this system enabled?
	bool ConsoleReport = true;                      // Should we report statistics on console?
	int Interval = 5;                               // What's the statistics update interval, in minutes?
	AccessLevel CanSeeStats = AccessLevel.Player;   // What's the level required to see statistics in-game?
	AccessLevel CanUpdateStats = AccessLevel.Seer;  // What's the level required to update statistics in-game?

Players in-game can check the status using the command:


> **[Statistics** (Shows the gump below)

![\[IMG\]][2]

And your staff members can force the statistics update using the command:

> **[UpdateStatistics** (By default, accessible to Seers and above levels).

## Installation ##

Just drop this script somewhere in your Scripts folder and restart the shard.

## For Developers ##

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

## Last Words ##

You probably won't need to add any "using" statement, as Statistics are on Server namespace.

   [1]: http://i.imgur.com/zbtSb7g.png
   [2]: http://i.imgur.com/hOn9wZQ.png
  