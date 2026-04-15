using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;
using RagnarokMod.Core;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    /// <summary>
    /// Main held projectile for the Aphelion scythe.
    ///
    /// State machine (localAI[0]):
    ///   0 = SPINNING   — building charge, smear shader active
    ///   1 = VORTEX     — portal distortion spawned (40% charge)
    ///   2 = SUNS_UP    — both suns alive (100% charge)
    ///
    /// ai[0]      = charge timer  (0 → MaxChargeFrames), synced
    /// localAI[0] = state
    /// localAI[1] = vortex projectile whoAmI  (-1 = none)
    /// </summary>
    public class AphelionPro : ScythePro2
    {


        public const int MaxChargeFrames = 180;
        private const float VortexThreshold = 0.40f;
        private const float SunsThreshold = 1.00f;

        private const float MinRotSpeed = 0.08f;   
        private const float MaxRotSpeed = 0.35f;   
        private const float SpinCurve = 0.55f;

        private float ChargeTimer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        private float State
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        private float VortexWhoAmI
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        private float ChargeProgress =>
            Math.Min(ChargeTimer / MaxChargeFrames, 1f);

        private float CurrentRotSpeed =>
            MathHelper.Lerp(MinRotSpeed, MaxRotSpeed,
                MathF.Pow(ChargeProgress, SpinCurve));

        private bool IsVortex => State >= 1f;
        private bool IsSunsUp => State >= 2f;

        private float _prevAngle;

        private int _sun0 = -1;
        private int _sun1 = -1;

        private static Effect SmearEffect => RagnarokShaders.AphelionSmear.Value;

        // Track previous rotation to compute angular delta for blur width.
        private float _prevRotation;
        private bool _firstFrame = true;

        public override void SafeSetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 3;
            Projectile.usesIDStaticNPCImmunity = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.height = 442;
            Projectile.width = 546;

            scytheCount = 1;
            dustCount = 2;
            dustType = DustID.UnusedWhiteBluePurple;
            dustScale = 1.2f;
            dustNoGravity = true;
            dustVel = new Vector2(0f, -1f);

            VortexWhoAmI = -1f;
            rotationSpeed = MinRotSpeed;
        }

        public override void SafeAI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!owner.channel)
            {
                OnRelease();
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 3;

            rotationSpeed = CurrentRotSpeed;

            if (!IsSunsUp)
                ChargeTimer = Math.Min(ChargeTimer + 1f, MaxChargeFrames);

            if (!IsVortex && ChargeProgress >= VortexThreshold)
                TransitionToVortex(owner);

            if (!IsSunsUp && ChargeProgress >= SunsThreshold)
                TransitionToSuns(owner);

            float g = ChargeProgress;
        }

        private void TransitionToVortex(Player owner)
        {
            State = 1f;

            int vid = Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                owner.Center,
                Vector2.Zero,
                ModContent.ProjectileType<AphelionVortex>(),
                0, 0f,
                Projectile.owner,
                Projectile.whoAmI);   // ai[0] = parent whoAmI

            VortexWhoAmI = vid;

            SoundEngine.PlaySound(SoundID.Item84 with
            {
                Pitch = -0.3f,
                Volume = 0.7f
            }, owner.Center);
        }

        private void TransitionToSuns(Player owner)
        {
            State = 2f;
            _sun0 = SpawnSun(owner, 0f);
            _sun1 = SpawnSun(owner, MathHelper.Pi);

            SoundEngine.PlaySound(SoundID.Item84 with
            {
                Pitch = 0.4f,
                Volume = 1.0f
            }, owner.Center);

            if (Main.LocalPlayer.Distance(owner.Center) < 1200f)
                Main.LocalPlayer.Calamity().GeneralScreenShakePower = 7f;
        }

        private int SpawnSun(Player owner, float phaseOffset)
        {
            return Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                owner.Center,
                Vector2.Zero,
                ModContent.ProjectileType<AphelionSun>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner,
                Projectile.whoAmI,   // ai[0] = parent whoAmI
                phaseOffset);        // ai[1] = phase offset
        }

        private void OnRelease()
        {
            KillChild((int)VortexWhoAmI);
            KillChild(_sun0);
            KillChild(_sun1);
        }

        private static void KillChild(int whoAmI)
        {
            if (whoAmI < 0 || whoAmI >= Main.maxProjectiles) return;
            Projectile p = Main.projectile[whoAmI];
            if (p.active) p.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            SpriteBatch sb = Main.spriteBatch;
            Effect effect = SmearEffect;
            Player owner = Main.player[Projectile.owner];

            float currRot = Projectile.rotation;
            float prevRot = _firstFrame ? currRot : _prevRotation;
            _firstFrame = false;

            float delta = currRot - prevRot;
            if (delta > MathHelper.Pi) delta -= MathHelper.TwoPi;
            if (delta < -MathHelper.Pi) delta += MathHelper.TwoPi;

            float blurAngle = MathHelper.Clamp(MathF.Abs(delta) * 4f, 0f, MathHelper.Pi);

            Color tint = Color.Lerp(
                Color.Lerp(lightColor, new Color(180, 220, 255), 0.6f),
                Color.Lerp(lightColor, new Color(255, 200, 80), 0.8f),
                ChargeProgress);

            float trailFade = MathHelper.Lerp(0.25f, 0.04f, ChargeProgress);

            effect.Parameters["blurAngle"].SetValue(blurAngle);
            effect.Parameters["trailFade"].SetValue(trailFade);
            effect.Parameters["tintColor"].SetValue(tint.ToVector4());

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, effect,
                Main.GameViewMatrix.TransformationMatrix);

            effect.CurrentTechnique.Passes[0].Apply();

            SpriteEffects fx = owner.direction == 1
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;

            Vector2 drawPos = Projectile.Center - Main.screenPosition
                            + new Vector2(0f, owner.gfxOffY);

            sb.Draw(tex, drawPos, null, Color.White, currRot,
                tex.Size() * 0.5f, Projectile.scale, fx, 0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(tex, drawPos, null, lightColor, currRot,
                tex.Size() * 0.5f, Projectile.scale, fx, 0f);

            _prevRotation = currRot;
            return false;
        }
    }
}