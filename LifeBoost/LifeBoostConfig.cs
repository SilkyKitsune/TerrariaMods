using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace LifeBoost;

public sealed class LifeBoostConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Header("Life")]

    [Range(1, int.MaxValue)]
    [DefaultValue(100)]
    public int baseLifeAmount = 100;

    [Range(0, int.MaxValue)]
    [DefaultValue(20)]
    public int lifeCrystalAmount = 20;

    [Range(0, int.MaxValue)]
    [DefaultValue(15)]
    public int maxLifeCrystals = 15;

    [Range(0, int.MaxValue)]
    [DefaultValue(5)]
    public int lifeFruitAmount = 5;

    [Range(0, int.MaxValue)]
    [DefaultValue(20)]
    public int maxLifeFruits = 20;

    [DefaultValue(true)]
    public bool lifeFruitRequireMaxLifeCrystals = true;

    [Header("Mana")]

    [Range(0, int.MaxValue)]
    [DefaultValue(20)]
    public int baseManaAmount = 20;

    [Range(0, int.MaxValue)]
    [DefaultValue(20)]
    public int manaCrystalAmount = 20;

    [Range(0, int.MaxValue)]
    [DefaultValue(9)]
    public int maxManaCrystals = 9;

    [Header("Attack")]
    [Range(0, 1000)]
    [DefaultValue(100)]
    public int attackMultiplier = 100;

    [Header("Defense")]
    [Range(0, 1000)]
    [DefaultValue(100)]
    public int defenseMultiplier = 100;

    [Header("Speed")]
    [Range(0, 1000)]
    [DefaultValue(100)]
    public int speedMultiplier = 100;
}