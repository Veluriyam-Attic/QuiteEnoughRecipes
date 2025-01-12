using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;

namespace QuiteEnoughRecipes;

/*
 * Displays an NPC. Similar to a bestiary button, but is never locked. Shows the name of the NPC
 * when hovered.
 */
public class UINPCPanel : UIPanel
{
	// This needs to be a child of the panel to handle overflow properly.
	private class UINPCIcon : UIElement
	{
		private int _npcID;
		private bool _isHovering => Parent?.IsMouseHovering ?? false;

		public BestiaryEntry _entry;

		public UINPCIcon(int npcID)
		{
			_npcID = npcID;
			_entry = Main.BestiaryDB.FindEntryByNPCID(npcID);

			OverflowHidden = true;
			IgnoresMouseInteraction = true;
			Width.Percent = 1;
			Height.Percent = 1;
		}

		public override void Update(GameTime t)
		{
			var rect = GetDimensions().ToRectangle();
			var collectionInfo = new BestiaryUICollectionInfo(){
				OwnerEntry = _entry,
				UnlockState = BestiaryEntryUnlockState.CanShowPortraitOnly_1
			};
			_entry.Icon?.Update(collectionInfo, rect,
				new EntryIconDrawSettings(){
					iconbox = rect,
					IsHovered = _isHovering,
					IsPortrait = false
				});
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			var collectionInfo = new BestiaryUICollectionInfo(){
				OwnerEntry = _entry,
				UnlockState = BestiaryEntryUnlockState.CanShowPortraitOnly_1
			};

			_entry.Icon?.Draw(collectionInfo, sb,
				new EntryIconDrawSettings(){
					iconbox = GetDimensions().ToRectangle(),
					IsHovered = _isHovering,
					IsPortrait = false
				});

			if (IsMouseHovering)
			{
				Main.instance.MouseText(
					_entry.Icon?.GetHoverText(collectionInfo) ?? Lang.GetNPCNameValue(_npcID));
			}
		}
	}

	UINPCIcon _icon;

	public UINPCPanel(int npcID)
	{
		_icon = new(npcID);
	}

	public override void OnInitialize()
	{
		Append(_icon);
	}

	protected override void DrawSelf(SpriteBatch sb)
	{
		base.DrawSelf(sb);

		var collectionInfo = new BestiaryUICollectionInfo(){
			OwnerEntry = _icon._entry,
			UnlockState = BestiaryEntryUnlockState.CanShowPortraitOnly_1
		};

		if (IsMouseHovering)
		{
			Main.instance.MouseText(_icon._entry.Icon.GetHoverText(collectionInfo));
		}
	}
}
