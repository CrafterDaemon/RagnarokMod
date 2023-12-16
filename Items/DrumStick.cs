using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using RagnarokMod.Projectiles;
using CalamityMod.Items.Materials;

namespace RagnarokMod.Items;
public class DrumStick : BardItem
{
    public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

    public override void SetStaticDefaults()
    {
        Empowerments.AddInfo<MovementSpeed>(2);
    }

    public override void SetBardDefaults()
    {
        ((ModItem)this).Item.damage = 200;
        base.InspirationCost = 1;
        ((Entity)((ModItem)this).Item).width = 25;
        ((Entity)((ModItem)this).Item).height = 30;
        ((ModItem)this).Item.useTime = 20;
        ((ModItem)this).Item.useAnimation = 20;
        ((ModItem)this).Item.autoReuse = true;
        ((ModItem)this).Item.useStyle = ItemUseStyleID.Swing;
        ((ModItem)this).Item.noMelee = false;
        ((ModItem)this).Item.knockBack = 4f;
        ((ModItem)this).Item.value = Item.sellPrice(0, 10, 0, 0);
        ((ModItem)this).Item.rare = ItemRarityID.Red;
        ((ModItem)this).Item.UseSound = Sounds.RagnarokModSounds.bonk;
        ((ModItem)this).Item.shoot = ModContent.ProjectileType<NoProj>();
        ((ModItem)this).Item.shootSpeed = 10f;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
        AddIngredient(ItemID.Wood,32).
        AddIngredient<PerennialBar>(8).
        AddTile(TileID.MythrilAnvil).
        Register();
    }
}