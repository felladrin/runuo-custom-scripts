## Max Followers Based On Intelligence

Intelligent people have more influence over others. So, this script evaluates how intelligent is the player and changes the number of creatures he could control.

### Installation

1\. Drop this script somewhere in your Scripts folder

2\. Open it and change the Config class to suit your needs.

3\. Open `PlayerMobile.cs` and **find**, in the `ValidateEquipment_Sandbox()` method, the following line:

	Mobile from = this;

**Below** that line, **add** the following line:

	Felladrin.Automations.MaxFollowersBasedOnIntelligence.Evaluate(from);

4\. Restart the shard and it will automatically start evaluating the players' max followers.

### Configuration ##

You can configure it on the first lines of the script.

	int IntelligenceCap = 125;      // What's the intelligence cap for players?
	int MinFollowersAllowed = 2;    // What's the minimum number of followers they can have?
	int MaxFollowersAllowed = 8;    // What's the maximum number of followers they can have?
