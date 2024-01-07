using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChestConverter;

internal sealed class ChestConverter : Mod { }

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
        //ItemID.CorruptionChest,
        //ItemID.CrimsonChest,
        //ItemID.HallowedChest,
        //ItemID.FrozenChest,
        //ItemID.JungleChest,

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
        //ItemID.DungeonDesertChest,
        
        //ItemID.AshWoodChest,//I think this is in a newer version
        //ItemID.BalloonChest,//I think this is in a newer version
        //ItemID.ReefChest,//I think this is in a newer version
    };

    internal static int ChestGroupID;

    internal static void AddChestRecipe(int id)
    {
        Recipe recipe = Recipe.Create(id);
        recipe.AddRecipeGroup(ChestGroupID);
        recipe.Register();
    }

    public override void AddRecipes()
    {
        foreach (int id in chestIDs) AddChestRecipe(id);
    }

    public override void AddRecipeGroups()
    {
        RecipeGroup chestGroup = new(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.Chest)}", chestIDs);
        ChestGroupID = RecipeGroup.RegisterGroup("Chest", chestGroup);
    }
}