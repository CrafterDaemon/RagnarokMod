using CalamityMod;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Riffs
{
    public abstract class Riff : ModType
    {
        public byte RiffType { get; internal set; }
        public virtual float Range => 1000f;
        public virtual int CooldownTicks => 300;

        public EmpowermentPool Empowerments { get; } = new EmpowermentPool();

        protected override void Register() => RiffLoader.AddRiff(this);
        public EmpowermentList TooltipList { get; private set; }

        public override void SetupContent()
        {
            TooltipList = EmpowermentList.CreateList(0);
            SetStaticDefaults();
        }

        public void OnStart(Player bardPlayer)
        {
            bardPlayer.AddCooldown(RiffLoader.Cooldown.ID, CooldownTicks);
            SafeOnStart(bardPlayer);
        }

        public void OnEnd(Player bardPlayer)
        {
            SafeOnEnd(bardPlayer);
        }

        // Override this in subclasses to point to the nested Cooldown class

        public void Update(Player bardPlayer, Player target)
        {
            SafeUpdate(bardPlayer, target);
        }

        public virtual void SafeOnStart(Player bardPlayer) { }
        public virtual void SafeOnEnd(Player bardPlayer) { }
        public virtual void SafeUpdate(Player bardPlayer, Player target) { }
    }
}