using CalamityMod;
using CalamityMod.Cooldowns;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Riffs;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class GlacialHarvester : ScytheItem
    {
        public class GlacialSlamCooldown : CooldownHandler
        {
            public static new string ID => "GlacialSlamCooldown";
            public override bool ShouldDisplay => true;
            public override bool SavedWithPlayer => true;
            public override bool PersistsThroughDeath => true;
            public override LocalizedText DisplayName => Language.GetText("Mods.RagnarokMod.Items.HealerItems.Scythes.GlacialHarvester.Cooldown");
            public override string Texture => "RagnarokMod/Items/HealerItems/Scythes/GlacialHarvester";
            public override Color OutlineColor => Color.LightSkyBlue;
            public override Color CooldownStartColor => Color.LightCyan;
            public override Color CooldownEndColor => Color.LightSteelBlue;

            public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
                Texture2D value3 = ModContent.Request<Texture2D>(ChargeBarTexture).Value;
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
                ApplyBarShaders(opacity);
                spriteBatch.Draw(value3, position, null, Color.White * opacity, 0f, value3.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                spriteBatch.Draw(value2, position, null, OutlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale/2, SpriteEffects.None, 0f);
            }

            public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
            {
                Texture2D value = ModContent.Request<Texture2D>(Texture).Value;
                Texture2D value2 = ModContent.Request<Texture2D>(OutlineTexture).Value;
                Texture2D value3 = ModContent.Request<Texture2D>(OverlayTexture).Value;
                Color outlineColor = OutlineColor;
                spriteBatch.Draw(value2, position, null, outlineColor * opacity, 0f, value2.Size() * 0.5f, scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(value, position, null, Color.White * opacity, 0f, value.Size() * 0.5f, scale/2, SpriteEffects.None, 0f);
                int num = (int)Math.Ceiling((float)value3.Height * (1f - instance.Completion));
                spriteBatch.Draw(sourceRectangle: new Rectangle(0, num, value3.Width, value3.Height - num), texture: value3, position: position + Vector2.UnitY * num * scale, color: outlineColor * opacity * 0.9f, rotation: 0f, origin: value.Size() * 0.25f, scale: scale, effects: SpriteEffects.None, layerDepth: 0f);
            }

        }
        public int SlamCooldown = 300;
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 35;
            scytheSoulCharge = 2;
            base.Item.width = 54;
            base.Item.height = 54;
            base.Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            base.Item.rare = ItemRarityID.Pink;
            base.Item.shoot = ModContent.ProjectileType<GlacialHarvesterPro>();
            base.Item.shootSpeed = 0f;
            base.Item.channel = true; // hold to channel
            Item.knockBack = 4f;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                var ragnarokPlayer = player.GetRagnarokModPlayer();

                if (player.Calamity().cooldowns.ContainsKey(GlacialSlamCooldown.ID))
                    return false;
                player.AddCooldown(GlacialSlamCooldown.ID, SlamCooldown);
                Item.useTime = 16;
                Item.useAnimation = 16;
                Item.channel = false;
                Item.noMelee = true;
            }
            else
            {
                Item.useTime = 30;
                Item.useAnimation = 30;
                Item.channel = true;
                Item.noMelee = false;
            }
            return base.CanUseItem(player);
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                // Only one charge swing at a time
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.owner == player.whoAmI
                        && p.type == ModContent.ProjectileType<GlacialSlam>())
                        return false;
                }
                Projectile.NewProjectile(source, position, velocity,
                    ModContent.ProjectileType<GlacialSlam>(),
                    damage * 3, knockback * 4f, player.whoAmI);
                return false;
            }

            // Left-click: only spawn the scythe if one isn't already active
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == type && p.owner == player.whoAmI)
                    return false;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }
    }
}