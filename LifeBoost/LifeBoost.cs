using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LifeBoost;

internal sealed class LifeBoostPlayer : ModPlayer
{
    internal int lifeCrystalsUsed = 0, lifeFruitsUsed = 0, manaCrystalsUsed = 0;

    internal float lifeMultiplier = 1f, manaMultiplier = 1f;

    public override void LoadData(TagCompound tag)
    {
        tag.TryGet(nameof(lifeCrystalsUsed), out lifeCrystalsUsed);
        tag.TryGet(nameof(lifeFruitsUsed),   out lifeFruitsUsed);
        tag.TryGet(nameof(manaCrystalsUsed), out manaCrystalsUsed);
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) =>
        modifiers.FinalDamage *= ModContent.GetInstance<LifeBoostConfig>().attackMultiplier / 100f;

    public override void PostUpdateBuffs()
    {
        Player player = Player;
        LifeBoostConfig config = ModContent.GetInstance<LifeBoostConfig>();

        player.statLifeMax2 +=
            (int)(((config.baseLifeAmount < 1 ? 1 : config.baseLifeAmount) - 100 +
            (lifeCrystalsUsed * config.lifeCrystalAmount) +
            (lifeFruitsUsed * config.lifeFruitAmount)) * lifeMultiplier);

        player.statManaMax2 +=
            (int)(((config.baseManaAmount < 0 ? 0 : config.baseManaAmount) - 20 +
            (manaCrystalsUsed * config.manaCrystalAmount)) * manaMultiplier);

        player.statDefense *= config.defenseMultiplier / 100f;

        player.moveSpeed *= config.speedMultiplier / 100f;
    }

    public override void PreUpdateBuffs() => lifeMultiplier = manaMultiplier = 1f;

    public override void SaveData(TagCompound tag)
    {
        if (lifeCrystalsUsed > 0) tag.Add(nameof(lifeCrystalsUsed), lifeCrystalsUsed);
        if (lifeFruitsUsed > 0)   tag.Add(nameof(lifeFruitsUsed),   lifeFruitsUsed);
        if (manaCrystalsUsed > 0) tag.Add(nameof(manaCrystalsUsed), manaCrystalsUsed);
    }
}