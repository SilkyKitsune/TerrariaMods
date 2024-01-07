using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace CustomStatBars;

internal sealed class UIPercentBar : UIElement
{
    internal UIPercentBar(int x, int y, int width, int height, Color backColor, Color frontColor)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.backColor = backColor;
        this.frontColor = frontColor;
    }

    private readonly UIPanel back = new(), front = new();
    private readonly UIText text = new("1 / 1");

    private readonly Color backColor, frontColor;

    private int x, y, width, height, divisor = 1, dividend = 1;

    public override void OnInitialize()
    {
        back.BackgroundColor = backColor;
        front.BackgroundColor = frontColor;

        SetRegion(x, y, width, height);

        Append(back);
        Append(front);
        Append(text);
    }

    internal void SetRegion(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;

        Left.Set(x, 0f);
        Top.Set(y, 0f);
        Width.Set(width, 0f);
        Height.Set(height, 0f);

        back.Width.Set(width, 0f);
        back.Height.Set(height, 0f);

        front.Height.Set(height, 0f);
        SetValue(divisor, dividend);

        text.Width.Set(width, 0f);
        text.Height.Set(height, 0f);

        back.Recalculate();
        text.Recalculate();
        Recalculate();
    }

    internal void SetValue(int divisor, int dividend)
    {
        if (dividend <= 0) dividend = 1;

        this.divisor = divisor;
        this.dividend = dividend;

        float f = (float)divisor / dividend;

        text.SetText($"{divisor} / {dividend} ({(int)(f * 100f)}%)");

        front.Width.Set(f * width, 0f);
        front.Recalculate();
    }
}