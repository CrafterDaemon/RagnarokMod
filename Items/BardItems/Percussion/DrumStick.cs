using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using RagnarokMod.Projectiles;
using CalamityMod.Items.Materials;

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
        Item.damage = 200;
        InspirationCost = 1;
        Item.width = 25;
        Item.height = 30;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.autoReuse = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = false;
        Item.knockBack = 4f;
        Item.value = Item.sellPrice(0, 10, 0, 0);
        Item.rare = ItemRarityID.Red;
        Item.UseSound = Sounds.RagnarokModSounds.bonk;
        Item.shoot = ModContent.ProjectileType<NoProj>();
        Item.shootSpeed = 10f;
    }
    public override void AddRecipes()
    {
        CreateRecipe().
        AddIngredient(ItemID.Wood, 32).
        AddIngredient<PerennialBar>(8).
        AddTile(TileID.MythrilAnvil).
        Register();
    }
}