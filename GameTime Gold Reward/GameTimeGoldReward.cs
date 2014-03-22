//   ___|========================|___
//   \  |  Written by Felladrin  |  /	This script was released on RunUO Forums under the GPL licensing terms.
//    > |      December 2010     | <
//   /__|========================|__\	[GameTime Gold Reward] - Current version: 1.1.0 (December 27, 2010)
using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Misc
{
	public class GameTimeGoldRewardTimer : Timer
	{
		//_________________________System Settings_____________________________//

		private static AccessLevel RewardAccessLevel = AccessLevel.Player; // Any character with this access and below receives the reward.

		private static int GoldQuantity = 200; // How much gold should we reward?

		private static int MinutesOnline = 3; // Every X minutes we give the reward.

		private static bool DropOnBank = true; // Should we deposit the gold on character's bankbox (true) or backpack (false)?

		private static bool MakeBankChecks = true; // Should we convert the gold rewarded to bank checks to free space on characters's bank/backpack?

		private static int MakeCheckAfterRewarded = 5000; // At what quantity should we start convert the gold to checks?

		//_____________________________________________________________________//

		public static void Initialize()
		{
			new GameTimeGoldRewardTimer().Start();
		}

		public GameTimeGoldRewardTimer() : base( TimeSpan.FromMinutes( MinutesOnline ), TimeSpan.FromMinutes( MinutesOnline ) )
		{
			Priority = TimerPriority.OneMinute;
		}

		private static int CheckWorths = (int)(MakeCheckAfterRewarded / GoldQuantity);

		private static int Ticks = 0;

		protected override void OnTick()
		{
			foreach ( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;

				if ( m != null && m is PlayerMobile && m.AccessLevel <= RewardAccessLevel )
				{
					if ( DropOnBank && m.BankBox != null)
					{
						Item gold = m.BankBox.FindItemByType( typeof(Gold) );

						if ( gold != null )
							gold.Amount += GoldQuantity;
						else
							m.BankBox.DropItem( new Gold(GoldQuantity) );

						Ticks++;

						if ( MakeBankChecks && Ticks == CheckWorths )
						{
							if ( m.BankBox.ConsumeTotal(typeof(Gold), CheckWorths*GoldQuantity) )
								m.BankBox.DropItem( new BankCheck(CheckWorths*GoldQuantity) );

							Ticks = 0;
						}
					}
					else if ( m.Backpack != null )
					{
						Item gold = m.Backpack.FindItemByType( typeof(Gold) );

						if ( gold != null )
							gold.Amount += GoldQuantity;
						else
							m.Backpack.DropItem( new Gold(GoldQuantity) );

						Ticks++;

						if ( MakeBankChecks && Ticks == CheckWorths )
						{
							if ( m.Backpack.ConsumeTotal(typeof(Gold), CheckWorths*GoldQuantity) )
								m.Backpack.DropItem( new BankCheck(CheckWorths*GoldQuantity) );

							Ticks = 0;
						}
					}
				}
			}
		}
	}
}
