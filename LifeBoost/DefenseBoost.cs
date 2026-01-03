using Terraria;
using Terraria.ModLoader;

namespace LifeBoost;

[System.Obsolete]
internal sealed class DefenseBoost : ModBuff
{
    internal const float DefMulti = 0.5f;

    internal const int BuffTime = 60;

    private const string ActiveBuffTip = "The spirits protect you...\n (x1.5 defense increase)";
    private const string InactiveBuffTip = "The spirits cannot protect you...\n (no defense increase)";

    internal static int typeID;

#if V1_4_3
    public override void ModifyBuffTip(ref string tip, ref int rare) => tip = Main.hardMode ? ActiveBuffTip : InactiveBuffTip;
#elif V1_4_4
    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) =>
#endif
        tip = Main.hardMode ? ActiveBuffTip : InactiveBuffTip;

    public override bool RightClick(int buffIndex) => false;

    public override void SetStaticDefaults()
    {
        typeID = Type;
        Main.debuff[typeID] = false;
        Main.buffNoSave[typeID] = true;
        Main.persistentBuff[typeID] = true;
        Main.buffNoTimeDisplay[typeID] = true;
#if V1_4_3
        DisplayName.SetDefault("Ethereal Protection");
#elif V1_4_4
        //?
#endif
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.buffTime[buffIndex] = BuffTime;
        if (Main.hardMode) player.statDefense *= 1.5f;
    }
}