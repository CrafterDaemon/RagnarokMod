using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items.Materials;
using ThoriumMod.Items.BardItems;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles;
using ThoriumMod.Utilities;
using RagnarokMod.Projectiles.BardPro.Percussion;

namespace RagnarokMod.Items.BardItems.Percussion;
public class BlazingDrumBeat : BigInstrumentItemBase
{
    //flame slash go whoosh
    public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

    public override void SafeSetStaticDefaults()
    {
        Empowerments.AddInfo<MovementSpeed>(4);
    }

    public override void SafeSetBardDefaults()
    {
        Item.damage = 300;
        InspirationCost = 0;
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
        Item.shoot = ModContent.ProjectileType<BlazingDrumBeatFireSlash>();
        Item.shootSpeed = 10f;

    }
    public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        ThoriumPlayer thoriumplayer = player.GetThoriumPlayer();
        float bardHit = 1f + success * 0.02f;
        if (success != 0)
        {
            Projectile.NewProjectile(source, position, velocity * bardHit, type, (int)(damage * 1.3f), knockback, player.whoAmI, level - 1, 0f, 0f);
            thoriumplayer.bardResource += 4;
        }
        else
        {
            Projectile.NewProjectile(source, position, velocity * bardHit, ModContent.ProjectileType<NoProj>(), damage, knockback, player.whoAmI, level - 1, 0f, 0f);
            thoriumplayer.bardResource -= 4;
        }

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