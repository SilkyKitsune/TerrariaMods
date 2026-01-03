using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeBoost;

public sealed class ManaCrystalPlus : ModItem
{
    internal static int typeID = 0;

    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.ManaCrystal).Register();
        CreateRecipe().AddIngredient(ItemID.FallenStar, 5).Register();
    }

    public override bool CanUseItem(Player player) =>
        player.TryGetModPlayer(out LifeBoostPlayer modPlayer) && modPlayer.manaCrystalsUsed < ModContent.GetInstance<LifeBoostConfig>().maxManaCrystals;

    public override void ModifyTooltips(List<TooltipLine> tooltips) =>
        tooltips.Add(new(Mod, "Tooltip0", "Permanently increases maximum mana by " + ModContent.GetInstance<LifeBoostConfig>().manaCrystalAmount));

    public override void SetDefaults()
    {
        Item item = Item;
        typeID = item.type;
        item.CloneDefaults(ItemID.ManaCrystal);
        item.consumable = true;
    }

    public override bool? UseItem(Player player)
    {
        if (player.TryGetModPlayer(out LifeBoostPlayer modPlayer))
        {
            modPlayer.manaCrystalsUsed++;
            return true;
        }
        return false;
    }
}