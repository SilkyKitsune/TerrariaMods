using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CustomStatBars;

internal abstract class UICustomButton : UIElement
{
    internal const int size = 50;
    
    internal UICustomButton(int x, int y)
    {
        this.x = x;
        this.y = y;
        OnMouseUp += Click;//different event?
        OnMouseOver += (evt, listeningElement) => mouseOver = true;
        OnMouseOut += (evt, listeningElement) => mouseOver = false;
    }

    private readonly UIPanel panel = new();
    private readonly UIImageButton button = new(TextureAssets.Beetle);

    private readonly int x, y;

    private bool initialized = false;

    internal bool mouseOver = false;

    private protected abstract void Click(UIMouseEvent evt, UIElement listeningElement);

    private protected abstract Asset<Texture2D> GetTexture();

    public sealed override void Draw(SpriteBatch spriteBatch)
    {
        button.SetImage(GetTexture());
        button.Width.Set(size, 0f);
        button.Height.Set(size, 0f);
        button.Recalculate();
        base.Draw(spriteBatch);
    }

    public override void OnInitialize()
    {
        if (!initialized) Init();
    }

    internal void Init()
    {
        Left.Set(x, 0f);
        Top.Set(y, 0f);
        Width.Set(size, 0f);
        Height.Set(size, 0f);

        panel.Width.Set(size, 0f);
        panel.Height.Set(size, 0f);
        panel.BackgroundColor = new(64, 64, 255);
        Append(panel);

        button.Width.Set(size, 0f);
        button.Height.Set(size, 0f);
        Append(button);

        initialized = true;
    }
}

internal sealed class UIHealButton : UICustomButton
{
    internal static int IsHealPotion(int id)
    {
        switch (id)
        {
            case ItemID.LesserHealingPotion:
                return 60;
            case ItemID.HealingPotion:
                goto case ItemID.LesserHealingPotion;
            case ItemID.GreaterHealingPotion:
                goto case ItemID.LesserHealingPotion;
            case ItemID.SuperHealingPotion:
                goto case ItemID.LesserHealingPotion;
            case ItemID.RestorationPotion:
                return 45;
            default:
                return 0;
        }
    }

    public UIHealButton(int x, int y) : base(x, y) { }

    private protected override void Click(UIMouseEvent evt, UIElement listeningElement)
    {
        Player p = Main.player[Main.myPlayer];
        int i = p.statLifeMax2 - p.statLife;
        if (i > 0 && !p.HasBuff(BuffID.PotionSickness)) foreach (Item item in p.inventory)
                if (item != null && item.stack > 0 && ItemLoader.CanUseItem(item, p))
                {
                    int sickness = IsHealPotion(item.type);
                    if (sickness > 0)
                    {
                        p.statLife = item.healLife > i ? p.statLifeMax2 : p.statLife + item.healLife;
                        item.stack--;
                        p.AddBuff(BuffID.PotionSickness, 60 * sickness);
                        SoundEngine.PlaySound(item.UseSound);
                        return;
                    }
                }
    }

    private protected override Asset<Texture2D> GetTexture() => TextureAssets.Item[ItemID.LesserHealingPotion];
}

internal sealed class UIClearSicknessButton : UICustomButton
{
    public UIClearSicknessButton(int x, int y) : base(x, y) { }

    private protected override void Click(UIMouseEvent evt, UIElement listeningElement) => Main.player[Main.myPlayer].ClearBuff(BuffID.PotionSickness);

    private protected override Asset<Texture2D> GetTexture() => TextureAssets.Buff[BuffID.PotionSickness];
}