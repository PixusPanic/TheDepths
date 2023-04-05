using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheDepths.Dusts
{
	public class LightningDust : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.velocity.Y = Main.rand.Next(-10, 6) * 0.1f;
			dust.velocity.X *= 0.3f;
			dust.scale *= 0.7f;
		}

		public override bool MidUpdate(Dust dust) {
			if (!dust.noGravity) {
				dust.velocity.Y += 0.05f;
			}

			if (dust.noLight) {
				return false;
			}

			float strength = dust.scale * 1.4f;
			if (strength > 1f) {
				strength = 1f;
			}
			Lighting.AddLight(dust.position, 0.226f * strength, 0.227f * strength, 0.231f * strength);
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor) 
			=> new Color(lightColor.R, lightColor.G, lightColor.B, 25);
	}
}