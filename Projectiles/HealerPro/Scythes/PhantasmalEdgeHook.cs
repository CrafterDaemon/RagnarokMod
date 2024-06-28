using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ReLogic.Content;
using CalamityMod;
using Terraria.DataStructures;
using Terraria.Audio;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class PhantasmalEdgeHook : ModProjectile, ILocalizedModType
    {
        public Player player => Main.player[Projectile.owner];
        private const string ChainAssetPath = "RagnarokMod/Projectiles/HealerPro/Scythes/PhantasmalEdgeChain";
        private static Asset<Texture2D> chain;
        private enum AIState
        {
            PreLaunch,
            Launching,
            MissedRetract,
            EnemyHitRetract,
            BossHitRetract
        }
        private AIState CurrentAIState
        {
            get => (AIState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }
        public ref float StateTimer => ref Projectile.ai[1];
        private bool enemyhit = false;
        private bool bosshit = false;
        private NPC hitTarget;
        private bool kaboom = false;

        public override void Load()
        {
            chain = ModContent.Request<Texture2D>(ChainAssetPath);
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.timeLeft = 3600;
            Projectile.penetrate = -1;
            Projectile.width = 152;
            Projectile.height = 148;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }
        public override void AI()
        {
            // Kill the projectile if the player dies or gets crowd controlled
            if (!player.active || player.dead || player.noItems || player.CCed || Vector2.Distance(Projectile.Center, player.Center) > 3500f)
            {
                Projectile.Kill();
                return;
            }
            if (Main.myPlayer == Projectile.owner && Main.mapFullscreen)
            {
                Projectile.Kill();
                return;
            }
            Lighting.AddLight(Projectile.Center, 1f, 0.686f, 0.686f);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 5;
            player.itemAnimation = 5;
            Vector2 mCenter = player.MountedCenter;
            float maxDist = 3000f;
            float launchSpeed = 25f;
            float missedPullSpeed = 20f;
            float enemyPullSpeed = 10f;
            float bossPullSpeed = 15f;
            float pullingAcceleration = 5f;

            float healerAtkSpd = player.GetTotalAttackSpeed(ThoriumDamageBase<HealerDamage>.Instance);
            missedPullSpeed *= healerAtkSpd;
            enemyPullSpeed *= healerAtkSpd;
            bossPullSpeed *= healerAtkSpd;
            launchSpeed *= healerAtkSpd;
            pullingAcceleration *= healerAtkSpd;

            switch (CurrentAIState)
            {
                case AIState.PreLaunch:
                    {
                        Vector2 uVTM = mCenter.DirectionTo(Main.MouseWorld).SafeNormalize(Vector2.UnitX * player.direction);
                        Projectile.velocity = uVTM * launchSpeed + player.velocity;
                        CurrentAIState = AIState.Launching;
                        StateTimer = 0f;
                        Projectile.netUpdate = true;
                        break;
                    }
                case AIState.Launching:
                    {
                        if ((Projectile.Distance(mCenter) >= maxDist && !enemyhit && !bosshit) || !player.Calamity().mouseRight)
                        {
                            CurrentAIState = AIState.MissedRetract;
                            StateTimer = 0f;
                            Projectile.netUpdate = true;
                            break;
                        }
                        if (enemyhit)
                        {
                            CurrentAIState = AIState.EnemyHitRetract;
                            StateTimer = 0f;
                            Projectile.netUpdate = true;
                            break;
                        }
                        if (bosshit)
                        {
                            CurrentAIState = AIState.BossHitRetract;
                            StateTimer = 0f;
                            Projectile.netUpdate = true;
                            break;
                        }
                        if (player.direction != -1)
                        {
                            Projectile.rotation = Projectile.DirectionFrom(player.Center).ToRotation() + MathHelper.ToRadians(45f);
                        }
                        else
                        {
                            Projectile.rotation = Projectile.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(45f);
                        }
                        player.ChangeDir((player.Center.X < Projectile.Center.X).ToDirectionInt());
                        StateTimer += 1f;
                        break;
                    }
                case AIState.MissedRetract:
                    {
                        Vector2 uVTP = Projectile.DirectionTo(mCenter).SafeNormalize(Vector2.Zero);
                        if (Projectile.Distance(mCenter) <= missedPullSpeed)
                        {
                            Projectile.Kill();
                            return;
                        }
                        Projectile.velocity = Projectile.velocity.MoveTowards(uVTP * missedPullSpeed, pullingAcceleration);
                        player.ChangeDir((player.Center.X < Projectile.Center.X).ToDirectionInt());
                        break;
                    }
                case AIState.EnemyHitRetract:
                    {
                        kaboom = true;
                        Vector2 uVTP = Projectile.DirectionTo(mCenter).SafeNormalize(Vector2.Zero);
                        if (Projectile.Distance(mCenter) <= 3 * hitTarget.width)
                        {
                            Projectile.Kill();
                            return;
                        }
                        Projectile.velocity = Projectile.velocity.MoveTowards(uVTP * enemyPullSpeed, pullingAcceleration);
                        hitTarget.Center = Projectile.Center;
                        player.ChangeDir((player.Center.X < Projectile.Center.X).ToDirectionInt());
                        break;
                    }
                case AIState.BossHitRetract:
                    {
                        kaboom = true;
                        Projectile.velocity = Vector2.Zero;
                        Projectile.Center = hitTarget.Center;
                        Vector2 uVTP = player.DirectionTo(Projectile.Center).SafeNormalize(Vector2.Zero);
                        if (Projectile.Distance(mCenter) <= 3 * hitTarget.width || (StateTimer > 30 && player.velocity == Vector2.Zero) || Projectile.scale < 0.05)
                        {
                            Projectile.Kill();
                            return;
                        }
                        player.velocity = player.velocity.MoveTowards(uVTP * bossPullSpeed, pullingAcceleration);
                        player.ChangeDir((player.Center.X < Projectile.Center.X).ToDirectionInt());
                        Projectile.scale *= 0.99f;
                        StateTimer++;
                        break;
                    }
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.boss)
            {
                bosshit = true;
            }
            else
            {
                enemyhit = true;
            }
            hitTarget = target;
        }

        public override void OnKill(int timeLeft)
        {
            if (kaboom)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                Projectile.localNPCHitCooldown = -1;
                Projectile.Damage();

                int e;
                for (int i = 0; i < 3; i = e + 1)
                {
                    int firespeck = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 100, default, 1.5f);
                    Main.dust[firespeck].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)Projectile.width / 2f;
                    e = i;
                }
                for (int j = 0; j < 10; j = e + 1)
                {
                    int morefirespec = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 0, default, 2.5f);
                    Main.dust[morefirespec].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)Projectile.width / 2f;
                    Main.dust[morefirespec].noGravity = true;
                    Dust dust = Main.dust[morefirespec];
                    dust.velocity *= 2f;
                    e = j;
                }
                for (int k = 0; k < 5; k = e + 1)
                {
                    int finalfirefleck = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 0, default, 1.5f);
                    Main.dust[finalfirefleck].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy((double)Projectile.velocity.ToRotation(), default) * (float)Projectile.width / 2f;
                    Main.dust[finalfirefleck].noGravity = true;
                    Dust dust = Main.dust[finalfirefleck];
                    dust.velocity *= 2f;
                    e = k;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.spriteDirection = player.direction;
            ChainHandler(ref lightColor);
            lightColor = Color.White;
            return true;
        }
        public void ChainHandler(ref Color lightColor)
        {

            Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);

            // This fixes a vanilla GetPlayerArmPosition bug causing the chain to draw incorrectly when stepping up slopes. The flail itself still draws incorrectly due to another similar bug. This should be removed once the vanilla bug is fixed.
            playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;

            Rectangle? chainSourceRectangle = null;
            // Drippler Crippler customizes sourceRectangle to cycle through sprite frames: sourceRectangle = asset.Frame(1, 6);
            float chainHeightAdjustment = 0f; // Use this to adjust the chain overlap. 

            Vector2 chainOrigin = chainSourceRectangle.HasValue ? (chainSourceRectangle.Value.Size() / 2f) : (chain.Size() / 2f);
            Vector2 chainDrawPosition = Projectile.Center;
            Vector2 vectorFromProjectileToPlayerArms = playerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToPlayerArms = vectorFromProjectileToPlayerArms.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = (chainSourceRectangle.HasValue ? chainSourceRectangle.Value.Height : chain.Height()) + chainHeightAdjustment;
            if (chainSegmentLength == 0)
            {
                chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
            }
            float chainRotation = unitVectorFromProjectileToPlayerArms.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;
            float chainLengthRemainingToDraw = vectorFromProjectileToPlayerArms.Length() + chainSegmentLength / 2f;

            // This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
            while (chainLengthRemainingToDraw > 0f)
            {

                // Here, we draw the chain texture at the coordinates
                Main.spriteBatch.Draw(chain.Value, chainDrawPosition - Main.screenPosition, chainSourceRectangle, Color.LightPink, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

                // chainDrawPosition is advanced along the vector back to the player by the chainSegmentLength
                chainDrawPosition += unitVectorFromProjectileToPlayerArms * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }

            // Add a motion trail when moving forward, like most flails do (don't add trail if already hit a tile)
            if (CurrentAIState == AIState.Launching)
            {
                Texture2D texture = TextureAssets.Projectile[Type].Value;
                Vector2 drawOrigin = Projectile.Size / 2;
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Vector2 pos = Projectile.oldPos[k] + drawOrigin;
                    Color drawColor = Color.White * (1 / ((float)k + 1));
                    SpriteEffects flipped;
                    if (Projectile.oldDirection == 1) { flipped = SpriteEffects.None; }
                    else { flipped = SpriteEffects.FlipHorizontally; }
                    Main.EntitySpriteDraw(texture, pos - Main.screenPosition, null, drawColor, Projectile.oldRot[k], drawOrigin, Projectile.scale, flipped);
                }
            }
        }
    }
}
