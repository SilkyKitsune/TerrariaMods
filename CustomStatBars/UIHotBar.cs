using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace CustomStatBars;

internal class UIHotBar : UIElement
{
    private sealed class UIHotButton : UICustomButton
    {
        public UIHotButton(int x, int y, int slot, UIHotBar hotBar) : base(x, y)
        {
            this.slot = slot;
            this.hotBar = hotBar;
        }

        private readonly UIHotBar hotBar;

        private readonly int slot;

        private protected override void Click(UIMouseEvent evt, UIElement listeningElement)
        {
            int i = Math.Abs(hotBar.currentSlot - slot);
            if (i != 0)
            {
                hotBar.currentSlot = slot;
                Main.player[Main.myPlayer].ScrollHotbar(i);
            }
        }

        private protected override Asset<Texture2D> GetTexture()
        {
            Item item = Main.player[Main.myPlayer].inventory[slot];
            return (item != null && item.stack > 0) ? TextureAssets.Item[item.type] : TextureAssets.Heart;
        }
    }

    internal UIHotBar(int x, int y, params int[] slots)
    {
        this.x = x;
        this.y = y;
        buttons = new UIHotButton[slots.Length];
        for (int i = 0; i < slots.Length; i++)
            buttons[i] = new(i * UICustomButton.size, 0, slots[i], this);
    }

    private readonly UIHotButton[] buttons;

    private readonly int x, y;

    private int currentSlot = 0;

    internal bool MouseOverHotbar
    {
        get
        {
            foreach (UIHotButton button in buttons)
                if (button.mouseOver) return true;
            return false;
        }
    }

    public override void OnInitialize()
    {
        Left.Set(x, 0f);
        Top.Set(y, 0f);
        Width.Set(UICustomButton.size * buttons.Length, 0f);
        Height.Set(UICustomButton.size, 0f);

        foreach (UIHotButton button in buttons)
        {
            button.Init();
            Append(button);
        }
    }
}