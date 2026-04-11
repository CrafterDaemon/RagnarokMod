using CalamityMod.Buffs.StatBuffs;
using CalamityMod.CalPlayer;
using CalamityMod;
using CalamityMod.Items.Placeables.Furniture.Paintings;
using RagnarokMod.Items.Placeables.Paintings;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using RagnarokMod.Balancing;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class RagnarokGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool debuffNightfallen;

        public override void ResetEffects(NPC npc)
        {
            debuffNightfallen = false;
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.boss)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead)
                    {
                        player.GetRagnarokModPlayer().bloodflarebloodlust = 0;
                    }
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (debuffNightfallen)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                    npc.lifeRegen -= 200;
                }
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            OnHitAny(npc, player, hit, damageDone);
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            OnHitAny(npc, player, hit, damageDone);
        }

        private void OnHitAny(NPC npc, Player player, NPC.HitInfo hit, int damageDone)
        {
            if (debuffNightfallen)
            {
                hit.Damage = (int)(hit.Damage * 1.1f);
            }
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            BalancingChangesManager.ApplyFromProjectile(npc, ref modifiers, projectile);
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.boss && npc.type != NPCID.TorchGod) //anything that is considered a boss from (except Torch God lol) will have a 1/100 chance to drop Ragnarok's painting directly
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FateoftheGods>(), ThankYouPainting.DropInt));
        }
    }
}
