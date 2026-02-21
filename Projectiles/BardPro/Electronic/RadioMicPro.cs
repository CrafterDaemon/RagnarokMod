using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Buffs;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Electronic
{
    public class RadioMicPro : BardProjectile, ILocalizedModType
    {
        public override string Texture => "Terraria/Images/Projectile_645";

        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

        public bool hasHit = false;

        public override void SetBardDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.scale = 1f;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Prevent default drawing of the projectile's texture
            return false;
        }

        public override void AI()
        {
            Projectile.ai[1] += 1f;

            if (Projectile.ai[1] >= 0f)
            {
                int dustType;

                if (Projectile.ai[0] == 1f)
                {
                    dustType = DustID.GreenTorch;
                }
                else
                {
                    dustType = DustID.PinkTorch;
                }

                Projectile.scale += 0.4f;

                int numDusts = 30;
                for (int i = 0; i < numDusts; i++)
                {
                    Vector2 offset = Vector2.UnitY.RotatedBy((float)i * MathHelper.TwoPi / numDusts) * new Vector2(6f, 14f) * Projectile.scale;
                    offset = offset.RotatedBy(Projectile.velocity.ToRotation());

                    Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, dustType, offset.SafeNormalize(Vector2.UnitY), 0, default, 1f);
                    dust.noGravity = true;
                }

                Projectile.ai[1] = -5f;
            }

            // Resize based on scale
            Vector2 center = Projectile.Center;
            Projectile.Size = new Vector2(20f * Projectile.scale);
            Projectile.Center = center;
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 1f)
            {
                if (!hasHit)
                {
                    int proj = Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        target.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<TendrilStrike>(),
                        Projectile.damage,
                        0f,
                        Projectile.owner
                    );
                    Main.projectile[proj].ai[1] = target.whoAmI;
                    hasHit = true;

                    SoundEngine.PlaySound(RagnarokModSounds.RadioDemon, target.Center);
                }

                target.AddBuff((BuffID.CursedInferno), 180);
                if (Main.player[Projectile.owner].GetRagnarokModPlayer().redglassMonocle)
                    target.AddBuff(ModContent.BuffType<Charmed>(), 300);
            }
            else
            {
                // Normal pink behavior
                for (int i = 0; i < 2; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Unit() * 6f;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        target.Center,
                        velocity,
                        ModContent.ProjectileType<RadioMicShadowBurst>(),
                        Projectile.damage / 10,
                        1f,
                        Projectile.owner
                    );
                }

                target.AddBuff(BuffID.ShadowFlame, 180);
                SoundEngine.PlaySound(SoundID.Item20, target.Center);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.ai[0] == 1f)
                return Color.LimeGreen;

            return Color.HotPink;
        }
    }
}