using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CustomStatBars;

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

    /*public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) =>
        [
            new(ItemID.GoldPickaxe),
            new(ItemID.GoldAxe),
            new(ItemID.DD2PetGhost),
            new(ItemID.ExoticEasternChewToy),
            new(ItemID.TinkerersWorkshop)
        ];*/

    public override bool CanUseItem(Item item) => !mouseOverButtons && base.CanUseItem(item);

#if V1_4_3
    public override void OnEnterWorld(Player player) => player.AddBuff(TimeBuff.typeID, 60);
#elif V1_4_4
    //public override void OnEnterWorld() => Player.AddBuff(TimeBuff.typeID, 60);
#endif
}

internal sealed class TimeBuff : ModBuff
{
    private const int ExtraTimeTick = 3;
    
    private const string NormalTip = "Time flows out of control...", BossActiveTip = "Time stands still...";

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

#if V1_4_3
    public override void ModifyBuffTip(ref string tip, ref int rare) =>
#elif V1_4_4
    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) =>
#endif
        tip = NPC.AnyNPCs(NPCID.EyeofCthulhu) ? BossActiveTip : NormalTip;

    public override bool RightClick(int buffIndex) => false;

    public override void SetStaticDefaults()
    {
        typeID = Type;
        Main.debuff[typeID] = false;
        Main.buffNoSave[typeID] = true;
        Main.persistentBuff[typeID] = true;
        Main.buffNoTimeDisplay[typeID] = true;
#if V1_4_3
        DisplayName.SetDefault("Time Distortion");
#elif V1_4_4
        //?
#endif
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

#if V1_4_3
internal sealed class IncreasedItemStacks : GlobalItem
{
    //temp placement
    public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (Main.npc.Length == 0) return;
        
        Vector2 mouse = Main.MouseWorld, upper = mouse.RotatedBy(-0.05d * Math.Tau, player.position);
        double d = 0.1d * Math.Tau;

        //find closest enemy within angle of upper and lower
        //find angle between projectile and enemy
        //rotate velocity to enemy

        float delta = float.MaxValue, newDelta;

        NPC closest = Main.npc[0];
        foreach (NPC npc in Main.npc)
        {
            if (npc != null)
            {
                if (npc.position.Equals(player.position))
                {
                    closest = npc;
                    return;
                }

                if (closest != npc)
                {
                    newDelta = player.position.Distance(npc.position);
                    if (newDelta < delta)
                    {
                        float angle = npc.position.AngleFrom(upper);
                        if (angle < d)
                        {
                            closest = npc;
                            delta = newDelta;
                        }
                    }
                }
            }
        }
        velocity = velocity.RotatedBy(closest.position.AngleFrom(mouse), player.position);
    }

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
        if (npc.type == NPCID.Spazmatism) npc.lifeMax /= 4;
        else if (npc.type == NPCID.GoblinSummoner)//move to own mod
        {
            //npc.lifeMax /= 4;
            npc.defense = 5;
        }
    }
}
#endif