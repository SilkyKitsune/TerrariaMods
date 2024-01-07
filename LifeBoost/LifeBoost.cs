using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LifeBoost;

internal sealed class LifeBoost : Mod { }

internal sealed class LifeBoostPlayer : ModPlayer
{
    internal bool lifeBoosted = false;
    internal int extraLife = 0;
    internal int crystalsUsed = 0;
    internal int fruitsUsed = 0;

    internal int currentDef = 0;
    internal int currentDefBoost = 0;

    public override void LoadData(TagCompound tag)
    {
        lifeBoosted = tag.TryGet(nameof(lifeBoosted), out bool boostValue) && boostValue;
        extraLife = tag.TryGet(nameof(extraLife), out int lifeValue) ? lifeValue : 0;
        crystalsUsed = tag.TryGet(nameof(crystalsUsed), out int crystalValue) ? crystalValue : 0;
        fruitsUsed = tag.TryGet(nameof(fruitsUsed), out int fruitValue) ? fruitValue : 0;
    }

    public override void PostUpdate() => currentDef = Player.statDefense - currentDefBoost;

    public override void SaveData(TagCompound tag)
    {
        if (lifeBoosted) tag.Add(nameof(lifeBoosted), lifeBoosted);
        if (extraLife != 0) tag.Add(nameof(extraLife), extraLife);
        if (crystalsUsed != 0) tag.Add(nameof(crystalsUsed), crystalsUsed);
        if (fruitsUsed != 0) tag.Add(nameof(fruitsUsed), fruitsUsed);
    }

    public override void OnEnterWorld(Player player)
    {
        if (extraLife > 0) player.statLifeMax += extraLife;

        if (!lifeBoosted)
        {
            player.statLifeMax += 100;
            lifeBoosted = true;
        }
        
        player.AddBuff(DefenseBoost.typeID, DefenseBoost.BuffTime);
    }
}

internal sealed class DefenseBoost : ModBuff
{
    internal const float DefMulti = 0.5f;

    internal const int BuffTime = 60;

    private const string ActiveBuffTip = "The spirits protect you...\n (x1.5 defense increase)";
    private const string InactiveBuffTip = "The spirits cannot protect you...\n (no defense increase)";

    internal static int typeID;

    public override void ModifyBuffTip(ref string tip, ref int rare) => tip = Main.hardMode ? ActiveBuffTip : InactiveBuffTip;

    public override bool RightClick(int buffIndex) => false;

    public override void SetStaticDefaults()
    {
        typeID = Type;
        Main.debuff[typeID] = false;
        Main.buffNoSave[typeID] = true;
        Main.persistentBuff[typeID] = true;
        Main.buffNoTimeDisplay[typeID] = true;
        DisplayName.SetDefault("Ethereal Protection");
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.buffTime[buffIndex] = BuffTime;
        if (Main.hardMode)
        {
            LifeBoostPlayer p = player.GetModPlayer<LifeBoostPlayer>();
            p.currentDefBoost = (int)(p.currentDef * DefMulti);
            player.statDefense += p.currentDefBoost;
        }
    }
}

internal sealed class LifeItemsBoost : GlobalItem
{
    internal const int MaxCrystals = 12;
    internal const int MaxFruits = 10;

    internal const int CrystalIncrease = 50;
    internal const int FruitIncrease = 20;

    internal const int PotionMulti = 2;

    public override void SetDefaults(Item item)
    {
        switch (item.type)
        {
            case ItemID.LesserHealingPotion:
                item.healLife *= PotionMulti;
                break;
            case ItemID.HealingPotion:
                goto case ItemID.LesserHealingPotion;
            case ItemID.GreaterHealingPotion:
                goto case ItemID.LesserHealingPotion;
            case ItemID.SuperHealingPotion:
                goto case ItemID.LesserHealingPotion;
            case ItemID.RestorationPotion:
                goto case ItemID.LesserHealingPotion;
        }
    }
    
    public override bool? UseItem(Item item, Player player)
    {
        LifeBoostPlayer p = player.GetModPlayer<LifeBoostPlayer>();
        
        if (item.type == ItemID.LifeCrystal && p.crystalsUsed < MaxCrystals)
        {
            player.statLifeMax += player.statLifeMax < 400 ? 30 : CrystalIncrease;
            p.crystalsUsed++;

            ConsumeItem(item, player);

            if (player.statLifeMax > 500) p.extraLife = player.statLifeMax - 500;

            return true;
        }
        if (item.type == ItemID.LifeFruit && p.fruitsUsed < MaxFruits)
        {
            player.statLifeMax += player.statLifeMax >= 400 && player.statLifeMax < 500 ? 15 : FruitIncrease;
            p.fruitsUsed++;

            ConsumeItem(item, player);

            if (player.statLifeMax > 500) p.extraLife = player.statLifeMax - 500;

            return true;
        }
        return null;
    }
}

#if DEBUG
[System.Obsolete] internal sealed class LifeStartBoost : ModItem
{
    internal static int typeID;

    public override void OnConsumeItem(Player player)
    {
        player.statLifeMax += 100;
        player.GetModPlayer<LifeBoostPlayer>().crystalsUsed += 2;
        base.OnConsumeItem(player);
    }
    
    public override bool? UseItem(Player player)
    {
        if (player.statLifeMax < 200)
        {
            ConsumeItem(player);
            return true;
        }
        return false;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item item = Item;
        item.maxStack = 1;
        item.value = Item.sellPrice(0, 0, 0, 1);
        //item.width = 40;
        //item.height = 40;
        item.useTime = 20;
        item.useAnimation = 20;
        item.useStyle = ItemUseStyleID.HoldUp;
        item.rare = ItemRarityID.Green;
        item.UseSound = SoundID.Item1;
        item.consumable = true;

        typeID = item.type;
    }
}
#endif