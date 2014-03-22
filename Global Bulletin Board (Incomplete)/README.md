## Introduction ##

I like the bulletin board uo system. And I've imagined it could work as an alternative to a extern forum for especific topics (suggestions, for example) or even to replace completly the a forum, on small shards.

## The idea ##

With a simple command in-game you could open the bulletin board from anywhere and follow in realtime the changes.

All they need to do is to type the command: `[BB`

![](http://i.imgur.com/oyc9v0m.png)

## The problem ##

As it's an item (based on BulletinBoard) the gump will only work if you are in range (3 tiles, considering X and Y location).

So I have to move the item to player's location when they use the command. But this makes the gump instantly close for any other player who had this opened.

I think it's because the shard sends a Remove Packet when the item is moved, which also closes the gump. This is defined on Item.cs core folder.

So I think the only way to make the gump stay opened despite the phisical item location, is changing the core file and recompling the server.

I hope I'm wrong and someone could come with a simple idea to make it work with no distro changes.

## Installation ##

Just drop this script somewhere in your Scripts folder and restart the server.

## Some notes ##

When the bulletin board changes map, it needs some time to reorganize the threads. That's why I set it to open with 1 second delay if it was last opened on another map. Otherwise it would show duplicated threads.

The bulletin board is moved 100 tiles under the player who used the command. So it shouldn't be visible, but is still accessible and the gump auto-opens after the command.

The board title is automatically set to YourShardName followed by " Bulletin Board".

It's not constructable. The system will auto-create the global bulletin board if it does not exist yet, after using the command.

Currently there's no time set to delete the threads. They shall stay there untill you delete the board. You can do it with the command `[Global Remove Where GlobalBulletinBoard`
  