using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using RagnarokMod.Projectiles;
using CalamityMod.Items.Materials;

namespace RagnarokMod.Items.BardItems;
public class DrumStick : BardItem
{
    public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

    public override void SetStaticDefaults()
    {
        Empowerments.AddInfo<MovementSpeed>(2);
    }

    public override void SetBardDefaults()
    {
        this.Item.damage = 200;
        InspirationCost = 1;
        this.Item.width = 25;
        this.Item.height = 30;
        this.Item.useTime = 20;
        this.Item.useAnimation = 20;
        this.Item.autoReuse = true;
        this.Item.useStyle = ItemUseStyleID.Swing;
        this.Item.noMelee = false;
        this.Item.knockBack = 4f;
        this.Item.value = Item.sellPrice(0, 10, 0, 0);
        this.Item.rare = ItemRarityID.Red;
        this.Item.UseSound = Sounds.RagnarokModSounds.bonk;
        this.Item.shoot = ModContent.ProjectileType<NoProj>();
        this.Item.shootSpeed = 10f;
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