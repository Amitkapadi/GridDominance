﻿using Microsoft.Xna.Framework;
using GridDominance.Shared.Resources;
using GridDominance.Shared.Screens.Common;
using GridDominance.Shared.Screens.OverworldScreen.Entities.EntityOperations;
using GridDominance.Shared.Screens.OverworldScreen.HUD;
using MonoSAMFramework.Portable.BatchRenderer;
using MonoSAMFramework.Portable.Screens.Entities.MouseArea;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.Localization;
using MonoSAMFramework.Portable.ColorHelper;
using MonoSAMFramework.Portable.DeviceBridge;
using MonoSAMFramework.Portable.GameMath;
using MonoSAMFramework.Portable.GameMath.Geometry;
using MonoSAMFramework.Portable.LogProtocol;
using MonoSAMFramework.Portable.RenderHelper;

// ReSharper disable HeuristicUnreachableCode
#pragma warning disable 162
namespace GridDominance.Shared.Screens.OverworldScreen.Entities
{
	public class OverworldNode_SCCM : OverworldNode
	{
		public readonly float[] VertexRotations =
		{
			FloatMath.RAD_POS_315,
			FloatMath.RAD_POS_045,
			FloatMath.RAD_POS_090,
			FloatMath.RAD_POS_135,
			FloatMath.RAD_POS_225,
		};
		
		public override bool IsNodeEnabled => true;

		private float _pulseTimer = 0;

		private readonly WorldUnlockState _ustate;

		public OverworldNode_SCCM(GDOverworldScreen scrn, FPoint pos) : base(scrn, pos, L10NImpl.STR_WORLD_ONLINE, Levels.WORLD_ID_ONLINE)
		{
			//AddEntityOperationDelayed(new NetworkAnimationTriggerOperation(), NetworkAnimationTriggerOperation.INITIAL_DELAY);

			_ustate = UnlockManager.IsUnlocked(Levels.WORLD_ID_MULTIPLAYER, false);
		}

		protected override void OnUpdate(SAMTime gameTime, InputState istate)
		{
			base.OnUpdate(gameTime, istate);

			_pulseTimer += gameTime.ElapsedSeconds;
		}

		protected override void OnDraw(IBatchRenderer sbatch)
		{
			var outerBounds = FRectangle.CreateByCenter(Position, DrawingBoundingBox);
			var innerBounds = FRectangle.CreateByCenter(Position, new FSize(INNERSIZE, INNERSIZE));

			FlatRenderHelper.DrawRoundedBlurPanel(sbatch, outerBounds, clickArea.IsMouseDown() ? FlatColors.ButtonPressedHUD : FlatColors.Asbestos, 0.5f * GDConstants.TILE_WIDTH);
			SimpleRenderHelper.DrawRoundedRectOutline(sbatch, outerBounds.AsInflated(1f, 1f), FlatColors.MidnightBlue, 8, 2f, 0.5f * GDConstants.TILE_WIDTH);

			sbatch.FillRectangle(innerBounds, FlatColors.Background);

			var scoreRectSize = innerBounds.Width / 10f;
			for (int x = 0; x < 10; x++)
			{
				for (int y = 0; y < 10; y++)
				{
					var bc = ((x % 2 == 0) ^ (y % 2 == 0)) ? FlatColors.Background : FlatColors.BackgroundLight;

					if (_ustate == WorldUnlockState.OpenAndUnlocked)
					{
						var d = FloatMath.Sqrt((x - 3.5f) * (x - 3.5f) + (y - 3.5f) * (y - 3.5f));

						var p = 1 - (d / 4.5f);
						if (p < 0) p = 0;

						p *= FloatMath.PercSin(_pulseTimer * FloatMath.TAU * 0.25f);

						bc = ColorMath.Blend(bc, FlatColors.PeterRiver, p);
					}

					var col = ColorMath.Blend(FlatColors.Background, bc, AlphaOverride);
					sbatch.FillRectangle(new FRectangle(innerBounds.X + scoreRectSize * x, innerBounds.Y + scoreRectSize * y, scoreRectSize, scoreRectSize), col);
				}
			}

			//TODO

			sbatch.DrawRectangle(innerBounds, Color.Black, Owner.PixelWidth);
			
			FontRenderHelper.DrawTextCentered(sbatch, Textures.HUDFontBold, 0.9f * GDConstants.TILE_WIDTH, L10N.T(_l10ndescription), FlatColors.TextHUD, Position + new Vector2(0, 2.25f * GDConstants.TILE_WIDTH));
		}

		protected override void OnClick(GameEntityMouseArea area, SAMTime gameTime, InputState istate)
		{
			var ownr = ((GDOverworldScreen)Owner);
			if (ownr.IsTransitioning) return;
			
			//TODO
		}
	}
}
