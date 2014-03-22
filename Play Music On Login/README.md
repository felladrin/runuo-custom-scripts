## Introduction ##

I've made this one to break the loop from the client start music. Forcing the server to play a town theme for the players when they join the game.

## Installation ##

Just drop this script somewhere in you Scripts folder.

## Configuration ##

    public static bool PlayRandomMusic = true;               // Should we play a random music from the list?
    public static MusicName SingleMusic = MusicName.Stones2; // Music to be played if PlayRandomMusic = false.

By default, it will play a random music from the list.

	public static MusicName[] MusicList = new MusicName[]
	{
	    MusicName.Stones2,
	    MusicName.Magincia,
	    MusicName.Minoc,
	    MusicName.Ocllo,
	    MusicName.Skarabra,
	    MusicName.Trinsic,
	    MusicName.Yew,
	    MusicName.InTown01,
	    MusicName.Moonglow,
	    MusicName.MinocNegative,
	    MusicName.ValoriaShips
	};

But if you set `PlayRandomMusic = false` it will play the classic Stones song.

The complete list of musics available can be found on the Server/Region.cs core script.