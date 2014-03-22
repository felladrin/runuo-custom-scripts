## Introduction ##

Intelligent people have more influence over others. So, this script evaluates how intelligent is the player and changes the number of creatures he could control.

## Installation ##

1\. Drop this script somewhere in your Scripts folder

2\. Open it and change the config to suit your needs.

3\. Open PlayerMobile.cs and **find**, in the `ValidateEquipment_Sandbox` method, the following line:

	Mobile from = this;

**Under** that line, **add** the line bellow:

	MaxFollowersIntelBased.Evaluate(from);

4\. Restart the shard and it will automatically start evaluating the max followers of the players.

## Configuration ##

You can configure it on the first lines of the script.

	MaxIntAllowed = 150;      // What's the Intelligence limit for players?
	MaxFollowersAllowed = 10; // What's the maximum number of followers they can have?
	MinFollowersAllowed = 2;  // What's the minimum number of followers they can have?