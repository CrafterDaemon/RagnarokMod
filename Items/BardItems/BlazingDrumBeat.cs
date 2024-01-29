using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.BardPro;

namespace RagnarokMod.Items.BardItems;
public class BlazingDrumBeat : BardItem
{
    //flame slash go whoosh
    public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

    public override void SetStaticDefaults()
    {
        Empowerments.AddInfo<MovementSpeed>(4);
    }

    public override void SetBardDefaults()
    {
        Item.damage = 650;
        InspirationCost = 4;
        Item.width = 25;
        Item.height = 30;
        Item.useTime = 40;
        Item.useAnimation = 40;
        Item.autoReuse = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = false;
        Item.knockBack = 4f;
        Item.value = Item.sellPrice(0, 10, 0, 0);
        Item.rare = ItemRarityID.Red;
        Item.UseSound = Sounds.RagnarokModSounds.bonk;
        Item.shoot = ModContent.ProjectileType<BlazingDrumBeatFireSlash>();
        Item.shootSpeed = 10f;

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