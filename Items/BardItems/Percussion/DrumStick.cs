using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using RagnarokMod.Projectiles;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using Terraria.Audio;
using RagnarokMod.Items.BardItems.Percussion;
using Microsoft.Xna.Framework;

namespace RagnarokMod.Items.BardItems.Percussion;

public class DrumStick : BardItem
{
    //bonk bonk bonk bonk
    public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

    public override void SetStaticDefaults()
    {
        Empowerments.AddInfo<MovementSpeed>(2);
    }

    public override void SetBardDefaults()
    {
        Item.damage = 100;
        InspirationCost = 1;
        Item.width = 25;
        Item.height = 30;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.autoReuse = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = false;
        Item.knockBack = 4f;
        Item.value = CalamityGlobalItem.RarityLimeBuyPrice;
        Item.rare = ItemRarityID.Lime;
        Item.UseSound = SoundID.Item1;
        Item.shoot = ModContent.ProjectileType<NoProj>();
        Item.shootSpeed = 10f;
    }

    public void TriggerMeleeHit(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        SoundEngine.PlaySound(Sounds.RagnarokModSounds.bonk, target.Center);

        // Try Thorium projectile
        if (!ModLoader.TryGetMod("ThoriumMod", out Mod thorium))
            return;

        int projType = thorium.Find<ModProjectile>("DrumMalletPro3")?.Type ?? -1;
        if (projType < 0)
            return;

        Projectile.NewProjectile(
            player.GetSource_OnHit(target),
            target.Center,
            Vector2.Zero,
            projType,
            damageDone,
            hit.Knockback,
            player.whoAmI
        );
    }

    public override void AddRecipes()
    {
        CreateRecipe().
        AddIngredient<DrumMallet>(1).
        AddIngredient<PerennialBar>(8).
        AddTile(TileID.MythrilAnvil).
        Register();
    }
}

public class BardMeleeHitPlayer : ModPlayer
{
    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (item.type == ModContent.ItemType<DrumStick>())
        {
            // Cast the ModItem to your DrumStick class
            if (item.ModItem is DrumStick drum)
            {
                drum.TriggerMeleeHit(Player, target, hit, damageDone);
            }
        }
    }
}