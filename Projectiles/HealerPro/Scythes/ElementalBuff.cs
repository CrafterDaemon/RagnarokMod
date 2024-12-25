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
using RagnarokMod.Buffs;
using RagnarokMod.Utils;

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

            Player myPlayer = Main.player[Projectile.owner];
            RagnarokModPlayer ragnarokPlayer = myPlayer.GetRagnarokModPlayer();
            int elementIndex = ragnarokPlayer.elementalReaperIndex;

            // Apply effects based on the current element
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];

                if (player.active && !player.dead &&
                    (player.team == Main.player[Projectile.owner].team || Main.player[Projectile.owner].team == 0) &&
                    Vector2.Distance(Projectile.Center, player.Center) <= radius)
                {
                    switch (elementIndex)
                    {
                        case 0: // Sand
                            ApplyBuffWithParticles(player, ModContent.BuffType<ScytheSandBuff>(), 900, DustID.Sandstorm, Color.SandyBrown);
                            ragnarokPlayer.elementalReaperIndex++;
                            break;

                        case 1: // Water
                            ApplyBuffWithParticles(player, ModContent.BuffType<ScytheWaterBuff>(), 900, DustID.Water, Color.CornflowerBlue);
                            ragnarokPlayer.elementalReaperIndex++;
                            break;

                        case 2: // Air
                            ApplyBuffWithParticles(player, ModContent.BuffType<ScytheAirBuff>(), 900, DustID.Cloud, Color.LightGray);
                            ragnarokPlayer.elementalReaperIndex++;
                            break;

                        case 3: // Earth
                            ApplyBuffWithParticles(player, ModContent.BuffType<ScytheEarthBuff>(), 900, DustID.Stone, Color.SaddleBrown);
                            ragnarokPlayer.elementalReaperIndex++;
                            break;

                        case 4: // Brimstone
                            ApplyBuffWithParticles(player, ModContent.BuffType<ScytheBrimstoneBuff>(), 900, DustID.RedTorch, Color.OrangeRed);
                            ragnarokPlayer.elementalReaperIndex++;
                            break;

                        case 5: // Oasis
                            ApplyBuffWithParticles(player, ModContent.BuffType<ScytheOasisBuff>(), 900, DustID.Grass, Color.LimeGreen);
                            ragnarokPlayer.elementalReaperIndex = 0;
                            break;
                    }
                }
            }

            // Cycle the element for the next use
            Projectile.Kill(); // Destroy the projectile after applying the effect
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item4, Projectile.position); // Play a sound effect (e.g., Item4)
        }

        private void ApplyBuffWithParticles(Player player, int buffID, int buffDuration, int dustType, Color color)
        {
            // Apply the buff
            player.AddBuff(buffID, buffDuration);

            // Create a burst of particles around the player
            for (int j = 0; j < 20; j++) // 20 particles
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, dustType, 0f, 0f, 100, color, 1.5f);
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f); // Random circular velocity
                dust.noGravity = true; // Makes the dust float
            }
        }

    }
}
