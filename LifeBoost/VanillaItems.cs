using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace LifeBoost;

public sealed class VanillaItems : GlobalItem
{
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
                item.healLife *= 2;
                break;     
        }*/
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (item.type == ItemID.LifeCrystal || item.type == ItemID.LifeFruit || item.type == ItemID.ManaCrystal) tooltips.RemoveAt(tooltips.Count - 1);
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