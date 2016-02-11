using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Menus;
using Server.Menus.Questions;
using Server.Accounting;
using Server.Multis;
using Server.Mobiles;

namespace Server.Engines.Help
{
	public class ContainedMenu : QuestionMenu
	{
		private Mobile m_From;

		public ContainedMenu( Mobile from ) : base( "Você já possui um chamado de ajuda em aberto. Nós enviaremos alguém para ajudá-lo assim que possível.  O que você gostaria de fazer?", new string[]{ "Deixe meu pedido de ajuda como está.", "Remova meu pedido de ajuda da fila." } )
		{
			m_From = from;
		}

		public override void OnCancel( NetState state )
		{
			m_From.SendLocalizedMessage( 1005306, "", 0x35 ); // Help request unchanged.
		}

		public override void OnResponse( NetState state, int index )
		{
			if ( index == 0 )
			{
				m_From.SendLocalizedMessage( 1005306, "", 0x35 ); // Help request unchanged.
			}
			else if ( index == 1 )
			{
				PageEntry entry = PageQueue.GetEntry( m_From );

				if ( entry != null && entry.Handler == null )
				{
					m_From.SendLocalizedMessage( 1005307, "", 0x35 ); // Removed help request.
					entry.AddResponse( entry.Sender, "[Cancelado]" );
					PageQueue.Remove( entry );
				}
				else
				{
					m_From.SendLocalizedMessage( 1005306, "", 0x35 ); // Help request unchanged.
				}
			}
		}
	}

	public class HelpGump : Gump
	{
		public static void Initialize()
		{
			EventSink.HelpRequest += new HelpRequestEventHandler( EventSink_HelpRequest );
		}

		private static void EventSink_HelpRequest( HelpRequestEventArgs e )
		{
			foreach ( Gump g in e.Mobile.NetState.Gumps )
			{
				if ( g is HelpGump )
					return;
			}

			if ( !PageQueue.CheckAllowedToPage( e.Mobile ) )
				return;

			if ( PageQueue.Contains( e.Mobile ) )
				e.Mobile.SendMenu( new ContainedMenu( e.Mobile ) );
			else
				e.Mobile.SendGump( new HelpGump( e.Mobile ) );
		}

		private static bool IsYoung( Mobile m )
		{
			if ( m is PlayerMobile )
				return ((PlayerMobile)m).Young;

			return false;
		}

		public static bool CheckCombat( Mobile m )
		{
			for ( int i = 0; i < m.Aggressed.Count; ++i )
			{
				AggressorInfo info = m.Aggressed[i];

				if ( DateTime.UtcNow - info.LastCombatTime < TimeSpan.FromSeconds( 30.0 ) )
					return true;
			}

			return false;
		}

		public HelpGump( Mobile from ) : base( 0, 0 )
		{
			from.CloseGump( typeof( HelpGump ) );

			bool isYoung = IsYoung( from );

			AddBackground( 50, 25, 540, 430, 2600 );

			AddPage( 0 );

			AddHtmlLocalized( 150, 50, 360, 40, 1001002, false, false ); // <CENTER><U>Ultima Online Help Menu</U></CENTER>
			AddButton( 425, 415, 2073, 2072, 0, GumpButtonType.Reply, 0 ); // Close

			AddPage( 1 );

			if ( isYoung )
			{
				AddButton( 80, 75, 5540, 5541, 9, GumpButtonType.Reply, 2 );
				AddHtml( 110, 75, 450, 58, @"<BODY><BASEFONT COLOR=BLACK><u>Transporte para Haven para Jogadores Jovens.</u> Selecione esta opção se você deseja ser transportado para Haven.</BODY>", true, true );

				AddButton( 80, 140, 5540, 5541, 1, GumpButtonType.Reply, 2 );
                AddHtml( 110, 140, 450, 58, @"<u>Questões gerais sobre o Ultima Online.</u> Selecione esta opção se você tiver uma pergunta geral sobre jogabilidade, se precisa de ajuda para aprender a usar uma habilidade, ou se gostaria de pesquisar na Base de Conhecimento do UO.", true, true );

				AddButton( 80, 205, 5540, 5541, 2, GumpButtonType.Reply, 0 );
                AddHtml( 110, 205, 450, 58, @"<u>Meu personagem está travado.</u> Esta escolha só cobre casos em que seu personagem está travado em um local do qual não pode sair. Esta opção só funcionará duas vezes a cada 24 horas.", true, true );

				AddButton( 80, 270, 5540, 5541, 0, GumpButtonType.Page, 3 );
                AddHtml( 110, 270, 450, 58, @"<u>Um jogador está me perturbando.</u> Um jogador está assediando seu personagem verbalmente. Ao selecionar esta opção, você estará enviando um log de texto para os gerenciadores do shard.", true, true );

				AddButton( 80, 335, 5540, 5541, 0, GumpButtonType.Page, 2 );
                AddHtml( 110, 335, 450, 58, @"<u>Outro.</u> Se você estiver enfrentando um problema no jogo que não se enquadram em uma das outras categorias, utilize esta opção.", true, true );
			}
			else
			{
				AddButton( 80, 90, 5540, 5541, 1, GumpButtonType.Reply, 2 );
                AddHtml( 110, 90, 450, 74, @"<u>Questões gerais sobre o Ultima Online.</u> Selecione esta opção se você tiver uma pergunta geral sobre jogabilidade, se precisa de ajuda para aprender a usar uma habilidade, ou se gostaria de pesquisar na Base de Conhecimento do UO.", true, true );

				AddButton( 80, 170, 5540, 5541, 2, GumpButtonType.Reply, 0 );
                AddHtml( 110, 170, 450, 74, @"<u>Meu personagem está travado.</u> Esta escolha só cobre casos em que seu personagem está travado em um local do qual não pode sair. Esta opção só funcionará duas vezes a cada 24 horas.", true, true );

				AddButton( 80, 250, 5540, 5541, 0, GumpButtonType.Page, 3 );
                AddHtml( 110, 250, 450, 74, @"<u>Um jogador está me perturbando.</u> Um jogador está assediando seu personagem verbalmente. Ao selecionar esta opção, você estará enviando um log de texto para os gerenciadores do shard.", true, true );

				AddButton( 80, 330, 5540, 5541, 0, GumpButtonType.Page, 2 );
                AddHtml( 110, 330, 450, 74, @"<u>Outro.</u> Se você estiver enfrentando um problema no jogo que não se enquadram em uma das outras categorias, utilize esta opção.", true, true );
			}

			AddPage( 2 );

			AddButton( 80, 90, 5540, 5541, 3, GumpButtonType.Reply, 0 );
            AddHtml( 110, 90, 450, 74, @"<u>Relatar um erro.</u> Use esta opção para iniciar o navegador web e e-mail em um relatório de bug. Seu relatório será lido por nossa equipe de Garantia da Qualidade. Pedimos desculpas por não ser capaz de responder a relatórios individuais.", true, true );

			AddButton( 80, 170, 5540, 5541, 4, GumpButtonType.Reply, 0 );
			AddHtml( 110, 170, 450, 74, @"<u>Enviar uma sugestão.</u> Escolha esta opção se você deseja fazer uma sugestão à equipe de desenvolvimento do shard.", true, true );

			AddButton( 80, 250, 5540, 5541, 5, GumpButtonType.Reply, 0 );
            AddHtml( 110, 250, 450, 74, @"<u>Gerenciamento de Conta.</u> Para questões relacionadas com a sua conta, tais como senhas esquecidas, a ativação da conta e transferência de conta, escolha esta opção.", true, true );

			AddButton( 80, 330, 5540, 5541, 6, GumpButtonType.Reply, 0 );
            AddHtml( 110, 330, 450, 74, @"<u>Outro.</u> Se você estiver enfrentando um problema no jogo que não se enquadram em uma das outras categorias, utilize esta opção.", true, true );

			AddPage( 3 );

			AddButton( 80, 90, 5540, 5541, 7, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 110, 90, 450, 145, 1062572, true, true ); /* <U><CENTER>Another player is harassing me (or Exploiting).</CENTER></U><BR>
																		 * VERBAL HARASSMENT<BR>
																		 * Use this option when another player is verbally harassing your character.
																		 * Verbal harassment behaviors include but are not limited to, using bad language, threats etc..
																		 * Before you submit a complaint be sure you understand what constitutes harassment
																		 * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=40">– what is verbal harassment? -</A>
																		 * and that you have followed these steps:<BR>
																		 * 1. You have asked the player to stop and they have continued.<BR>
																		 * 2. You have tried to remove yourself from the situation.<BR>
																		 * 3. You have done nothing to instigate or further encourage the harassment.<BR>
																		 * 4. You have added the player to your ignore list.
																		 * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=138">- How do I ignore a player?</A><BR>
																		 * 5. You have read and understand Origin’s definition of harassment.<BR>
																		 * 6. Your account information is up to date. (Including a current email address)<BR>
																		 * *If these steps have not been taken, GMs may be unable to take action against the offending player.<BR>
																		 * **A chat log will be review by a GM to assess the validity of this complaint.
																		 * Abuse of this system is a violation of the Rules of Conduct.<BR>
																		 * EXPLOITING<BR>
																		 * Use this option to report someone who may be exploiting or cheating.
																		 * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=41">– What constitutes an exploit?</a>
																		 */

			AddButton( 80, 240, 5540, 5541, 8, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 110, 240, 450, 145, 1062573, true, true ); /* <U><CENTER>Another player is harassing me using game mechanics.</CENTER></U><BR>
																		  * <BR>
																		  * PHYSICAL HARASSMENT<BR>
																		  * Use this option when another player is harassing your character using game mechanics.
																		  * Physical harassment includes but is not limited to luring, Kill Stealing, and any act that causes a players death in Trammel.
																		  * Before you submit a complaint be sure you understand what constitutes harassment
																		  * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=59"> – what is physical harassment?</A>
																		  * and that you have followed these steps:<BR>
																		  * 1. You have asked the player to stop and they have continued.<BR>
																		  * 2. You have tried to remove yourself from the situation.<BR>
																		  * 3. You have done nothing to instigate or further encourage the harassment.<BR>
																		  * 4. You have added the player to your ignore list.
																		  * <A HREF="http://uo.custhelp.com/cgi-bin/uo.cfg/php/enduser/std_adp.php?p_faqid=138"> - how do I ignore a player?</A><BR>
																		  * 5. You have read and understand Origin’s definition of harassment.<BR>
																		  * 6. Your account information is up to date. (Including a current email address)<BR>
																		  * *If these steps have not been taken, GMs may be unable to take action against the offending player.<BR>
																		  * **This issue will be reviewed by a GM to assess the validity of this complaint.
																		  * Abuse of this system is a violation of the Rules of Conduct.
																		  */

			AddButton( 150, 390, 5540, 5541, 0, GumpButtonType.Page, 1 );
			AddHtmlLocalized( 180, 390, 335, 40, 1001015, false, false ); // NO  - I meant to ask for help with another matter.
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			PageType type = (PageType)(-1);

			switch ( info.ButtonID )
			{
				case 0: // Close/Cancel
				{
					from.SendLocalizedMessage( 501235, "", 0x35 ); // Help request aborted.

					break;
				}
				case 1: // General question
				{
					type = PageType.Question;
					break;
				}
				case 2: // Stuck
				{
					BaseHouse house = BaseHouse.FindHouseAt( from );

					if ( house != null && house.IsAosRules && !from.Region.IsPartOf( typeof( Engines.ConPVP.SafeZone ) ) ) // Dueling
					{
						from.Location = house.BanLocation;
					}
					else if ( from.Region.IsPartOf( typeof( Server.Regions.Jail ) ) )
					{
						from.SendLocalizedMessage( 1114345, "", 0x35 ); // You'll need a better jailbreak plan than that!
					}
					else if ( Factions.Sigil.ExistsOn( from ) )
					{
						from.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
					}
					else if ( from is PlayerMobile && ((PlayerMobile)from).CanUseStuckMenu() && from.Region.CanUseStuckMenu( from ) && !CheckCombat( from ) && !from.Frozen && !from.Criminal && (Core.AOS || from.Kills < 5) )
					{
						StuckMenu menu = new StuckMenu( from, from, true );

						menu.BeginClose();

						from.SendGump( menu );
					}
					else
					{
						type = PageType.Stuck;
					}

					break;
				}
				case 3: // Report bug or contact Origin
				{
					type = PageType.Bug;
					break;
				}
				case 4: // Game suggestion
				{
					type = PageType.Suggestion;
					break;
				}
				case 5: // Account management
				{
					type = PageType.Account;
					break;
				}
				case 6: // Other
				{
					type = PageType.Other;
					break;
				}
				case 7: // Harassment: verbal/exploit
				{
					type = PageType.VerbalHarassment;
					break;
				}
				case 8: // Harassment: physical
				{
					type = PageType.PhysicalHarassment;
					break;
				}
				case 9: // Young player transport
				{
					if ( IsYoung( from ) )
					{
						if ( from.Region.IsPartOf( typeof( Regions.Jail ) ) )
						{
							from.SendLocalizedMessage( 1114345, "", 0x35 ); // You'll need a better jailbreak plan than that!
						}
						else if ( from.Region.IsPartOf( "Haven Island" ) )
						{
							from.SendLocalizedMessage( 1041529 ); // You're already in Haven
						}
						else
						{
							from.MoveToWorld( new Point3D( 3503, 2574, 14 ), Map.Trammel );
						}
					}

					break;
				}
			}

			if ( type != (PageType)(-1) && PageQueue.CheckAllowedToPage( from ) )
				from.SendGump( new PagePromptGump( from, type ) );
		}
	}
}