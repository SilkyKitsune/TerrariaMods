using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LifeBoost;

internal sealed class LifeBoostPlayer : ModPlayer
{
    internal int extraLife = 100;
    internal int crystalsUsed = 0;
    internal int fruitsUsed = 0;

    internal int currentDef = 0;
    internal int currentDefBoost = 0;

    public override void LoadData(TagCompound tag)
    {
        crystalsUsed = tag.TryGet(nameof(crystalsUsed), out int crystalValue) ? crystalValue : 0;
        fruitsUsed = tag.TryGet(nameof(fruitsUsed), out int fruitValue) ? fruitValue : 0;
    }

    public override void PostUpdate() => currentDef = Player.statDefense - currentDefBoost;

    public override void SaveData(TagCompound tag)
    {
        if (crystalsUsed > 0) tag.Add(nameof(crystalsUsed), crystalsUsed);
        if (fruitsUsed > 0) tag.Add(nameof(fruitsUsed), fruitsUsed);
    }

    //public override void OnEnterWorld() => Player.AddBuff(DefenseBoost.typeID, DefenseBoost.BuffTime);
}