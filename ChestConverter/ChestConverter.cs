using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChestConverter;

internal sealed class ChestConverterSystem : ModSystem
{
    internal static readonly int[] chestIDs =
    {
        ItemID.Chest,
        ItemID.GoldChest,
        ItemID.ShadowChest,
        ItemID.EbonwoodChest,
        ItemID.RichMahoganyChest,
        ItemID.PearlwoodChest,
        ItemID.IvyChest,
        ItemID.IceChest,
        ItemID.LivingWoodChest,
        ItemID.SkywareChest,
        ItemID.ShadewoodChest,
        ItemID.WebCoveredChest,

        ItemID.LihzahrdChest,
        ItemID.WaterChest,
#if V1_4_4
        ItemID.CorruptionChest,
        ItemID.CrimsonChest,
        ItemID.HallowedChest,
        ItemID.FrozenChest,
        ItemID.JungleChest,
#endif

        ItemID.MushroomChest,
        ItemID.BoneChest,
        ItemID.FleshChest,
        ItemID.GlassChest,
        ItemID.HoneyChest,
        ItemID.SlimeChest,
        ItemID.SteampunkChest,
        ItemID.BlueDungeonChest,
        ItemID.BorealWoodChest,
        ItemID.CactusChest,
        ItemID.DynastyChest,
        ItemID.GreenDungeonChest,
        ItemID.MartianChest,
        ItemID.ObsidianChest,
        ItemID.PalmWoodChest,
        ItemID.PinkDungeonChest,
        ItemID.PumpkinChest,
        ItemID.SpookyChest,

        ItemID.GraniteChest,
        ItemID.MarbleChest,
        ItemID.GoldenChest,
        ItemID.LesionChest,
        ItemID.CrystalChest,
        ItemID.MeteoriteChest,
        ItemID.SpiderChest,

        ItemID.DesertChest,
        ItemID.NebulaChest,
        ItemID.SolarChest,
        ItemID.StardustChest,
        ItemID.VortexChest,
        ItemID.BambooChest,
#if V1_4_4
        ItemID.DungeonDesertChest,
        
        ItemID.AshWoodChest,
        ItemID.BalloonChest,
        //ItemID.ReefChest,
#endif
    };

    internal static int ChestGroupID;

    public override void AddRecipeGroups()
    {
        RecipeGroup chestGroup = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.Chest)}", chestIDs);
        ChestGroupID = RecipeGroup.RegisterGroup("Chest", chestGroup);
    }

    public override void AddRecipes()
    {
        foreach (int id in chestIDs)
        {
            Recipe recipe = Recipe.Create(id);
            recipe.AddRecipeGroup(ChestGroupID);
            recipe.Register();
        }
    }
}