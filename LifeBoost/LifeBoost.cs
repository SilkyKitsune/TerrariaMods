using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LifeBoost;

internal sealed class LifeBoostPlayer : ModPlayer
{
    internal const int
        BaseLifeAmount = 200, LifeCrystalAmount = 50, MaxLifeCrystals = 12,
        LifeFruitAmount = 20, MaxLifeFruits = 10,
        BaseManaAmount = 50, ManaCrystalAmount = 50, MaxManaCrystals = 10;

    internal const float AttackMultiplier = 2f, DefenseMultiplier = 3f, SpeedMultiplier = 10f;

    internal int lifeCrystalsUsed = 0, lifeFruitsUsed = 0, manaCrystalsUsed = 0;

    internal float lifeMultiplier = 1f, manaMultiplier = 1f;

    public override void LoadData(TagCompound tag)
    {
        tag.TryGet(nameof(lifeCrystalsUsed), out lifeCrystalsUsed);
        tag.TryGet(nameof(lifeFruitsUsed),   out lifeFruitsUsed);
        tag.TryGet(nameof(manaCrystalsUsed), out manaCrystalsUsed);
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => modifiers.FinalDamage *= AttackMultiplier;

    public override void PostUpdateBuffs()
    {
        Player player = Player;

        player.statLifeMax2 +=
            (int)(((BaseLifeAmount < 1 ? 1 : BaseLifeAmount) - 100 +
            (lifeCrystalsUsed * LifeCrystalAmount) +
            (lifeFruitsUsed * LifeFruitAmount)) * lifeMultiplier);

        player.statManaMax2 +=
            (int)(((BaseManaAmount < 0 ? 0 : BaseManaAmount) - 20 +
            (manaCrystalsUsed * ManaCrystalAmount)) * manaMultiplier);

        player.statDefense *= DefenseMultiplier;

        player.moveSpeed *= SpeedMultiplier;
    }

    public override void PreUpdateBuffs() => lifeMultiplier = manaMultiplier = 1f;

    public override void SaveData(TagCompound tag)
    {
        if (lifeCrystalsUsed > 0) tag.Add(nameof(lifeCrystalsUsed), lifeCrystalsUsed);
        if (lifeFruitsUsed > 0)   tag.Add(nameof(lifeFruitsUsed),   lifeFruitsUsed);
        if (manaCrystalsUsed > 0) tag.Add(nameof(manaCrystalsUsed), manaCrystalsUsed);
    }
}