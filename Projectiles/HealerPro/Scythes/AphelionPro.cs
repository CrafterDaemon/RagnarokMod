using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Core;
using RagnarokMod.Sounds;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;

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

        //private float _prevAngle;

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
            Projectile.height = 221;
            Projectile.width = 273;

            scytheCount = 2;
            dustCount = 0;
            dustType = DustID.UnusedWhiteBluePurple;
            dustScale = 1.2f;
            dustNoGravity = true;
            dustVel = new Vector2(0f, 0f);
            dustOffset = new Vector2(0f, 80f);

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



            if (!IsSunsUp && owner.ownedProjectileCounts[ModContent.ProjectileType<AphelionSun>()] < 1)
                ChargeTimer = Math.Min(ChargeTimer + 1f, MaxChargeFrames);


            if (!IsVortex && ChargeProgress >= VortexThreshold)
                TransitionToVortex(owner);

            if (!IsSunsUp && ChargeProgress >= SunsThreshold)
                TransitionToSuns(owner);
            
            if (IsSunsUp && !_collapsing
    && Projectile.owner == Main.myPlayer
    && Main.mouseRight)
            {
                SignalCollapse(owner);
            }

            if (IsSunsUp && _collapsing)
            {
                if (Projectile.localAI[2] >= 2f)
                    Projectile.Kill();
            }
        }

        private bool _collapsing;

        private void SignalCollapse(Player owner)
        {
            if (_collapsing) return;
            _collapsing = true;

            // Set collapse flag on both sun projectiles via localAI[2]
            SetSunCollapse(_sun0, owner);
            SetSunCollapse(_sun1, owner);
        }

        private static void SetSunCollapse(int whoAmI, Player owner)
        {
            if (whoAmI < 0 || whoAmI >= Main.maxProjectiles) return;
            Projectile p = Main.projectile[whoAmI];
            if (p.active && p.type == ModContent.ProjectileType<AphelionSun>())
                p.ai[2] = 1f;
            SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot with
            {
                Pitch = -1f,
                Volume = 1.5f
            }, owner.Center);
        }

        private void SpawnSupernova(Player owner)
        {
            VortexWhoAmI = -1f;
            ChargeTimer = 0f;
            State = 0f;
            _collapsing = false;
            _sun0 = -1;
            _sun1 = -1;
        }
        public override void OnKill(int timeLeft)
        {
            SpawnSupernova(Main.player[Projectile.owner]);
        }
        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300, false);
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
            Projectile.localAI[2] = 0f;
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
                (int)(Projectile.damage * 1.75f),
                Projectile.knockBack,
                Projectile.owner,
                Projectile.whoAmI,   // ai[0] = parent whoAmI
                phaseOffset);        // ai[1] = phase offset
        }

        private void OnRelease()
        {
            if (VortexWhoAmI >= 0 && VortexWhoAmI < Main.maxProjectiles)
            {
                Projectile vortex = Main.projectile[(int)VortexWhoAmI];
                if (vortex.active && vortex.type == ModContent.ProjectileType<AphelionVortex>())
                    vortex.ai[1] = 1f; // fade-out signal, handled in AphelionVortex.AI
            }
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

            float blurAngle = -MathHelper.Clamp(MathF.Abs(delta) * 8f, 0f, MathHelper.Pi);

            Color tint = Color.White;

            float trailFade = MathHelper.Lerp(0.04f, 1f, ChargeProgress);

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
                tex.Size() * 0.5f, Projectile.scale/2, fx, 0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(tex, drawPos, null, Color.White, currRot,
                tex.Size() * 0.5f, Projectile.scale/2, fx, 0f);

            _prevRotation = currRot;

            // afterimage
            float opacity = MathHelper.Lerp(0f, 0.1f, ChargeProgress);

            Color scytheColor = Color.LightGreen * opacity;
            scytheColor.A = 0;

            Texture2D slashTexture = (Texture2D)ModContent.Request<Texture2D>("RagnarokMod/Effects/Assets/Slash_3");
            float slashScale = 3.5f;
            float rotationOffset = 40;

            if (Main.player[Projectile.owner].direction != 1)
            {
                rotationOffset *= -1;
            }


            // Two forward-facing afterimages
            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + rotationOffset, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + rotationOffset, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            // Two mirrored afterimages (rotated by 180)
            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + MathHelper.Pi + rotationOffset, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + MathHelper.Pi + rotationOffset, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false;
        }
    }
}