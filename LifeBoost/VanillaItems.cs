using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeBoost;

public sealed class VanillaItems : GlobalItem
{
    internal const int MaxCrystals = 12;
    internal const int MaxFruits = 10;

    internal const int CrystalIncrease = 50;
    internal const int FruitIncrease = 20;

    internal const int PotionMulti = 2;

    public override bool CanUseItem(Item item, Player player) =>
        item.type != ItemID.LifeCrystal && item.type != ItemID.LifeFruit && item.type != ItemID.ManaCrystal;

    public override void SetDefaults(Item item)
    {
        if (item.type == ItemID.LifeCrystal || item.type == ItemID.LifeFruit || item.type == ItemID.ManaCrystal) item.consumable = false;

        /*switch (item.type)
        {
            case ItemID.LesserHealingPotion:
            case ItemID.HealingPotion:
            case ItemID.GreaterHealingPotion:
            case ItemID.SuperHealingPotion:
            case ItemID.RestorationPotion:
                item.healLife *= PotionMulti;
                break;     
        }*/
    }

    /*public override bool? UseItem(Item item, Player player)
    {
        LifeBoostPlayer p = player.GetModPlayer<LifeBoostPlayer>();
        
        if (item.type == ItemID.LifeCrystal && p.crystalsUsed < MaxCrystals)
        {
            player.statLifeMax += player.statLifeMax < 400 ? 30 : CrystalIncrease;
            p.crystalsUsed++;

            ConsumeItem(item, player);

            if (player.statLifeMax > 500) p.extraLife = player.statLifeMax - 500;

            return true;
        }
        if (item.type == ItemID.LifeFruit && p.fruitsUsed < MaxFruits)
        {
            player.statLifeMax += player.statLifeMax >= 400 && player.statLifeMax < 500 ? 15 : FruitIncrease;
            p.fruitsUsed++;

            ConsumeItem(item, player);

            if (player.statLifeMax > 500) p.extraLife = player.statLifeMax - 500;

            return true;
        }
        return null;
    }*/
}