using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Effects;   // <— added for Filters.Scene
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class GeminiConstellation : ModProjectile, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Particles/Sparkle";

        private const int StarburstDrainInterval = 12;
        private const int FireInterval = 12;
        private const float FireRange = 800f;

        private const int SpawnTime = 60;
        private int _spawnTimer = 0;
        private float SpawnProgress => MathHelper.Clamp(_spawnTimer / (float)SpawnTime, 0f, 1f);

        private int _fireTimer = 0;
        private int _nextStarIndex = 0;
        private float _pulseFade = 0f;

        // Beam state
        private Vector2 _beamStarPos;
        private Vector2 _beamTargetPos;
        private float _beamFade = 0f;
        private const float BeamFadeDuration = 28f;

        // Star pulse ring state
        private int _lastFiredStarIndex = -1;
        private float _ringFade = 0f;
        private float _ringRadius = 0f;
        private const float RingMaxRadius = 55f;

        private Vector2 _beamStarPos2;
        private Vector2 _beamTargetPos2;
        private float _beamFade2 = 0f;
        private int _lastFiredStarIndex2 = -1;
        private float _ringFade2 = 0f;
        private float _ringRadius2 = 0f;

        private int _fireCount = 0;

        public List<Vector2> Stars = new();

        private static readonly (int, int)[] Connections = new[]
        {
            (0, 1), (1, 2), (1, 3), (1, 5), (5, 6), (6, 7), (7, 8), (5, 9),
            (3, 4), (4, 10), (4, 11), (4, 12), (12, 13), (13, 15), (12, 14), (14, 16)
        };

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
            BuildStars();
        }

        private void BuildStars()
        {
            Vector2 center = Projectile.Center;
            Stars.Add(center + new Vector2(0, -160));
            Stars.Add(center + new Vector2(-60, -100));
            Stars.Add(center + new Vector2(-120, -150));
            Stars.Add(center + new Vector2(-100, -75));
            Stars.Add(center + new Vector2(-160, -70));
            Stars.Add(center + new Vector2(20, 0));
            Stars.Add(center + new Vector2(80, 30));
            Stars.Add(center + new Vector2(110, 30));
            Stars.Add(center + new Vector2(140, 15));
            Stars.Add(center + new Vector2(60, 100));
            Stars.Add(center + new Vector2(-200, -120));
            Stars.Add(center + new Vector2(-210, 0));
            Stars.Add(center + new Vector2(-100, 50));
            Stars.Add(center + new Vector2(-20, 80));
            Stars.Add(center + new Vector2(-100, 120));
            Stars.Add(center + new Vector2(40, 150));
            Stars.Add(center + new Vector2(0, 200));
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.velocity = Vector2.Zero;

            // Beam and ring decay
            if (_beamFade > 0f)
                _beamFade = MathHelper.Max(_beamFade - 1f / BeamFadeDuration, 0f);
            if (_ringFade > 0f)
            {
                _ringFade = MathHelper.Max(_ringFade - 1f / BeamFadeDuration, 0f);
                _ringRadius = RingMaxRadius * (1f - _ringFade);
            }
            if (_beamFade2 > 0f)
                _beamFade2 = MathHelper.Max(_beamFade2 - 1f / BeamFadeDuration, 0f);
            if (_ringFade2 > 0f)
            {
                _ringFade2 = MathHelper.Max(_ringFade2 - 1f / BeamFadeDuration, 0f);
                _ringRadius2 = RingMaxRadius * (1f - _ringFade2);
            }

            if (_spawnTimer < SpawnTime)
            {
                _spawnTimer++;
                if (Main.rand.NextBool(3))
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    float dist = Main.rand.NextFloat(20f, 100f) * SpawnProgress;
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
            _pulseFade = MathHelper.Max(_pulseFade - 1f / FireInterval, 0f);

            bool dismissed = Projectile.ai[0] == 1f;
            if (dismissed)
            {
                if (Projectile.timeLeft > 60) Projectile.timeLeft = 60;
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

            _fireTimer++;
            if (_fireTimer >= (FireInterval) && Stars.Count > 0)
            {
                _fireTimer = 0;
                FireFromStar(_nextStarIndex);
                _nextStarIndex = Main.rand.Next(Stars.Count);
                FireFromStar(_nextStarIndex);
                _nextStarIndex = Main.rand.Next(Stars.Count);

            }

            Lighting.AddLight(Projectile.Center, 0.1f, 0.25f, 0.5f);
        }

        private void FireFromStar(int starIndex)
        {
            if (starIndex < 0 || starIndex >= Stars.Count) return;
            if (Main.myPlayer != Projectile.owner) return;

            Vector2 starPos = Stars[starIndex];

            NPC closest = null;
            float closestDist = FireRange;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage || npc.lifeMax <= 5) continue;
                float dist = Vector2.Distance(starPos, npc.Center);
                if (dist < closestDist) { closestDist = dist; closest = npc; }
            }

            if (closest == null) return;

            Vector2 direction = Vector2.Normalize(closest.Center - starPos);

            // Alternate between beam slot 1 and 2
            if (_fireCount % 2 == 0)
            {
                _beamStarPos = starPos;
                _beamTargetPos = closest.Center;
                _beamFade = 1f;
                _lastFiredStarIndex = starIndex;
                _ringFade = 1f;
                _ringRadius = 0f;
            }
            else
            {
                _beamStarPos2 = starPos;
                _beamTargetPos2 = closest.Center;
                _beamFade2 = 1f;
                _lastFiredStarIndex2 = starIndex;
                _ringFade2 = 1f;
                _ringRadius2 = 0f;
            }
            _fireCount++;
            _pulseFade = 0.8f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                starPos, direction * 42f,
                ModContent.ProjectileType<GeminiMoonlightRay>(),
                Projectile.damage/4, Projectile.knockBack,
                Projectile.owner, closest.whoAmI);

            SoundEngine.PlaySound(SoundID.Item84 with { Volume = 0.45f, Pitch = 0.5f }, starPos);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Stars.Count; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Dust d = Dust.NewDustPerfect(Stars[i], DustID.BlueFairy,
                        Main.rand.NextVector2Circular(4f, 4f), 0, default, Main.rand.NextFloat(1f, 2f));
                    d.noGravity = true;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float spawnFade = SpawnProgress;
            float deathFade = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);
            float twinkle = _spawnTimer < SpawnTime
                ? 0.6f + 0.4f * MathF.Sin(Main.GlobalTimeWrappedHourly * 12f + Projectile.whoAmI)
                : 1f;
            float totalFade = spawnFade * deathFade * twinkle;

            SpriteBatch sb = Main.spriteBatch;
            Texture2D invis = ModContent.Request<Texture2D>("CalamityMod/Projectiles/InvisibleProj").Value;

            // plz no crash :(
            sb.End();

            if (_beamFade > 0f)
            {
                float beamAngle = (_beamTargetPos - _beamStarPos).ToRotation();
                float fadeEased = _beamFade * _beamFade;

                Effect effect = Filters.Scene["CalamityMod:SpreadTelegraph"].GetShader().Shader;
                effect.Parameters["centerOpacity"].SetValue(0.9f * fadeEased);
                effect.Parameters["mainOpacity"].SetValue(fadeEased);
                effect.Parameters["halfSpreadAngle"].SetValue(MathHelper.ToRadians(0.5f));
                effect.Parameters["edgeColor"].SetValue(new Color(80, 160, 255).ToVector3());
                effect.Parameters["centerColor"].SetValue(Color.White.ToVector3());
                effect.Parameters["edgeBlendLength"].SetValue(0.055f);
                effect.Parameters["edgeBlendStrength"].SetValue(11f);

                sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState,
                    DepthStencilState.None, Main.Rasterizer, effect,
                    Main.GameViewMatrix.TransformationMatrix);

                float beamLength = Vector2.Distance(_beamStarPos, _beamTargetPos) * 9f;
                Main.EntitySpriteDraw(invis, _beamStarPos - Main.screenPosition, null, Color.White,
                    beamAngle, new Vector2(invis.Width * 0.5f, invis.Height * 0.5f), beamLength, 0, 0);

                sb.End();
            }

            if (_beamFade2 > 0f)
            {
                float beamAngle2 = (_beamTargetPos2 - _beamStarPos2).ToRotation();
                float fadeEased2 = _beamFade2 * _beamFade2;

                Effect effect = Filters.Scene["CalamityMod:SpreadTelegraph"].GetShader().Shader;
                effect.Parameters["centerOpacity"].SetValue(0.9f * fadeEased2);
                effect.Parameters["mainOpacity"].SetValue(fadeEased2);
                effect.Parameters["halfSpreadAngle"].SetValue(MathHelper.ToRadians(0.5f));
                effect.Parameters["edgeColor"].SetValue(new Color(80, 160, 255).ToVector3());
                effect.Parameters["centerColor"].SetValue(Color.White.ToVector3());
                effect.Parameters["edgeBlendLength"].SetValue(0.055f);
                effect.Parameters["edgeBlendStrength"].SetValue(11f);

                sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState,
                    DepthStencilState.None, Main.Rasterizer, effect,
                    Main.GameViewMatrix.TransformationMatrix);

                float beamLength2 = Vector2.Distance(_beamStarPos2, _beamTargetPos2) * 9f;
                Main.EntitySpriteDraw(invis, _beamStarPos2 - Main.screenPosition, null, Color.White,
                    beamAngle2, new Vector2(invis.Width * 0.5f, invis.Height * 0.5f), beamLength2, 0, 0);

                sb.End();
            }

            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp,
                DepthStencilState.None, Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            Texture2D tex = TextureAssets.Projectile[Type].Value;

            Color connectionColor = new Color(120, 200, 255)
                * ((MathF.Sin(Main.GlobalTimeWrappedHourly) + 1f) * 0.125f + 0.75f)
                * totalFade;

            foreach (var (a, b) in Connections)
                CalamityUtils.DrawLineBetter(sb, Stars[a], Stars[b], connectionColor, 2f);

            for (int i = 0; i < Stars.Count; i++)
            {
                float phase = i * MathHelper.TwoPi / Stars.Count;
                float starTwinkle = 0.8f + 0.2f * MathF.Sin(Main.GlobalTimeWrappedHourly * 6f + phase);
                sb.Draw(tex, Stars[i] - Main.screenPosition, null,
                    Color.White * totalFade * starTwinkle, 0f, tex.Size() * 0.5f,
                    0.75f * totalFade * starTwinkle, SpriteEffects.None, 0f);
            }

            if (_ringFade > 0f && _lastFiredStarIndex >= 0 && _lastFiredStarIndex < Stars.Count)
            {
                Vector2 ringCenter = Stars[_lastFiredStarIndex];
                Color ringColor = new Color(160, 220, 255) * (_ringFade * _ringFade * totalFade);
                const int Segments = 24;
                for (int s = 0; s < Segments; s++)
                {
                    float a0 = s * MathHelper.TwoPi / Segments;
                    float a1 = (s + 1) * MathHelper.TwoPi / Segments;
                    CalamityUtils.DrawLineBetter(sb,
                        ringCenter + a0.ToRotationVector2() * _ringRadius,
                        ringCenter + a1.ToRotationVector2() * _ringRadius,
                        ringColor, 1.5f * _ringFade);
                }
            }

            if (_ringFade2 > 0f && _lastFiredStarIndex2 >= 0 && _lastFiredStarIndex2 < Stars.Count)
            {
                Vector2 ringCenter2 = Stars[_lastFiredStarIndex2];
                Color ringColor2 = new Color(160, 220, 255) * (_ringFade2 * _ringFade2 * totalFade);
                const int Segments = 24;
                for (int s = 0; s < Segments; s++)
                {
                    float a0 = s * MathHelper.TwoPi / Segments;
                    float a1 = (s + 1) * MathHelper.TwoPi / Segments;
                    CalamityUtils.DrawLineBetter(sb,
                        ringCenter2 + a0.ToRotationVector2() * _ringRadius2,
                        ringCenter2 + a1.ToRotationVector2() * _ringRadius2,
                        ringColor2, 1.5f * _ringFade2);
                }
            }

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}