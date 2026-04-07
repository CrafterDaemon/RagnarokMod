using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class AstralRipperPro : ScythePro2
    {
        public const int StarCount = 24;
        private const int SpawnInterval = 2; // frames between each star spawn
        private bool dying = false;
        private int _readyTimer = 0;
        private bool _clearReleaseFlag = false;
        // ai[0] = stars spawned so far
        // ai[1] = spawn timer
        // ai[2] = release flag (1 = all stars should fly off)

        public override void SafeSetDefaults()
        {
            Projectile.Size = new Vector2(228f, 232f);
            Projectile.light = 0.5f;
            dustOffset = new Vector2(0f, 30f);
            dustCount = 4;
            dustType = ModContent.DustType<AstralBlue>();
            dustType2 = ModContent.DustType<AstralOrange>();
        }

        public override void SafeAI()
        {
            Player owner = Main.player[Projectile.owner];

            // One frame after auto-release, clear the flag so the new circle can spawn
            if (_clearReleaseFlag)
            {
                Projectile.ai[2] = 0f;
                _clearReleaseFlag = false;
                Projectile.netUpdate = true;
            }

            if (owner.active && !owner.dead && owner.channel && !dying)
            {
                // Increment every frame to fight base AI decrementing it
                Projectile.timeLeft++;
            }
            else
            {
                // Channel dropped — trigger release then let the scythe die naturally
                dying = true;
                if (Projectile.ai[2] != 1f)
                {
                    Projectile.ai[2] = 1f;
                    Projectile.netUpdate = true;
                }
                return;
            }

            // Auto-release after 5 seconds of the full circle being ready
            if (Projectile.ai[0] >= StarCount && Main.netMode != NetmodeID.MultiplayerClient)
            {
                _readyTimer++;
                if (_readyTimer >= 120)
                {
                    Projectile.ai[2] = 1f;  // signal all stars to release
                    Projectile.ai[0] = 0f;  // reset star count for new circle
                    Projectile.ai[1] = 0f;  // reset spawn timer
                    _readyTimer = 0;
                    _clearReleaseFlag = true; // clear ai[2] next frame so spawning can resume
                    Projectile.netUpdate = true;
                }
            }

            // Spawn stars gradually while channeling
            if (Projectile.ai[0] < StarCount && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] >= SpawnInterval)
                {
                    Projectile.ai[1] = 0;

                    // Offset each star's starting angle to compensate for the standby orbit
                    // drift that earlier stars accumulate while later ones are still growing.
                    // Also subtract the growth-phase overshoot: during the ~60 growth frames each
                    // star orbits at OrbitSpeed instead of StandbyOrbitSpeed, pushing it ahead by
                    // (OrbitSpeed - StandbyOrbitSpeed) * growthFrames radians. Subtracting this
                    // makes the star start behind its final slot and arrive there from the back.
                    float growthFrames = AstralRipperStarPro.MaxRadius / AstralRipperStarPro.GrowthRate;
                    float angle = -(MathHelper.TwoPi * Projectile.ai[0] / StarCount)
                                + (AstralRipperStarPro.StandbyOrbitSpeed * Projectile.ai[0] * SpawnInterval)
                                - ((AstralRipperStarPro.OrbitSpeed - AstralRipperStarPro.StandbyOrbitSpeed) * growthFrames);

                    Projectile star = Projectile.NewProjectileDirect(
                        Projectile.GetSource_FromThis(),
                        owner.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<AstralRipperStarPro>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );
                    star.ai[0] = angle;
                    star.ai[1] = Projectile.whoAmI;

                    Projectile.ai[0]++;
                    Projectile.netUpdate = true;
                }
            }
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 180);
        }
    }
}