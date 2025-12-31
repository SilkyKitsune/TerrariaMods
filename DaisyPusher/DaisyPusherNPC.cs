using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DaisyPusher;

public sealed class DaisyPusherNPC : ModNPC
{
    public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(ItemDropRule.Common(ItemID.HerbBag, 1, 1, 3));

    public override void SetDefaults()
    {
        NPC npc = NPC;

        npc.width = 18;
        npc.height = 40;
        npc.aiStyle = NPCAIStyleID.Fighter;
        npc.damage = 14;
        npc.defense = 6;
        npc.lifeMax = 45;
        npc.HitSound = SoundID.NPCHit1;
        npc.DeathSound = SoundID.NPCDeath2;
        npc.knockBackResist = 0.5f;
        npc.value = 60f;

        AIType = NPCID.Zombie;
        AnimationType = NPCID.Zombie;
        BannerItem = Item.BannerToItem(
            Banner = Item.NPCtoBanner(NPCID.Zombie));
    }

    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

    public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.OverworldNightMonster.Chance / 6f;
}