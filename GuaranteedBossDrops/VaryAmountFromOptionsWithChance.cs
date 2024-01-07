using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;

namespace GuaranteedBossDrops;

internal sealed class VaryAmountFromOptionsWithChance : IItemDropRule
{
    internal VaryAmountFromOptionsWithChance(int denominator, int numerator, int minimum, int maximum, IItemDropRuleCondition condition = null, params int[] dropIDs)
    {
        this.denominator = denominator;
        this.numerator = numerator;
        this.minimum = minimum;
        this.maximum = maximum;
        this.condition = condition;
        this.dropIDs = dropIDs;
    }

    private readonly int denominator, numerator, minimum, maximum;
    private readonly int[] dropIDs;
    private readonly IItemDropRuleCondition condition;
    private readonly List<IItemDropRuleChainAttempt> chainedRules = new();

    public List<IItemDropRuleChainAttempt> ChainedRules => chainedRules;

    public bool CanDrop(DropAttemptInfo info) => condition == null || condition.CanDrop(info);

    public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
    {
        float rate = (float)numerator / denominator;
        List<IItemDropRuleCondition> conditions = condition == null ? null : new(new IItemDropRuleCondition[] { condition });
        foreach (int id in dropIDs) drops.Add(new(id, minimum, maximum, rate, conditions));
    }

    public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
    {
        ItemDropAttemptResultState state = info.player.RollLuck(denominator) < numerator ?
            ItemDropAttemptResultState.Success : ItemDropAttemptResultState.FailedRandomRoll;

        if (state == ItemDropAttemptResultState.Success)
            CommonCode.DropItem(info, dropIDs[info.rng.Next(dropIDs.Length)],
                info.rng.Next(minimum, maximum + 1));

        return new() { State = state };
    }
}