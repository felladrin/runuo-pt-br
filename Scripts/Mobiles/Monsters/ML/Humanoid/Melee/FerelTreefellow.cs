using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName( "o corpo de uma árvore-viva feroz" )]
	public class FerelTreefellow : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}

		[Constructable]
		public FerelTreefellow() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
            Name = "uma árvore-viva feroz";
			Body = 301;

			SetStr( 1351, 1600 );
			SetDex( 301, 550 );
			SetInt( 651, 900 );

			SetHits( 1170, 1320 );

			SetDamage( 26, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 60, 70 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.MagicResist, 40.1, 55.0 );// Unknown
			SetSkill( SkillName.Tactics, 65.1, 90.0 );// Unknown
			SetSkill( SkillName.Wrestling, 65.1, 85.0 );// Unknown

			Fame = 12500;  //Unknown
			Karma = 12500;  //Unknown

			VirtualArmor = 24;
			PackItem( new Log( Utility.RandomMinMax( 23, 34 ) ) );
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override int GetIdleSound()
		{
			return 443;
		}

		public override int GetDeathSound()
		{
			return 31;
		}

		public override int GetAttackSound()
		{
			return 672;
		}

		public override bool BleedImmune{ get{ return true; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average ); //Unknown
		}

		public FerelTreefellow( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
