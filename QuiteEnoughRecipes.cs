using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;

namespace QuiteEnoughRecipes;

public class QuiteEnoughRecipes : Mod
{
	public static void DrawItemIcon(Item item, int context, SpriteBatch spriteBatch,
		Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor)
	{
		LoadItemAsync(item.type);
		ItemSlot.DrawItemIcon(item, context, spriteBatch, screenPositionForItemCenter, scale, sizeLimit, environmentColor);
	}

	public static void LoadItemAsync(int i)
	{
		if (TextureAssets.Item[i].State == AssetState.NotLoaded)
		{
			Main.Assets.Request<Texture2D>(TextureAssets.Item[i].Name, AssetRequestMode.AsyncLoad);
		}
	}

	/*
	 * When displaying the name of an ingredient that comes from a mod, this should be appended
	 * immediately after the name so that it's clear what mod the ingredient came from.
	 */
	public static string GetModTagText(Mod mod) => $"   [c/56665e:〈{mod.DisplayNameClean}〉]";
}
