using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LifeBoost;

public sealed class VanillaBuffs : GlobalBuff
{
    public override void Update(int type, Player player, ref int buffIndex)
    {
        if (type == BuffID.Lifeforce && player.TryGetModPlayer(out LifeBoostPlayer modPlayer))
            modPlayer.lifeMultiplier += 0.2f;
    }
}