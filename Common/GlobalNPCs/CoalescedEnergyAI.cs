using Microsoft.Xna.Framework;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using ThoriumMod;
using ThoriumMod.NPCs;
using ThoriumMod.NPCs.BossGraniteEnergyStorm;
using ThoriumMod.Projectiles.Boss;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class CoalescedEnergyAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        private static Mod thorium = ModLoader.GetMod("ThoriumMod");

        public int distance;
        public bool distanceShift;
        public float rot;

        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return npc.type == thorium.Find<ModNPC>("CoalescedEnergy").Type;
        }

        public override bool PreAI(NPC npc)
        {
            if (!(OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().granite)))
            {
                return true;
            }
            if (CalamityGamemodeCheck.isBossrush)
            {
                if (OtherModsCompat.tbr_loaded) // Can be removed as soon as Thorium Rework bossrush is fixed
                {
                    return true;
                }
                if (!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
                {
                    return true;
                }
                if (npc.ai[2] >= 1300f)
                {
                    Player player = Main.player[npc.target];
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        npc.active = false;
                        npc.life = 0;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                    float num = 50f;
                    int num2 = 0;
                    while ((float)num2 < num)
                    {
                        Vector2 vector = Vector2.UnitX * 0f;
                        vector += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.2831855f / num)), default(Vector2)) * new Vector2(10f, 10f);
                        vector = Terraria.Utils.RotatedBy(vector, (double)Terraria.Utils.ToRotation(npc.velocity * 0.1f), default(Vector2));
                        int num3 = Dust.NewDust(npc.Center, 0, 0, DustID.MagicMirror, 0f, 0f, 75, default(Color), 1.25f);
                        Main.dust[num3].noGravity = true;
                        Main.dust[num3].position = npc.Center + vector;
                        Main.dust[num3].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector, Vector2.UnitY) * 4f;
                        num2++;
                    }
                    if (!player.dead)
                    {
                        SoundEngine.PlaySound(SoundID.Item94, new Vector2?(npc.Center), null);
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            IEntitySource source_FromAI = npc.GetSource_FromAI(null);
                            Vector2 vector2 = new Vector2(npc.Center.X, npc.Center.Y - 30f);
                            Vector2 vector3 = player.DirectionFrom(vector2);
                            Projectile.NewProjectile(source_FromAI, vector2, vector3 * 32f, ModContent.ProjectileType<GraniteCharge>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f);
                        }
                    }
                    npc.ai[2] = 0f;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}