using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class AsterionConstellation : ModProjectile, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Particles/Sparkle";

        private const float RegenRadius = 400f;
        private const float FrostRadius = 400f;
        private const int StarburstDrainInterval = 12;

        private const int PulseInterval = 60;
        private int _pulseTimer = 0;
        private float _pulseFade = 0f;

        private const int SpawnTime = 60;
        private int _spawnTimer = 0;
        private float SpawnProgress => MathHelper.Clamp(_spawnTimer / (float)SpawnTime, 0f, 1f);

        public List<ConstellationSegment> Segments = new();

        private static Asset<Texture2D> GlowTex = null;
        private static Texture2D GetGlowTex()
        {
            GlowTex ??= ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle");
            return GlowTex.Value;
        }

        public class ConstellationSegment
        {
            public Vector2 Center;
            public ConstellationSegment(Vector2 center) { Center = center; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
        }

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            BuildSegments();
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.velocity = Vector2.Zero;

            if (_spawnTimer < SpawnTime)
            {
                _spawnTimer++;

                if (Main.rand.NextBool(3))
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    float dist = Main.rand.NextFloat(20f, 80f) * SpawnProgress;
                    Dust d = Dust.NewDustPerfect(
                        Projectile.Center + angle.ToRotationVector2() * dist,
                        DustID.BlueFairy,
                        angle.ToRotationVector2() * Main.rand.NextFloat(0.5f, 2f),
                        0, default, Main.rand.NextFloat(0.8f, 1.5f));
                    d.noGravity = true;
                }
                return;
            }
            Projectile.ai[1] = 1f;
            bool dismissed = Projectile.ai[0] == 1f;
            if (dismissed)
            {
                if (Projectile.timeLeft > 60)
                    Projectile.timeLeft = 60;
                return;
            }

            if (owner.Calamity().StratusStarburst > 0)
                Projectile.timeLeft++;
            else if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
                return;
            }

            if (Main.GameUpdateCount % StarburstDrainInterval == 0 && owner.Calamity().StratusStarburst > 0)
            {
                owner.Calamity().StratusStarburst--;
                owner.Calamity().StratusStarburstResetTimer = (int)MathHelper.Max(
                    owner.Calamity().StratusStarburstResetTimer, 240);
            }
            _pulseFade = MathHelper.Max(_pulseFade - 1f / PulseInterval, 0f);

            if (++_pulseTimer >= PulseInterval)
            {
                _pulseTimer = 0;
                _pulseFade = 1f;

                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player p = Main.player[i];
                    if (!p.active || p.dead) continue;
                    if (Vector2.Distance(p.Center, Projectile.Center) <= RegenRadius)
                        Projectile.ThoriumHeal(3, radius: 1000, specificPlayer: p.whoAmI);
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;
                    if (Vector2.Distance(npc.Center, Projectile.Center) <= FrostRadius)
                        npc.AddBuff(ModContent.BuffType<Voidfrost>(), 120);
                }
            }

            Lighting.AddLight(Projectile.Center, 0.1f, 0.25f, 0.5f);
        }

        private void BuildSegments()
        {
            const float Scale = 120f;
            Segments.Add(new ConstellationSegment(Projectile.Center + new Vector2(-Scale, Scale * 0.5f)));
            Segments.Add(new ConstellationSegment(Projectile.Center + new Vector2(0f, -Scale)));
            Segments.Add(new ConstellationSegment(Projectile.Center + new Vector2(Scale, Scale * 0.5f)));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Segments.Count < 3) return false;

            float spawnFade = SpawnProgress;
            float deathFade = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);

            float twinkle = _spawnTimer < SpawnTime
                ? 0.6f + 0.4f * System.MathF.Sin(Main.GlobalTimeWrappedHourly * 12f + Projectile.whoAmI)
                : 1f;

            float totalFade = spawnFade * deathFade * twinkle;


            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive,
                SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer,
                null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Texture2D glowTex = GetGlowTex();

            Color connectionColor = new Color(120, 200, 255)
                * ((System.MathF.Sin(Main.GlobalTimeWrappedHourly) + 1f) * 0.125f + 0.75f)
                * totalFade;
            Color starColor = new Color(160, 220, 255) * totalFade;

            CalamityUtils.DrawLineBetter(Main.spriteBatch, Segments[0].Center, Segments[1].Center, connectionColor, 2f);
            CalamityUtils.DrawLineBetter(Main.spriteBatch, Segments[1].Center, Segments[2].Center, connectionColor, 2f);

            foreach (var seg in Segments)
            {
                Main.spriteBatch.Draw(glowTex, seg.Center - Main.screenPosition,
                    null, starColor, 0f, glowTex.Size() * 0.5f,
                    0.2f * totalFade, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(tex, seg.Center - Main.screenPosition,
                    null, Color.White * totalFade, 0f, tex.Size() * 0.5f,
                    0.75f * totalFade, SpriteEffects.None, 0f);
            }

            if (_pulseFade > 0f)
            {
                float ringExpand = 1f - _pulseFade;

                Main.spriteBatch.EnterShaderRegion();
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].UseOpacity(0.1f * _pulseFade);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].UseSaturation(0.1f);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].SetShaderTexture(
                    ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Neurons"), 1);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].Apply();
                Texture2D ringTex = ModContent.Request<Texture2D>("CalamityMod/Particles/HigResThinCircle").Value;
                Main.EntitySpriteDraw(ringTex, drawPos, null,
                    new Color(120, 200, 255) * _pulseFade * 0.75f,
                    0f, ringTex.Size() * 0.5f,
                    RegenRadius * 2f * ringExpand / ringTex.Width,
                    SpriteEffects.None, 0);
                Main.spriteBatch.ExitShaderRegion();

                Main.spriteBatch.EnterShaderRegion();
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].UseOpacity(0.5f * _pulseFade);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].UseSaturation(0.2f);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].SetShaderTexture(
                    ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/MeltyNoiseHighContrast"), 1);
                GameShaders.Misc["CalamityMod:OtherworldBarrierDistortion"].Apply();
                Texture2D bloomTex = ModContent.Request<Texture2D>("CalamityMod/Particles/BloomCircle").Value;
                Main.EntitySpriteDraw(bloomTex, drawPos, null,
                    new Color(100, 180, 255) * _pulseFade * 0.35f,
                    0f, bloomTex.Size() * 0.5f,
                    RegenRadius * 2f * ringExpand / bloomTex.Width,
                    SpriteEffects.None, 0);
                Main.spriteBatch.ExitShaderRegion();
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer,
                null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}