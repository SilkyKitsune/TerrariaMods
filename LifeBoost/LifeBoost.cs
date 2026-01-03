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

    internal int extraLife = 100;
    internal int crystalsUsed = 0;
    internal int fruitsUsed = 0;

    public override void LoadData(TagCompound tag)
    {
        crystalsUsed = tag.TryGet(nameof(crystalsUsed), out int crystalValue) ? crystalValue : 0;
        fruitsUsed = tag.TryGet(nameof(fruitsUsed), out int fruitValue) ? fruitValue : 0;
    }


    public override void SaveData(TagCompound tag)
    {
        if (crystalsUsed > 0) tag.Add(nameof(crystalsUsed), crystalsUsed);
        if (fruitsUsed > 0) tag.Add(nameof(fruitsUsed), fruitsUsed);
    }
}