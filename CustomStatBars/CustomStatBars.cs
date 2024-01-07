using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.ModLoader;

namespace CustomStatBars;

internal sealed class CustomStatBars : Mod { }

internal sealed class CustomModSystem : ModSystem
{
    private sealed class CustomUI : UIState
    {
        private static readonly Color grey = new(64, 64, 64);

        internal CustomUI(int statBarsX, int statBarsY, int hotBarX, int hotBarY)
        {
            lifeBar = new(statBarsX, statBarsY, 500, 25, grey, new(255, 0, 64));
            manaBar = new(statBarsX + 125, statBarsY + 30, 250, 20, grey, new(64, 0, 255));
            //hotBar = new(hotBarX, hotBarY, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
            healButton = new(hotBarX - 75, hotBarY);
            //sicknessButton = new(hotBarX - 200, hotBarY);
        }

        private readonly UIPercentBar lifeBar, manaBar;
        //private readonly UIHotBar hotBar;
        private readonly UIHealButton healButton;
        //private readonly UIClearSicknessButton sicknessButton;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Player p = Main.player[Main.myPlayer];
            lifeBar.SetValue(p.statLife, p.statLifeMax);
            manaBar.SetValue(p.statMana, p.statManaMax);
            base.Draw(spriteBatch);
        }

        public override void OnInitialize()
        {
            Append(lifeBar);
            Append(manaBar);
            //Append(hotBar);
            Append(healButton);
            //Append(sicknessButton);
        }

        public override void Update(GameTime gameTime) =>
            Main.player[Main.myPlayer].GetModPlayer<CustomPlayer>().mouseOverButtons = healButton.mouseOver /*|| hotBar.MouseOverHotbar /*|| sicknessButton.mouseOver*/;
    }

    private readonly UserInterface ui = new();
    private readonly CustomUI cui = new(500, 25, 600 - 100, 800);

    public override void Load() => ui.SetState(cui);

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
        if (resourceBarIndex > -1)
        {
            layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer("CustomHUD", () =>
            {
                GameTime gameTime = new();
                ui.Draw(Main.spriteBatch, gameTime);
                return true;
            }, InterfaceScaleType.UI));
        }
    }

    public override void UpdateUI(GameTime gameTime) => ui.Update(gameTime);
}

internal sealed class CustomPlayer : ModPlayer
{
    internal bool mouseOverButtons = false;

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) =>
        new Item[]
        {
            new Item(ItemID.GoldPickaxe),
            new Item(ItemID.GoldAxe),
            new Item(ItemID.DD2PetGhost),
            new Item(ItemID.ExoticEasternChewToy),
            new Item(ItemID.TinkerersWorkshop)
        };

    public override bool CanUseItem(Item item) => !mouseOverButtons && base.CanUseItem(item);

    public override void OnEnterWorld(Player player) => player.AddBuff(TimeBuff.typeID, 60);
}

internal sealed class TimeBuff : ModBuff
{
    private const int ExtraTimeTick = 3;
    
    private const string NormalTip = "Time flows out of control...", BossActiveTip = "Time stand still...";

    internal static int typeID;

    private double time = -1;

    private static bool AnyBosses() =>
        NPC.AnyNPCs(NPCID.EyeofCthulhu) ||
        NPC.AnyNPCs(NPCID.KingSlime) ||
        NPC.AnyNPCs(NPCID.EaterofWorldsHead) ||
        NPC.AnyNPCs(NPCID.EaterofWorldsBody) ||
        NPC.AnyNPCs(NPCID.EaterofWorldsTail) ||
        NPC.AnyNPCs(NPCID.BrainofCthulhu) ||
        NPC.AnyNPCs(NPCID.QueenBee) ||
        NPC.AnyNPCs(NPCID.SkeletronHead) ||
        NPC.AnyNPCs(NPCID.Deerclops) ||
        NPC.AnyNPCs(NPCID.WallofFlesh) ||
        NPC.AnyNPCs(NPCID.QueenSlimeBoss) ||
        NPC.AnyNPCs(NPCID.Retinazer) ||
        NPC.AnyNPCs(NPCID.Spazmatism) ||
        NPC.AnyNPCs(NPCID.TheDestroyer) ||
        NPC.AnyNPCs(NPCID.TheDestroyerBody) ||
        NPC.AnyNPCs(NPCID.TheDestroyerTail) ||
        NPC.AnyNPCs(NPCID.SkeletronPrime) ||
        NPC.AnyNPCs(NPCID.Plantera) ||
        NPC.AnyNPCs(NPCID.Golem) ||
        NPC.AnyNPCs(NPCID.GolemHead) ||
        NPC.AnyNPCs(NPCID.GolemHeadFree) ||
        NPC.AnyNPCs(NPCID.DukeFishron) ||
        NPC.AnyNPCs(NPCID.HallowBoss) ||
        NPC.AnyNPCs(NPCID.CultistBoss) ||
        NPC.AnyNPCs(NPCID.MoonLordHead) ||
        NPC.AnyNPCs(NPCID.MoonLordHand) ||
        NPC.AnyNPCs(NPCID.MoonLordCore);

    public override void ModifyBuffTip(ref string tip, ref int rare) => tip = NPC.AnyNPCs(NPCID.EyeofCthulhu) ? BossActiveTip : NormalTip;

    public override bool RightClick(int buffIndex) => false;

    public override void SetStaticDefaults()
    {
        typeID = Type;
        Main.debuff[typeID] = false;
        Main.buffNoSave[typeID] = true;
        Main.persistentBuff[typeID] = true;
        Main.buffNoTimeDisplay[typeID] = true;
        DisplayName.SetDefault("Time Distortion");
    }
    
    public override void Update(Player player, ref int buffIndex)
    {
        player.buffTime[buffIndex] = 60;
        if (AnyBosses())
        {
            if (time < 0) time = Main.time;
            Main.time = time;
        }
        else
        {
            time = -1d;
            Main.time += ExtraTimeTick;
        }
    }
}

internal sealed class IncreasedItemStacks : GlobalItem
{
    public override void SetDefaults(Item item)
    {
        if (item.potion) item.maxStack = 999;
        else if (item.createTile > -1 && item.type != ItemID.CopperCoin && item.type != ItemID.SilverCoin && item.type != ItemID.GoldCoin) item.maxStack = 9999;
    }
}

internal sealed class FuckSpasmatism : GlobalNPC
{
    public override void SetDefaults(NPC npc)
    {
        if (npc.type == NPCID.Spazmatism)
        {
            //npc.lifeMax = 1;
            npc.lifeMax /= 4;
        }
    }
}