//   ___|========================|___
//   \  |  Written by Felladrin  |  /   [Play Music On Login] - Current version: 1.1 (September 01, 2013)
//    > |        July 2013       | <
//   /__|========================|__\   Description: Plays a specific or random music for players on login.

namespace Server.Network
{
    public class PlayMusicOnLogin
    {
        public static class Config
        {
            public static bool PlayRandomMusic = true;               // Should we play a random music from the list?
            public static MusicName SingleMusic = MusicName.Stones2; // Music to be played if PlayRandomMusic = false.
        }
        
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
        
        public static void Initialize()
        {
            EventSink.Login += new LoginEventHandler(onLogin);
        }

        private static void onLogin(LoginEventArgs args)
        {
            MusicName toPlay = Config.SingleMusic;

            if (Config.PlayRandomMusic)
                toPlay = MusicList[Utility.Random(MusicList.Length)];
            
            args.Mobile.Send(PlayMusic.GetInstance(toPlay));
        }
    }
}