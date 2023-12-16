using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles;

namespace RagnarokMod.Items;
public class BlazingDrumBeat : BardItem
{
    public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

    public override void SetStaticDefaults()
    {
        Empowerments.AddInfo<MovementSpeed>(4);
    }

    public override void SetBardDefaults()
    {
        ((ModItem)this).Item.damage = 650;
        base.InspirationCost = 4;
        ((Entity)((ModItem)this).Item).width = 25;
        ((Entity)((ModItem)this).Item).height = 30;
        ((ModItem)this).Item.useTime = 40;
        ((ModItem)this).Item.useAnimation = 40;
        ((ModItem)this).Item.autoReuse = true;
        ((ModItem)this).Item.useStyle = ItemUseStyleID.Swing;
        ((ModItem)this).Item.noMelee = false;
        ((ModItem)this).Item.knockBack = 4f;
        ((ModItem)this).Item.value = Item.sellPrice(0, 10, 0, 0);
        ((ModItem)this).Item.rare = ItemRarityID.Red;
        ((ModItem)this).Item.UseSound = Sounds.RagnarokModSounds.bonk;
        ((ModItem)this).Item.shoot = ModContent.ProjectileType<BlazingDrumBeatFireSlash>();
        ((ModItem)this).Item.shootSpeed = 10f;
        
    }
    public override void AddRecipes()
    {
        CreateRecipe().
        AddIngredient<DrumStick>().
        AddIngredient<UelibloomBar>(8).
        AddTile(TileID.LunarCraftingStation).
        Register();
    }
}