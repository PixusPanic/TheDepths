using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheDepths.Pets.ShadowCat
{
	public class ShadowCat : ModProjectile
	{
		protected bool IsAboutToTeleport = false;
		protected bool firstPoof = false;
		protected int teleportWaitTimer;
		protected int AnimationbeginningTimer;

		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 17;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, 7, 6)
				.WithOffset(-10, -2f)
				.WithSpriteDirection(-1);
		}

		public override void SetDefaults() {
			Projectile.width = 36;
			Projectile.height = 28;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.timeLeft *= 5;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.hide = true;

			AIType = -1;
		}

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCsAndTiles.Add(index);
        }

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<ShadowCatBuff>())) {
				Projectile.timeLeft = 2;
			}

			if (player.position.X >= Projectile.Center.X)
			{
				Projectile.spriteDirection = -1;
			}
			else
            {
				Projectile.spriteDirection = 1;
            }

			Projectile.velocity.Y += 0.41f; // gravity

			if (player.position.X >= Projectile.Center.X + 500f || player.position.X <= Projectile.Center.X - 500f)
            {
				if (player.fallStart == (int)(player.position.Y / 16f) && player.releaseJump == true && player.jump == 0 && player.wingTime == player.wingTimeMax && player.justJumped == false && player.rocketDelay2 == 0)
				{
					IsAboutToTeleport = true;
                }
            }

            if (IsAboutToTeleport == true)
            {
				AnimationbeginningTimer++;
				if (Projectile.frame < 9)
                {
					Projectile.frame = 9;
                }
				if (AnimationbeginningTimer >= 8 * 4)
                {
					teleportWaitTimer++;
                }
				if (teleportWaitTimer == 1)
                {
					for (int i = 0; i < 4; i++)
					{
						Gore.NewGore(new EntitySource_Misc(""), Projectile.position, default(Vector2), GoreID.Smoke1);
						Gore.NewGore(new EntitySource_Misc(""), Projectile.position, default(Vector2), GoreID.Smoke2);
						Gore.NewGore(new EntitySource_Misc(""), Projectile.position, default(Vector2), GoreID.Smoke3);
					}
					Projectile.position.X = player.Center.X;
					Projectile.position.Y = player.Center.Y - 10;
				}
				if (teleportWaitTimer > 1)
                {
					Projectile.frame = 16;
                }
				if (teleportWaitTimer >= 20)
                {
					for (int i = 0; i < 4; i++)
					{
						Gore.NewGore(new EntitySource_Misc(""), Projectile.position, default(Vector2), GoreID.Smoke1);
						Gore.NewGore(new EntitySource_Misc(""), Projectile.position, default(Vector2), GoreID.Smoke2);
						Gore.NewGore(new EntitySource_Misc(""), Projectile.position, default(Vector2), GoreID.Smoke3);
					}
					teleportWaitTimer = 0;
					Projectile.frame = 15;
					IsAboutToTeleport = false;
					AnimationbeginningTimer = 0;
				}
            }



            Projectile.frameCounter++;

			if (Projectile.frameCounter > 6 && Projectile.frame < 8) //Idle
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame == 7)
			{
				Projectile.frame = 0;
			}
			if (Projectile.frame == 4 && Main.rand.NextBool(20))
            {
				Projectile.frame = 8;
            }
			if (Projectile.frameCounter > 6 && Projectile.frame == 8)
            {
				Projectile.frame = 5;
				Projectile.frameCounter = 0;
			}

			if (AnimationbeginningTimer == 0)
			{
				if (Projectile.frameCounter > 4 && Projectile.frame > 8 && Projectile.frame < 16) //Teleport come up
				{
					Projectile.frame--;
					Projectile.frameCounter = 0;
				}
				if (Projectile.frame == 9)
				{
					Projectile.frame = 0;
				}
			}

			if (Projectile.frameCounter > 4 && Projectile.frame > 8 && Projectile.frame < 16 && AnimationbeginningTimer > 0) //Teleport go down
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}

			if (Projectile.frame >= 16)
            {
				Projectile.frame = 16;
            }
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D glow = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture + "_Glow").Value;
			int height = texture.Height / 17;
			int y = height * Projectile.frame;
			Rectangle rect = new(0, y, texture.Width, height);
			Vector2 drawOrigin = new(texture.Width / 2, Projectile.height / 2);
			var effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(rect), Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
			Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, new Rectangle?(rect), Projectile.GetAlpha(Color.White), Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
			return false;
		}
	}
}
