using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Sounds;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Items.TestingOnly
{
    public class MaxEmpowermentTester : BardItem
    {
        public override string Texture => "ThoriumMod/Items/ZRemoved/TesterEmpowerment";
        public override void SetStaticDefaults()
        {
            Empowerments.AddInfo<FlatDamage>(10);
            Empowerments.AddInfo<Damage>(10);
            Empowerments.AddInfo<AttackSpeed>(10);
            Empowerments.AddInfo<CriticalStrikeChance>(10);
            Empowerments.AddInfo<JumpHeight>(10);
            Empowerments.AddInfo<MovementSpeed>(10);
            Empowerments.AddInfo<FlightTime>(10);
            Empowerments.AddInfo<AquaticAbility>(10);
            Empowerments.AddInfo<InvincibilityFrames>(10);
            Empowerments.AddInfo<Defense>(10);
            Empowerments.AddInfo<DamageReduction>(10);
            Empowerments.AddInfo<ResourceRegen>(10);
            Empowerments.AddInfo<ResourceMaximum>(10);
            Empowerments.AddInfo<ResourceConsumptionChance>(10);
            Empowerments.AddInfo<ResourceGrabRange>(10);
            Empowerments.AddInfo<LifeRegeneration>(10);
            Empowerments.AddInfo<EmpowermentProlongation>(10);
            Item.ResearchUnlockCount = 0;
        }
        public override void SetBardDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.autoReuse = true;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = -1;
            Item.UseSound = new SoundStyle?(ThoriumSounds.String_Sound);
            Item.shoot = ModContent.ProjectileType<AcousticWave>();
            Item.shootSpeed = 5f;
        }
    }
}
