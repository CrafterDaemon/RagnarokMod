using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class GlacialSlam : ModProjectile, ILocalizedModType
    {
        public override string Texture => "RagnarokMod/Items/HealerItems/Scythes/GlacialHarvester";

        private const int SwingDuration = 25;
        private const float ArmLength = 72f;
        private const float SweepArc = MathHelper.Pi * 1.5f; // 270°

        // Hit window as fraction of swing progress
        private const float WindupEnd = 0.28f;
        private const float FollowthroughStart = 0.82f;

        // ai[1] = swingTimer
        // ai[2] = swingBaseAngle (cocked angle at start)
        // localAI[0]: 0 = pre-swing, 1 = swinging

        private bool IsSwinging => Projectile.localAI[0] == 1f;
        private Player Owner => Main.player[Projectile.owner];

        private bool CanHit
        {
            get => Projectile.friendly;
            set => Projectile.friendly = value;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 56;
            Projectile.friendly = false;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = SwingDuration + 10;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Player owner = Owner;
            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            owner.heldProj = Projectile.whoAmI;

            if (!IsSwinging)
                BeginSwing(owner);
            else
                HandleSwing(owner);
        }

        private void BeginSwing(Player owner)
        {
            float cockedAngle = -(MathHelper.PiOver2 + (MathHelper.PiOver4 * 1.5f) * owner.direction);

            Projectile.ai[1] = 0f;
            Projectile.ai[2] = cockedAngle;
            Projectile.localAI[0] = 1f;
            Projectile.netUpdate = true;

            SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
        }

        private void HandleSwing(Player owner)
        {
            Projectile.ai[1]++;
            float progress = Projectile.ai[1] / SwingDuration;

            float startAngle = Projectile.ai[2];
            float endAngle = startAngle + SweepArc * owner.direction;

            float sweepT = MathHelper.SmoothStep(0f, 1f, Math.Max(0f, (progress - WindupEnd) / (1f - WindupEnd)));
            float curAngle = MathHelper.Lerp(startAngle, endAngle, sweepT);

            Projectile.Center = owner.Center + curAngle.ToRotationVector2() * ArmLength;
            Projectile.rotation = owner.direction == 1
                ? curAngle + MathHelper.PiOver4
                : curAngle - MathHelper.PiOver4 - MathHelper.Pi;

            owner.itemRotation = (Projectile.Center - owner.MountedCenter).ToRotation();
            if (owner.direction == -1)
                owner.itemRotation += MathHelper.Pi;
            owner.itemTime = 2;
            owner.itemAnimation = 2;

            CanHit = progress >= WindupEnd && progress <= FollowthroughStart;

            Lighting.AddLight(Projectile.Center, CanHit
                ? new Vector3(0.7f, 0.35f, 0.9f)
                : new Vector3(0.2f, 0.1f, 0.3f));

            if (Projectile.ai[1] >= SwingDuration)
            {
                Projectile p = Projectile.NewProjectileDirect(
                    Projectile.GetSource_FromThis(),
                    owner.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<GlacialWave>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner,
                    0,
                    owner.direction);
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player owner = Owner;
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = tex.Size() / 2f;

            SpriteEffects fx = owner.direction == -1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            Color trailColor = new Color(66, 189, 181);

            // Additive trail pass
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float fade = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 trailPos = Projectile.oldPos[i] + (Projectile.Size / 2f) - Main.screenPosition;
                sb.Draw(tex, trailPos, null, trailColor * fade * 0.4f,
                    Projectile.oldRot[i], origin, 1.5f, fx, 0f);
            }

            // Restore normal blend
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Main sprite
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, owner.gfxOffY);
            sb.Draw(tex, drawPos, null, lightColor, Projectile.rotation, origin, 1.5f, fx, 0f);

            return false;
        }
    }
}