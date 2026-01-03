using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeBoost;

public sealed class LifeFruitPlus : ModItem
{
    internal static int typeID = 0;

    public override void AddRecipes() => CreateRecipe().AddIngredient(ItemID.LifeFruit).Register();

    public override bool CanUseItem(Player player)
    {
        LifeBoostConfig config = ModContent.GetInstance<LifeBoostConfig>();
        return player.TryGetModPlayer(out LifeBoostPlayer modPlayer) &&
            (!config.lifeFruitRequireMaxLifeCrystals || modPlayer.lifeCrystalsUsed >= config.maxLifeCrystals) &&
            modPlayer.lifeFruitsUsed < config.maxLifeFruits;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips) =>
        tooltips.Add(new(Mod, "Tooltip0", "Permanently increases maximum life by " + ModContent.GetInstance<LifeBoostConfig>().lifeFruitAmount));

    public override void SetDefaults()
    {
        Item item = Item;
        typeID = item.type;
        item.CloneDefaults(ItemID.LifeFruit);
        item.consumable = true;
    }

    public override bool? UseItem(Player player)
    {
        if (player.TryGetModPlayer(out LifeBoostPlayer modPlayer))
        {
            modPlayer.lifeFruitsUsed++;
            return true;
        }
        return false;
    }
}