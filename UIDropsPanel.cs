using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;

namespace QuiteEnoughRecipes;

/*
 * Displays an element on the left with a grid of dropped items on the right. The offset of the
 * grid is based on the `width` value of the left element.
 */
public class UIDropsPanel : UIAutoExtend
{
	public UIDropsPanel(UIElement left, List<DropRateInfo> drops)
	{
		Width.Percent = 1;

		var grid = new UIAutoExtendGrid(){
			Width = new StyleDimension(-left.Width.Pixels - 10, 1 - left.Width.Percent),
			HAlign = 1
		};

		// These are in reverse order of probability, with smaller stacks listed first.
		foreach (var drop in drops.OrderBy(d => (1 - d.dropRate, d.stackMax)))
		{
			grid.Append(new UILootItemPanel(drop));
		}

		Append(left);
		Append(grid);
	}
}

file class UILootItemPanel : UIItemPanel
{
	private int _stackMin;
	private int _stackMax;
	private float _chance;
	private string? _conditions = null;

	public UILootItemPanel(DropRateInfo info) : base(new(info.itemId))
	{
		_stackMin = info.stackMin;
		_stackMax = info.stackMax;
		_chance = info.dropRate;

		var conditionDescs = info.conditions
			?.Select(c => c.GetConditionDescription())
			?.Where(d => !string.IsNullOrWhiteSpace(d));
		if (conditionDescs != null)
		{
			_conditions = string.Join("\n", conditionDescs);
		}
	}

	public override void ModifyTooltips(Mod mod, List<TooltipLine> tooltips)
	{
		base.ModifyTooltips(mod, tooltips);
		if (!string.IsNullOrWhiteSpace(_conditions))
		{
			tooltips.Add(new(mod, "QER: drop conditions", _conditions){
				OverrideColor = Main.OurFavoriteColor
			});
		}
	}

	protected override void DrawOverlayText(SpriteBatch sb)
	{
		if (_stackMax > 1 || _stackMin != _stackMax)
		{
			var text = _stackMin == _stackMax ? _stackMin.ToString() : $"{_stackMin}–{_stackMax}";
			DrawText(sb, text, new Vector2(10, 26));
		}

		var percentPos = new Vector2(25, 3);

		if (_chance < 0.00001f)
		{
			DrawText(sb, $"<{0.00001f:p3}", percentPos);
		}
		else if (_chance < 0.0001f)
		{
			DrawText(sb, $"{_chance:p3}", percentPos);
		}
		else if (_chance < 0.9999f)
		{
			DrawText(sb, $"{_chance:p2}", percentPos);
		}
	}
}
