﻿using Microsoft.Xna.Framework;
using GridDominance.Shared.Resources;
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
	public class OverworldNode_MP : OverworldNode
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

		private float _pulseTimer = FloatMath.PI * 0.25f;
		
		public OverworldNode_MP(GDOverworldScreen scrn, FPoint pos) : base(scrn, pos, L10NImpl.STR_WORLD_MULTIPLAYER, Levels.WORLD_ID_MULTIPLAYER)
		{
			AddEntityOperationDelayed(new NetworkAnimationTriggerOperation(), NetworkAnimationTriggerOperation.INITIAL_DELAY);
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

			var scoreRectSize = innerBounds.Width / 8f;
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					var bc = ((x % 2 == 0) ^ (y % 2 == 0)) ? FlatColors.Background : FlatColors.BackgroundLight;

					if (IsUnlocked())
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

			sbatch.DrawStretched(Textures.TexIconNetworkBase,    innerBounds, Color.White);
			sbatch.DrawStretched(Textures.TexIconNetworkVertex1, innerBounds, Color.White, VertexRotations[0]);
			sbatch.DrawStretched(Textures.TexIconNetworkVertex2, innerBounds, Color.White, VertexRotations[1]);
			sbatch.DrawStretched(Textures.TexIconNetworkVertex3, innerBounds, Color.White, VertexRotations[2]);
			sbatch.DrawStretched(Textures.TexIconNetworkVertex4, innerBounds, Color.White, VertexRotations[3]);
			sbatch.DrawStretched(Textures.TexIconNetworkVertex5, innerBounds, Color.White, VertexRotations[4]);
			
			sbatch.DrawRectangle(innerBounds, Color.Black, Owner.PixelWidth);
			
			FontRenderHelper.DrawTextCentered(sbatch, Textures.HUDFontBold, 0.9f * GDConstants.TILE_WIDTH, L10N.T(_l10ndescription), FlatColors.TextHUD, Position + new Vector2(0, 2.25f * GDConstants.TILE_WIDTH));

		}

		protected override void OnClick(GameEntityMouseArea area, SAMTime gameTime, InputState istate)
		{
			var ownr = ((GDOverworldScreen)Owner);
			if (ownr.IsTransitioning) return;
			
			Owner.HUD.AddModal(new MultiplayerMainPanel(), true, 0.5f, 1f);
		}

		private bool IsUnlocked()
		{
			if (GDConstants.USE_IAB)
			{
				// LIGHT VERSION

				var ip = MainGame.Inst.Bridge.IAB.IsPurchased(GDConstants.IAB_MULTIPLAYER);

				if (ip == PurchaseQueryResult.Refunded)
				{
					if (MainGame.Inst.Profile.PurchasedWorlds.Contains(Levels.WORLD_ID_MULTIPLAYER))
					{
						SAMLog.Debug("Level refunded: " + Levels.WORLD_ID_MULTIPLAYER);
						MainGame.Inst.Profile.PurchasedWorlds.Remove(Levels.WORLD_ID_MULTIPLAYER);
						MainGame.Inst.SaveProfile();
					}
					return false;
				}

				if (MainGame.Inst.Profile.PurchasedWorlds.Contains(Levels.WORLD_ID_MULTIPLAYER)) return true;

				switch (ip)
				{
					case PurchaseQueryResult.Purchased:
						MainGame.Inst.Profile.PurchasedWorlds.Add(Levels.WORLD_ID_MULTIPLAYER);
						MainGame.Inst.SaveProfile();
						return true;

					case PurchaseQueryResult.NotPurchased:
					case PurchaseQueryResult.Cancelled:
						return false;

					case PurchaseQueryResult.Error:
						Owner.HUD.ShowToast(L10N.T(L10NImpl.STR_IAB_TESTERR), 40, FlatColors.Pomegranate, FlatColors.Foreground, 2.5f);
						return false;

					case PurchaseQueryResult.Refunded:
						if (MainGame.Inst.Profile.PurchasedWorlds.Contains(Levels.WORLD_ID_MULTIPLAYER))
						{
							SAMLog.Debug("Level refunded: " + Levels.WORLD_ID_MULTIPLAYER);
							MainGame.Inst.Profile.PurchasedWorlds.Remove(Levels.WORLD_ID_MULTIPLAYER);
							MainGame.Inst.SaveProfile();
						}
						return false;

					case PurchaseQueryResult.NotConnected:
						Owner.HUD.ShowToast(L10N.T(L10NImpl.STR_IAB_TESTNOCONN), 40, FlatColors.Pomegranate, FlatColors.Foreground, 2.5f);
						return false;

					case PurchaseQueryResult.CurrentlyInitializing:
						Owner.HUD.ShowToast(L10N.T(L10NImpl.STR_IAB_TESTINPROGRESS), 40, FlatColors.Pomegranate, FlatColors.Foreground, 2.5f);
						return false;

					default:
						SAMLog.Error("ONMP::EnumSwitch-IU_MP", "IsUnlocked()", "MainGame.Inst.Bridge.IAB.IsPurchased(MainGame.IAB_MULTIPLAYER)) -> " + ip);
						return false;
				}
			}
			else
			{
				// FULL VERSION

				return true;
			}
		}
	}
}
