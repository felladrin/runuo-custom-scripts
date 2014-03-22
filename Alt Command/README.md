## Introduction ##

Simple addition for the Counselor's Commands, to alternate between True or False any boolean property of a targeted item or mobile.

So the targeted object will have its property:

- set to True if it was False.
- set to False if it was True.

Command accepts all modifiers: Global, Online, Region, Contained, Multi, Area and Self as it's based on the Get/Set RunUO command.

## Instalation ##

Just drop it somewhere on your Scripts folder.

## Usage Examples ##

1\. You need to remove or recover the Young status of a player:
> [alt young

2\. You know that three properties of a player (Young, Hidden, DisplayGuildTitle) are set to "true", "false", "true", respectively. And you want to set them to "false", "true", "false". (Not so common, but it is useful sometimes):

> [alt young hidden displayguildtitle

3\. During an event which requires team work, half of the players start paralysed and half free. They can move to get to their objetives only during 10 seconds, then a Staff member use the command to paralyse those who are moving and unparalyse the others, and so on till the event end. He could easily use:

> [area alt paralysed

## Last Words ##

Well, all that could be achieved with [set command, but I really think the [alt is useful, mainly during events. So that's why I'm sharing this.

Hope you find good uses for it.  