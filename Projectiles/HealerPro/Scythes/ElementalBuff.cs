using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ElementalBuff : ModProjectile, ILocalizedModType
    {
        public override string Texture => "RagnarokMod/Projectiles/NoProj";
        public override void SetStaticDefaults() { }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 40;
        }

        public override void AI()
        {
            // Radius in pixels (200 tiles * 16 pixels per tile)
            float radius = 200 * 16;
            // Apply the buff (e.g., BuffID.Regeneration for 10 seconds)
            Player myPlayer = Main.player[Projectile.owner];
            myPlayer.AddBuff(BuffID.Regeneration, 600);

            // Create a burst of particles around the player
            for (int j = 0; j < 20; j++) // 20 particles
            {
                Dust dust = Dust.NewDustDirect(myPlayer.position, myPlayer.width, myPlayer.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 1.5f);
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f); // Random circular velocity
                dust.noGravity = true; // Makes the dust float
            }
            // Loop through all active players
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];

                // Check if the player is active, alive, on the same team, and within the radiuss
                if (player.active && !player.dead && Projectile.owner == Main.myPlayer &&
                    (player.team == Main.player[Projectile.owner].team || Main.player[Projectile.owner].team == 0) &&
                    Vector2.Distance(Projectile.Center, player.Center) <= radius)
                {
                    // Apply the buff (e.g., BuffID.Regeneration for 10 seconds)
                    player.AddBuff(BuffID.Regeneration, 600);

                    // Create a burst of particles around the player
                    for (int j = 0; j < 20; j++) // 20 particles
                    {
                        Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 1.5f);
                        dust.velocity = Main.rand.NextVector2Circular(3f, 3f); // Random circular velocity
                        dust.noGravity = true; // Makes the dust float
                    }
                }
            }

            // Optionally, destroy the projectile after applying the effect
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            // Visual explosion effect when the projectile is destroyed
            for (int i = 0; i < 30; i++) // 30 particles
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, 2f);
                dust.velocity = Main.rand.NextVector2Circular(5f, 5f); // Random circular velocity
                dust.noGravity = true; // Makes the dust float
            }

            SoundEngine.PlaySound(SoundID.Item4, Projectile.position); // Play a sound effect (e.g., Item4)
        }

    }
}
