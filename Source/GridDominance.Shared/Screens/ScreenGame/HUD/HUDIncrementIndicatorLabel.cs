﻿using GridDominance.Shared.Screens.ScreenGame.HUDOperations;
using MonoSAMFramework.Portable.GameMath.Geometry;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.Screens.HUD.Elements.Primitives;
using MonoSAMFramework.Portable.Screens.HUD.Enums;

namespace GridDominance.Shared.Screens.ScreenGame.HUD
{
	public class HUDIncrementIndicatorLabel : HUDLabel
	{
		private readonly HUDRawText incrementLabel;

		public float AnimationOffset = 0f;

		public HUDIncrementIndicatorLabel(string value, string increment, int depth = 0)
			: base(depth)
		{
			Text = value;

			incrementLabel = new HUDRawText
			{
				Text = increment,
			};

			AddHUDOperation(new HUDIncrementIndicatorLabelWiggleOperation());
		}
		
		public override void OnInitialize()
		{
			base.OnInitialize();

			incrementLabel.Alignment = HUDAlignment.ABSOLUTE;
			incrementLabel.FontSize = FontSize / 2;
			incrementLabel.Font = Font;
			incrementLabel.TextColor = TextColor;

			AddElement(incrementLabel);
		}

		protected override void DoUpdate(SAMTime gameTime, InputState istate)
		{
			base.DoUpdate(gameTime, istate);

			var innerRel = internalText.Position + new FPoint(InnerLabelSize.Width + 5, AnimationOffset - incrementLabel.Height / 4);

			incrementLabel.RelativePosition = innerRel;
		}
	}
}
