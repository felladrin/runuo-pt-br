using System;
using Server.Network;

namespace Server.Misc
{
	public class LoginStats
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.Login += new LoginEventHandler( EventSink_Login );
		}

		private static void EventSink_Login( LoginEventArgs args )
		{
			int userCount = NetState.Instances.Count;
			int itemCount = World.Items.Count;
			int mobileCount = World.Mobiles.Count;

			Mobile m = args.Mobile;

			m.SendMessage(
                "Bem-vindo, {0}! No momento há {1} jogador{2} online, com {3} ite{4} e {5} mobile{6} no mundo.",
				args.Mobile.Name,
				userCount,
                userCount == 1 ? "" : "es",
				itemCount,
                itemCount == 1 ? "m" : "ns",
				mobileCount,
                mobileCount == 1 ? "" : "s" 
            );
		}
	}
}