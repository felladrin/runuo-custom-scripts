## History Command

Player command to open a gump with the history of everything they've said and heard from other players.

If you ever needed to read the journal, but couldn't understand anything because of the awful speech hue they've chosen, this script is for you!

![](http://i.imgur.com/AplCcsZ.png)

It's based on the SpeechLog component and requires it to be activated (Note: it comes activated by default).

The gump auto refreshes whenever someone says something. The auto refresh is turned off when the player start browsing the history, so they have to call the command again to re-enable the auto refresh.

Players names are auto-colored by default, to make it easy to identify who's speaking.

You can easily configure the script on the Config class:

    bool Enabled = true;               // Is this command enabled?
    bool AutoRefreshEnabled = true;    // Is the auto refresh enabled?
    bool AutoColoredNames = true;      // Should we auto color the players names?
    bool OpenGumpOnLogin = true;       // Should we display the gump when player logs in?
    int MaxMessagesPerPage = 19;       // How many messages should we display per page?