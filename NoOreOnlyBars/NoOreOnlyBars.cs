using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

using static NoOreOnlyBars.NoOreOnlyBars;

namespace NoOreOnlyBars
{
	internal sealed class NoOreOnlyBars : Mod
	{
        private const int dropRedux = 2;//would reduce 1 to 0
        
        internal static int OreItemToBar(int id) => id switch
        {
            ItemID.CopperOre => ItemID.CopperBar,
            ItemID.TinOre => ItemID.TinBar,
            ItemID.IronOre => ItemID.IronBar,
            ItemID.LeadOre => ItemID.LeadBar,
            ItemID.SilverOre => ItemID.SilverBar,
            ItemID.TungstenOre => ItemID.TungstenBar,
            ItemID.GoldOre => ItemID.GoldBar,
            ItemID.PlatinumOre => ItemID.PlatinumBar,

            ItemID.DemoniteOre => ItemID.DemoniteBar,
            ItemID.CrimtaneOre => ItemID.CrimtaneBar,

            ItemID.Meteorite => ItemID.MeteoriteBar,
            ItemID.Hellstone => ItemID.HellstoneBar,

            ItemID.OrichalcumOre => ItemID.OrichalcumBar,
            ItemID.PalladiumOre => ItemID.PalladiumBar,
            ItemID.TitaniumOre => ItemID.TitaniumBar,

            ItemID.MythrilOre => ItemID.MythrilBar,
            ItemID.CobaltOre => ItemID.CobaltBar,
            ItemID.AdamantiteOre => ItemID.AdamantiteBar,

            ItemID.ChlorophyteOre => ItemID.ChlorophyteBar,

            _ => int.MinValue
        };

        internal static int OreTileToBar(int id) => id switch
        {
            TileID.Copper => ItemID.CopperBar,
            TileID.Tin => ItemID.TinBar,
            TileID.Iron => ItemID.IronBar,
            TileID.Lead => ItemID.LeadBar,
            TileID.Silver => ItemID.SilverBar,
            TileID.Tungsten => ItemID.TungstenBar,
            TileID.Gold => ItemID.GoldBar,
            TileID.Platinum => ItemID.PlatinumBar,

            TileID.Demonite => ItemID.DemoniteBar,
            TileID.Crimtane => ItemID.CrimtaneBar,

            TileID.Meteorite => ItemID.MeteoriteBar,
            TileID.Hellstone => ItemID.HellstoneBar,

            TileID.Orichalcum => ItemID.OrichalcumBar,
            TileID.Palladium => ItemID.PalladiumBar,
            TileID.Titanium => ItemID.TitaniumBar,

            TileID.Mythril => ItemID.MythrilBar,
            TileID.Cobalt => ItemID.CobaltBar,
            TileID.Adamantite => ItemID.AdamantiteBar,

            TileID.Chlorophyte => ItemID.ChlorophyteBar,

            _ => int.MinValue
        };

        private static int ReduceStack(int amount) => amount < 2 ? amount : amount / dropRedux;

        internal static void ReplaceOreWithBars(ILoot loot)
        {
            List<IItemDropRule> rules = loot.Get();
            foreach (IItemDropRule rule in rules)
            {
                List<DropRateInfo> infos = new();
                rule.ReportDroprates(infos, default);

                int barType = int.MinValue;
                foreach (DropRateInfo info in infos)
                {
                    barType = OreItemToBar(info.itemId);
                    if (barType != int.MinValue) break;
                }

                if (barType != int.MinValue)
                {
                    loot.Remove(rule);
                    DropRateInfo info = infos[0];
                    if (info.conditions == null || info.conditions.Count == 0)
                        loot.Add(ItemDropRule.Common(barType, 1, ReduceStack(info.stackMin), ReduceStack(info.stackMax)));
                    else foreach (IItemDropRuleCondition condition in info.conditions)
                        {
                            if (condition == null) loot.Add(ItemDropRule.Common(barType, 1, ReduceStack(info.stackMin), ReduceStack(info.stackMax)));
                            else loot.Add(ItemDropRule.ByCondition(condition, barType, 1, ReduceStack(info.stackMin), ReduceStack(info.stackMax)));
                        }
                }
            }
        }
    }

	internal sealed class ItemDrops : GlobalItem
	{
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot) => ReplaceOreWithBars(itemLoot);
    }

	internal sealed class TileDrops : GlobalTile
	{
        public override bool Drop(int i, int j, int type)
        {
            int bar = OreTileToBar(type);
            if (bar != int.MinValue)
            {
                Item.NewItem(null, i * 16, j * 16, 0, 0, bar);
                return false;
            }
            return true;
        }
    }

	internal sealed class NPCDrops : GlobalNPC
	{
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.MeteorHead:
                case NPCID.EyeofCthulhu:
                case NPCID.KingSlime:
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
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
                    ReplaceOreWithBars(npcLoot);
                    break;
            }
        }
    }
}