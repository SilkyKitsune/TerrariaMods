using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

#if DEBUG
using System.Diagnostics;
#endif

namespace GuaranteedBossDrops;

internal sealed class GuaranteedBossDrops : Mod { }//change modname

internal sealed class NPCDrops : GlobalNPC
{
    private static readonly int[] PreHardModeBars =
    {
        ItemID.CopperBar,
        ItemID.TinBar,
        ItemID.IronBar,
        ItemID.LeadBar,
        ItemID.SilverBar,
        ItemID.TungstenBar,
        ItemID.GoldBar,
        ItemID.PlatinumBar,

        ItemID.DemoniteBar,
        ItemID.CrimtaneBar,

        ItemID.MeteoriteBar,
        ItemID.HellstoneBar,
    };

    private static readonly int[] HardModeBars =
    {
        ItemID.OrichalcumBar,
        ItemID.PalladiumBar,
        ItemID.TitaniumBar,

        ItemID.MythrilBar,
        ItemID.CobaltBar,
        ItemID.AdamantiteBar,

        ItemID.HallowedBar,

        ItemID.ChlorophyteBar,
        ItemID.ShroomiteBar,
        ItemID.SpectreBar,

        ItemID.LunarBar,
    };

    private static readonly int[] Potions =
    {
        ItemID.AmmoReservationPotion,
        ItemID.ArcheryPotion,
        ItemID.BattlePotion,
        //ItemID.BiomeSightPotion,
        ItemID.BuilderPotion,
        ItemID.CalmingPotion,
        ItemID.CratePotion,
        ItemID.TrapsightPotion,
        ItemID.EndurancePotion,
        ItemID.FeatherfallPotion,
        ItemID.FishingPotion,
        ItemID.FlipperPotion,
        ItemID.GillsPotion,
        ItemID.GravitationPotion,
        ItemID.LuckPotionGreater,
        ItemID.HeartreachPotion,
        ItemID.HunterPotion,
        ItemID.InfernoPotion,
        ItemID.InvisibilityPotion,
        ItemID.IronskinPotion,
        ItemID.LuckPotionLesser,
        ItemID.LifeforcePotion,
        ItemID.LuckPotion,
        ItemID.MagicPowerPotion,
        ItemID.ManaRegenerationPotion,
        ItemID.MiningPotion,
        ItemID.NightOwlPotion,
        ItemID.ObsidianSkinPotion,
        ItemID.RagePotion,
        ItemID.RegenerationPotion,
        ItemID.ShinePotion,
        ItemID.SonarPotion,
        ItemID.SpelunkerPotion,
        ItemID.SummoningPotion,
        ItemID.SwiftnessPotion,
        ItemID.ThornsPotion,
        ItemID.TitanPotion,
        ItemID.WarmthPotion,
        ItemID.WaterWalkingPotion,
        ItemID.WrathPotion,

        ItemID.GenderChangePotion,
        ItemID.PotionOfReturn,
        ItemID.RecallPotion,
        ItemID.TeleportationPotion,
        ItemID.WormholePotion,

        ItemID.RedPotion,
    };

    private static readonly int[] Flasks =
    {
        ItemID.FlaskofCursedFlames,
        ItemID.FlaskofFire,
        ItemID.FlaskofGold,
        ItemID.FlaskofIchor,
        ItemID.FlaskofNanites,
        ItemID.FlaskofParty,
        ItemID.FlaskofPoison,
        ItemID.FlaskofVenom,
    };

    private static readonly Conditions.IsPreHardmode isPreHardmode = new();
    private static readonly Conditions.IsHardmode isHardmode = new();
    private static readonly Conditions.IsCorruption isCorruption = new();
    private static readonly Conditions.IsCrimson isCrimson = new();

    private static void AddBarDrops(ILoot loot)
    {
        List<int> metalBars = new();
        int[] bars = PreHardModeBars[0..7];
        metalBars.AddRange(bars);
        metalBars.AddRange(bars);

        metalBars.Add(ItemID.DemoniteBar);
        loot.Add(new VaryAmountFromOptionsWithChance(4, 1, 1, 3, isCorruption, metalBars.ToArray()));
        
        metalBars.RemoveAt(8);
        metalBars.Add(ItemID.CrimtaneBar);
        loot.Add(new VaryAmountFromOptionsWithChance(4, 1, 1, 3, isCrimson, metalBars.ToArray()));

        metalBars.Clear();
        bars = HardModeBars[0..5];
        metalBars.AddRange(bars);
        metalBars.AddRange(bars);
        metalBars.Add(ItemID.HallowedBar);
        loot.Add(new VaryAmountFromOptionsWithChance(4, 1, 1, 3, isHardmode, metalBars.ToArray()));
    }

    private static void AddPotionDrops(ILoot loot)
    {
        List<int> potionsNflasks = new(Potions[0..(Potions.Length - 2)]);
        potionsNflasks.AddRange(Flasks);
        loot.Add(ItemDropRule.OneFromOptions(10, potionsNflasks.ToArray()));
    }

    private static void AddSummonerDrops(ILoot loot)
    {
        loot.Add(ItemDropRule.Common(ItemID.SuspiciousLookingEye, 10));
        loot.Add(new VaryAmountFromOptionsWithChance(10, 1, 1, 1, isHardmode, ItemID.MechanicalEye, ItemID.MechanicalWorm, ItemID.MechanicalSkull));
    }

    [Obsolete] private static void MaxOutDrops(ILoot loot)
    {
        foreach (IItemDropRule rule in loot.Get(false))
        {
            loot.Remove(rule);
            
            List<DropRateInfo> infos = new();
            rule.ReportDroprates(infos, default);

            foreach (DropRateInfo info in infos)
            {
                if (info.conditions == null || info.conditions.Count == 0)
                    loot.Add(ItemDropRule.Common(info.itemId, 1, info.stackMax, info.stackMax));
                else foreach (IItemDropRuleCondition condition in info.conditions)
                    {
                        if (condition == null) loot.Add(ItemDropRule.Common(info.itemId, 1, info.stackMax, info.stackMax));
                        else loot.Add(ItemDropRule.ByCondition(condition, info.itemId, 1, info.stackMax, info.stackMax));
                    }
            }
        }
    }

#if DEBUG
    private static void PrintDrops(ILoot loot)
    {
        foreach (IItemDropRule rule in loot.Get(false))
        {
            List<DropRateInfo> infos = new();
            rule.ReportDroprates(infos, default);
            
            foreach (DropRateInfo info in infos)
            {
                Debug.WriteLine(
                    $"{ItemID.Search.GetName(info.itemId)}" +
                    $" {info.stackMin}" +
                    $" {info.stackMax}" +
                    $" {info.dropRate}");
                if (info.conditions == null || info.conditions.Count == 0) Debug.WriteLine(" no conditions");
                else foreach (IItemDropRuleCondition condition in info.conditions)
                        Debug.WriteLine(condition == null ? "  no condition" : $"  {condition}");
            }
        }
    }
#endif

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        switch (npc.type)
        {
            #region townNPCs
            case NPCID.Merchant:
            case NPCID.Nurse:
            case NPCID.ArmsDealer:
            case NPCID.Dryad:
            case NPCID.Guide:
            case NPCID.OldMan:
            case NPCID.Demolitionist:
            case NPCID.BoundGoblin:
            case NPCID.GoblinTinkerer:
            case NPCID.BoundWizard:
            case NPCID.Wizard:
            case NPCID.BoundMechanic:
            case NPCID.Mechanic:
            case NPCID.SantaClaus:
            case NPCID.Steampunker:
            case NPCID.DyeTrader:
            case NPCID.PartyGirl:
            case NPCID.Cyborg:
            case NPCID.Painter:
            case NPCID.WitchDoctor:
            case NPCID.Pirate:
            case NPCID.Stylist:
            case NPCID.WebbedStylist:
            case NPCID.TravellingMerchant:
            case NPCID.Angler:
            case NPCID.SleepingAngler:
            case NPCID.TaxCollector:
            case NPCID.DemonTaxCollector:
            case NPCID.DD2Bartender:
            case NPCID.BartenderUnconscious:
            case NPCID.Golfer:
            case NPCID.GolferRescue:
            case NPCID.BestiaryGirl:
            case NPCID.TownCat:
            case NPCID.TownDog:
            case NPCID.TownBunny:
            case NPCID.Princess:
            //townslimes
                break;
            #endregion

            #region animals
            case NPCID.Bunny:
            case NPCID.BlueJellyfish:
            case NPCID.PinkJellyfish:
            case NPCID.Crab:
            case NPCID.Bird:
            case NPCID.GreenJellyfish:
            case NPCID.Penguin:
            case NPCID.PenguinBlack:
            case NPCID.SeaSnail:
            case NPCID.Squid:
            case NPCID.GoldfishWalker:
            case NPCID.BirdBlue:
            case NPCID.BirdRed:
            case NPCID.Squirrel:
            case NPCID.Mouse:
            case NPCID.BunnyXmas:
            case NPCID.Firefly:
            case NPCID.Butterfly:
            case NPCID.Worm:
            case NPCID.LightningBug:
            case NPCID.Snail:
            case NPCID.GlowingSnail:
            case NPCID.Frog:
            case NPCID.Duck:
            case NPCID.Duck2:
            case NPCID.DuckWhite:
            case NPCID.DuckWhite2:
            case NPCID.Scorpion:
            case NPCID.ScorpionBlack:
            case NPCID.TruffleWorm:
            case NPCID.TruffleWormDigger:
            case NPCID.Grasshopper:
            case NPCID.GoldBird:
            case NPCID.GoldBunny:
            case NPCID.GoldButterfly:
            case NPCID.GoldFrog:
            case NPCID.GoldGrasshopper:
            case NPCID.GoldMouse:
            case NPCID.GoldWorm:
            case NPCID.EnchantedNightcrawler:
            case NPCID.Grubby:
            case NPCID.Sluggy:
            case NPCID.Buggy:
            case NPCID.SquirrelRed:
            case NPCID.SquirrelGold:
            case NPCID.PartyBunny:
            case NPCID.GoldGoldfish:
            case NPCID.GoldGoldfishWalker:
            case NPCID.WindyBalloon:
            case NPCID.BlackDragonfly:
            case NPCID.BlueDragonfly:
            case NPCID.GreenDragonfly:
            case NPCID.OrangeDragonfly:
            case NPCID.RedDragonfly:
            case NPCID.YellowDragonfly:
            case NPCID.GoldDragonfly:
            case NPCID.Seagull:
            case NPCID.Seagull2:
            case NPCID.LadyBug:
            case NPCID.GoldLadyBug:
            case NPCID.Maggot:
            case NPCID.Pupfish:
            case NPCID.Grebe:
            case NPCID.Grebe2:
            case NPCID.Rat:
            case NPCID.Owl:
            case NPCID.WaterStrider:
            case NPCID.GoldWaterStrider:
            case NPCID.ExplosiveBunny:
            case NPCID.Dolphin:
            case NPCID.Turtle:
            case NPCID.TurtleJungle:
            case NPCID.SeaTurtle:
            case NPCID.Seahorse:
            case NPCID.GoldSeahorse:
            case NPCID.GemSquirrelAmethyst:
            case NPCID.GemSquirrelTopaz:
            case NPCID.GemSquirrelSapphire:
            case NPCID.GemSquirrelEmerald:
            case NPCID.GemSquirrelRuby:
            case NPCID.GemSquirrelDiamond:
            case NPCID.GemSquirrelAmber:
            case NPCID.GemBunnyAmethyst:
            case NPCID.GemBunnyTopaz:
            case NPCID.GemBunnySapphire:
            case NPCID.GemBunnyEmerald:
            case NPCID.GemBunnyRuby:
            case NPCID.GemBunnyDiamond:
            case NPCID.GemBunnyAmber:
            case NPCID.HellButterfly:
            case NPCID.Lavafly:
            case NPCID.MagmaSnail:
            case NPCID.EmpressButterfly:
            //case NPCID.Stinkbug:
            //case NPCID.ScarletMacaw:
            //case NPCID.BlueMacaw:
            //case NPCID.Toucan:
            //case NPCID.YellowCockatiel:
            //case NPCID.GrayCockatiel:
            //case NPCID.Shimmerfly:
                break;
            #endregion

            case NPCID.VileSpit:
            case NPCID.TargetDummy:
            case NPCID.TorchGod:
            case NPCID.ChaosBallTim:
            case NPCID.VileSpitEaterOfWorlds:
                break;

            #region spiders
            case NPCID.WallCreeper:
            case NPCID.WallCreeperWall:
            //case NPCID.JungleCreeper:
            //case NPCID.JungleCreeperWall:
            case NPCID.BloodCrawler:
            case NPCID.BloodCrawlerWall:
                npcLoot.Add(ItemDropRule.Common(ItemID.Cobweb, 1, 5, 10));
                goto default;
            case NPCID.BlackRecluse:
            case NPCID.BlackRecluseWall:
                npcLoot.Add(ItemDropRule.OneFromOptions(10, ItemID.SpiderStaff, ItemID.QueenSpiderStaff));
                goto case NPCID.BloodCrawlerWall;
            #endregion

            //surface desert enemies drop scarab bombs? (Do any already?)
            //they drop from desert pots I think

            #region jungle enemies
            case NPCID.Hornet:
            case NPCID.ManEater:
            case NPCID.JungleSlime:
            case NPCID.Piranha:
            case NPCID.LacBeetle:
            case NPCID.DoctorBones:
                npcLoot.Add(ItemDropRule.Common(ItemID.Abeemination, 10));
                goto default;

            case NPCID.JungleCreeper:
            case NPCID.JungleCreeperWall:
                npcLoot.Add(ItemDropRule.Common(ItemID.Cobweb, 1, 5, 10));
                goto case NPCID.GiantTortoise;
            case NPCID.Moth:
            case NPCID.MossHornet:
            case NPCID.AngryTrapper:
            case NPCID.Arapaima:
            case NPCID.GiantTortoise:
                npcLoot.Add(new VaryAmountFromOptionsWithChance(10, 1, 1, 1, isHardmode, ItemID.TempleKey, ExtractedBulb.typeID));
                goto default;
            #endregion

            #region hell enemies
            case NPCID.BoneSerpentHead:
            case NPCID.BoneSerpentBody:
            case NPCID.BoneSerpentTail:
                npcLoot.Add(ItemDropRule.Common(ItemID.Bone, 1, 10, 20));
                break;
            case NPCID.Hellbat:
            case NPCID.LavaSlime:
            case NPCID.Demon:
            case NPCID.VoodooDemon:
            case NPCID.Lavabat:
            case NPCID.RedDevil:
                npcLoot.Add(ItemDropRule.Common(ItemID.HellstoneBar, 2, 1, 3));
                break;
            #endregion

            #region bosses
            case NPCID.EyeofCthulhu:
            case NPCID.KingSlime:
            case NPCID.BrainofCthulhu:
            case NPCID.QueenBee:
            case NPCID.SkeletronHead:
            case NPCID.Deerclops:
            case NPCID.WallofFlesh:
            case NPCID.QueenSlimeBoss:
            case NPCID.Retinazer:
            case NPCID.Spazmatism:
            case NPCID.TheDestroyer:
            case NPCID.TheDestroyerBody:
            case NPCID.TheDestroyerTail:
            case NPCID.SkeletronPrime:
            case NPCID.Plantera:
            case NPCID.Golem:
            case NPCID.GolemHead:
            case NPCID.GolemHeadFree:
            case NPCID.DukeFishron:
            case NPCID.HallowBoss:
            case NPCID.CultistBoss:
            case NPCID.MoonLordHead:
            case NPCID.MoonLordHand:
            case NPCID.MoonLordCore:
            case NPCID.LunarTowerSolar:
            case NPCID.LunarTowerNebula:
            case NPCID.LunarTowerVortex:
            case NPCID.LunarTowerStardust:
            case NPCID.PirateShip:
            case NPCID.MartianSaucer:
                npcLoot.Add(ItemDropRule.ByCondition(isPreHardmode, ItemID.WoodenCrate, 1, 2, 5));
                npcLoot.Add(ItemDropRule.ByCondition(isHardmode, ItemID.WoodenCrateHard, 1, 2, 5));

                npcLoot.Add(ItemDropRule.ByCondition(isPreHardmode, ItemID.IronCrate, 1, 1, 3));
                npcLoot.Add(ItemDropRule.ByCondition(isHardmode, ItemID.IronCrateHard, 1, 1, 3));

                npcLoot.Add(ItemDropRule.ByCondition(isPreHardmode, ItemID.GoldenCrate, 2, 1, 1));
                npcLoot.Add(ItemDropRule.ByCondition(isHardmode, ItemID.GoldenCrateHard, 2, 1, 1));
                break;
            #endregion

            default:
                AddBarDrops(npcLoot);
                AddPotionDrops(npcLoot);
                AddSummonerDrops(npcLoot);
                break;
        }
    }

    public override void SetupShop(int type, Chest shop, ref int nextSlot)
    {
        if (type == NPCID.BestiaryGirl)
        {
            shop.item[nextSlot++].SetDefaults(ItemID.BabyBirdStaff);
            if (Main.hardMode) shop.item[nextSlot++].SetDefaults(ItemID.RavenStaff);
        }
    }
}

internal sealed class ShadowOrbDrops : GlobalTile
{
    private int i = 0;

    public override bool Drop(int i, int j, int type)
    {
        if (i >= 4) i = 0;

        if (type == TileID.ShadowOrbs && i++ == 0)//still manages to run 5 times
        {
            //Player p = Main.player[Main.myPlayer];
            //p.QuickSpawnItem(p.GetSource_GiftOrReward(), ItemID.WormFood);
            Item.NewItem(null, i * 16, j * 16, 0, 0, ItemID.WormFood);
            //Debug.WriteLine("test");
        }

        return true;
    }
}

internal sealed class ExtractedBulb : ModItem
{
    internal static int typeID;

    public override bool CanUseItem(Player player)
    {
        if (NPC.AnyNPCs(NPCID.Plantera)) return false;
        return true;
    }

    public override bool? UseItem(Player player)
    {
        Player p = Main.player[Main.myPlayer];
        NPC.SpawnBoss((int)(p.position.X - 100), (int)(p.position.Y - 50), NPCID.Plantera, Main.myPlayer);
        return true;
    }

    public override void SetDefaults()
    {
        Item item = Item;
        item.maxStack = 20;
        item.value = Item.sellPrice(0, 0, 0, 20);
        //item.width = 40;
        //item.height = 40;
        item.useTime = 20;
        item.useAnimation = 20;
        item.useStyle = ItemUseStyleID.HoldUp;
        //item.rare = ItemRarityID.Green;
        item.UseSound = SoundID.Item1;
        item.consumable = true;

        typeID = item.type;
    }
}