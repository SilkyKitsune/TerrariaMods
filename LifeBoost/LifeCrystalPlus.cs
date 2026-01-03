using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeBoost;

public sealed class LifeCrystalPlus : ModItem
{
    internal static int typeID = 0;

    public override void AddRecipes() => CreateRecipe().AddIngredient(ItemID.LifeCrystal).Register();

    public override bool CanUseItem(Player player) =>
        player.TryGetModPlayer(out LifeBoostPlayer modPlayer) && modPlayer.lifeCrystalsUsed < ModContent.GetInstance<LifeBoostConfig>().maxLifeCrystals;

    public override void ModifyTooltips(List<TooltipLine> tooltips) =>
        tooltips.Add(new(Mod, "Tooltip0", "Permanently increases maximum life by " + ModContent.GetInstance<LifeBoostConfig>().lifeCrystalAmount));

    public override void SetDefaults()
    {
        Item item = Item;
        typeID = item.type;
        item.CloneDefaults(ItemID.LifeCrystal);
        item.consumable = true;
    }

    public override bool? UseItem(Player player)
    {
        if (player.TryGetModPlayer(out LifeBoostPlayer modPlayer))
        {
            modPlayer.lifeCrystalsUsed++;
            return true;
        }
        return false;
    }
}