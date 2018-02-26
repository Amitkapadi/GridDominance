﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GridDominance.Shared.Resources;
using GridDominance.Shared.Screens.NormalGameScreen.Fractions;
using GridDominance.Shared.SCCM;
using Microsoft.Xna.Framework;
using MonoSAMFramework.Portable.ColorHelper;
using MonoSAMFramework.Portable.GameMath.Geometry;
using MonoSAMFramework.Portable.RenderHelper;
using MonoSAMFramework.Portable.Screens.HUD.Elements.Button;
using MonoSAMFramework.Portable.Screens.HUD.Elements.Container;
using MonoSAMFramework.Portable.Screens.HUD.Elements.Primitives;
using MonoSAMFramework.Portable.Screens.HUD.Enums;

namespace GridDominance.Shared.Screens.OverworldScreen.HUD.SCCM.Dialogs
{
	public class SCCMLevelPreviewDialog : HUDRoundedPanel
	{
		private const float TW = GDConstants.TILE_WIDTH;

		public const float WIDTH = 14 * TW;
		public const float HEIGHT = 8 * TW;

		public override int Depth => 0;

		private readonly SCCMLevelMeta _meta;

		public SCCMLevelPreviewDialog(SCCMLevelMeta meta)
		{
			_meta = meta;
			
			RelativePosition = FPoint.Zero;
			Size = new FSize(WIDTH, HEIGHT);
			Alignment = HUDAlignment.CENTER;
			Background = FlatColors.BackgroundHUD;
		}

		public override void OnInitialize()
		{
			#region Header
			
			AddElement(new HUDRectangle(-99)
			{
				Alignment = HUDAlignment.TOPCENTER,
				RelativePosition = FPoint.Zero,
				Size = new FSize(WIDTH, 1.40f*TW),

				Definition = HUDBackgroundDefinition.CreateRounded(FlatColors.BackgroundHUD.Darken(0.9f), 16, true, true, false, false),
			});

			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTER,
				Alignment = HUDAlignment.TOPCENTER,
				RelativePosition = new FPoint(0, 0),
				Size = new FSize(WIDTH, 1.40f*TW),

				Font = Textures.HUDFontBold,
				FontSize = TW,

				Text = _meta.LevelName,
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = Color.White,
			});
			
			AddElement(new HUDEllipseImageButton
			{
				Alignment = HUDAlignment.TOPRIGHT,
				RelativePosition = new FPoint(0, 0),
				Size = new FSize(64, 64),

				Image = Textures.TexIconStar,
				BackgroundNormal   = Color.Transparent,
				BackgroundPressed  = FlatColors.ButtonPressedHUD,
				ImageColor         = MainGame.Inst.Profile.HasCustomLevelStarred(_meta) ? FlatColors.SunFlower : FlatColors.Silver,
				ImageAlignment     = HUDImageAlignment.SCALE,

				Click = (s, a) => ToggleStar(),
			});
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.TOPRIGHT,
				Alignment = HUDAlignment.TOPRIGHT,
				RelativePosition = new FPoint(15, 55),
				Size = new FSize(128, 32),

				Font = Textures.HUDFontRegular,
				FontSize = 24,

				Text = _meta.Stars.ToString(),
				WordWrap = HUDWordWrap.NoWrap,
				TextColor = FlatColors.SunFlower,
			});

			#endregion

			#region Tab Header

			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.BOTTOMLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(1*TW, 1*TW),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontBold,
				FontSize = 32,

				L10NText = L10NImpl.STR_INF_YOU,
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = FlatColors.Clouds,
			});

			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.BOTTOMLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(4*TW, 1*TW),
				Size = new FSize(6*TW, TW),

				Font = Textures.HUDFontBold,
				FontSize = 32,

				L10NText = L10NImpl.STR_INF_HIGHSCORE,
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = FlatColors.Clouds,
			});

			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.BOTTOMLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(10*TW, 1*TW),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontBold,
				FontSize = 32,

				L10NText = L10NImpl.STR_INF_CLEARS,
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = FlatColors.Clouds,
			});

			#endregion

			#region Tab Col Images

			AddElement(new HUDImage
			{
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(8, 2*TW + 16 + (48+16)*0 + 8),
				Size = new FSize(48, 48),

				Image = Textures.TexDifficultyLine0,
				Color = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_0) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_0) : FlatColors.Silver,
				ImageAlignment = HUDImageAlignment.SCALE,
			});

			AddElement(new HUDImage
			{
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(8, 2*TW + 16 + (48+16)*1 + 8),
				Size = new FSize(48, 48),

				Image = Textures.TexDifficultyLine1,
				Color = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_1) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_1) : FlatColors.Silver,
				ImageAlignment = HUDImageAlignment.SCALE,
			});

			AddElement(new HUDImage
			{
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(8, 2*TW + 16 + (48+16)*2 + 8),
				Size = new FSize(48, 48),

				Image = Textures.TexDifficultyLine2,
				Color = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_2) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_2) : FlatColors.Silver,
				ImageAlignment = HUDImageAlignment.SCALE,
			});

			AddElement(new HUDImage
			{
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(8, 2*TW + 16 + (48+16)*3 + 8),
				Size = new FSize(48, 48),

				Image = Textures.TexDifficultyLine3,
				Color = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_3) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_3) : FlatColors.Silver,
				ImageAlignment = HUDImageAlignment.SCALE,
			});

			#endregion

			#region Tab Col 1

			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(1*TW, 2*TW + 16 + (48+16)*0),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = MainGame.Inst.Profile.GetCustomLevelTimeString(_meta, FractionDifficulty.DIFF_0),
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_0) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_0) : FlatColors.TextHUD,
			});

			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(1*TW, 2*TW + 16 + (48+16)*1),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = MainGame.Inst.Profile.GetCustomLevelTimeString(_meta, FractionDifficulty.DIFF_1),
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_1) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_1) : FlatColors.TextHUD,
			});

			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(1*TW, 2*TW + 16 + (48+16)*2),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = MainGame.Inst.Profile.GetCustomLevelTimeString(_meta, FractionDifficulty.DIFF_2),
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_2) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_2) : FlatColors.TextHUD,
			});

			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(1*TW, 2*TW + 16 + (48+16)*3),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = MainGame.Inst.Profile.GetCustomLevelTimeString(_meta, FractionDifficulty.DIFF_3),
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_3) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_3) : FlatColors.TextHUD,
			});
			
			#endregion

			#region Tab Col 2
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(4*TW, 2*TW + 16 + (48+16)*0),
				Size = new FSize(6*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Highscores[(int)FractionDifficulty.DIFF_0].FormatHighscoreCell(),
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_0) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_0) : FlatColors.TextHUD,
			});
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(4*TW, 2*TW + 16 + (48+16)*1),
				Size = new FSize(6*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Highscores[(int)FractionDifficulty.DIFF_1].FormatHighscoreCell(),
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_1) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_1) : FlatColors.TextHUD,
			});
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(4*TW, 2*TW + 16 + (48+16)*2),
				Size = new FSize(6*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Highscores[(int)FractionDifficulty.DIFF_2].FormatHighscoreCell(),
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_2) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_2) : FlatColors.TextHUD,
			});
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(4*TW, 2*TW + 16 + (48+16)*3),
				Size = new FSize(6*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Highscores[(int)FractionDifficulty.DIFF_3].FormatHighscoreCell(),
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_3) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_3) : FlatColors.TextHUD,
			});
			
			#endregion

			#region Tab Col 3
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(10*TW, 2*TW + 16 + (48+16)*0),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Highscores[(int)FractionDifficulty.DIFF_0].FormatGlobalClearsCell(),
				WordWrap = HUDWordWrap.NoWrap,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_0) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_0) : FlatColors.TextHUD,
			});
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(10*TW, 2*TW + 16 + (48+16)*1),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Highscores[(int)FractionDifficulty.DIFF_1].FormatGlobalClearsCell(),
				WordWrap = HUDWordWrap.NoWrap,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_1) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_1) : FlatColors.TextHUD,
			});
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(10*TW, 2*TW + 16 + (48+16)*2),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Highscores[(int)FractionDifficulty.DIFF_2].FormatGlobalClearsCell(),
				WordWrap = HUDWordWrap.NoWrap,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_2) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_2) : FlatColors.TextHUD,
			});
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.CENTERLEFT,
				Alignment = HUDAlignment.TOPLEFT,
				RelativePosition = new FPoint(10*TW, 2*TW + 16 + (48+16)*3),
				Size = new FSize(3*TW, TW),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Highscores[(int)FractionDifficulty.DIFF_3].FormatGlobalClearsCell(),
				WordWrap = HUDWordWrap.NoWrap,
				TextColor = MainGame.Inst.Profile.HasCustomLevelBeaten(_meta, FractionDifficulty.DIFF_3) ? FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_3) : FlatColors.TextHUD,
			});
			
			#endregion
			
			#region Footer

			AddElement(new HUDRectangle
			{
				Alignment = HUDAlignment.BOTTOMCENTER,
				RelativePosition = FPoint.Zero,
				Size = new FSize(WIDTH, 1.5f*TW),

				Definition = HUDBackgroundDefinition.CreateRounded(FlatColors.BackgroundHUD2, 16, false, false, true, true),
			});

			AddElement(new HUDSeperator(HUDOrientation.Horizontal, 3)
			{
				Alignment = HUDAlignment.BOTTOMCENTER,
				RelativePosition = new FPoint(0, 1.5f*TW),
				Size = new FSize(WIDTH, HUD.PixelWidth),

				Color = FlatColors.SeperatorHUD,
			});

			AddElement(new HUDEllipseImageButton
			{
				Alignment = HUDAlignment.BOTTOMCENTER,
				RelativePosition = new FPoint(-84/2 - 16 - 84 - 32, 6),
				Size = new FSize(84, 84),

				Image = Textures.TexDifficultyLine0,
				BackgroundNormal   = FlatColors.ButtonHUD,
				BackgroundPressed = FlatColors.ButtonPressedHUD,
				ImageColor = FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_0),
				ImageAlignment = HUDImageAlignment.SCALE,

				Click = (s, a) => Play(FractionDifficulty.DIFF_0),
			});
			
			AddElement(new HUDEllipseImageButton
			{
				Alignment = HUDAlignment.BOTTOMCENTER,
				RelativePosition = new FPoint(-84/2 - 16, 6),
				Size = new FSize(84, 84),

				Image = Textures.TexDifficultyLine1,
				BackgroundNormal   = FlatColors.ButtonHUD,
				BackgroundPressed = FlatColors.ButtonPressedHUD,
				ImageColor = FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_1),
				ImageAlignment = HUDImageAlignment.SCALE,

				Click = (s, a) => Play(FractionDifficulty.DIFF_1),
			});
			
			AddElement(new HUDEllipseImageButton
			{
				Alignment = HUDAlignment.BOTTOMCENTER,
				RelativePosition = new FPoint(+84/2 + 16, 6),
				Size = new FSize(84, 84),

				Image = Textures.TexDifficultyLine2,
				BackgroundNormal   = FlatColors.ButtonHUD,
				BackgroundPressed = FlatColors.ButtonPressedHUD,
				ImageColor = FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_2),
				ImageAlignment = HUDImageAlignment.SCALE,

				Click = (s, a) => Play(FractionDifficulty.DIFF_2),
			});
			
			AddElement(new HUDEllipseImageButton
			{
				Alignment = HUDAlignment.BOTTOMCENTER,
				RelativePosition = new FPoint(+84/2 + 16 + 84 + 32, 6),
				Size = new FSize(84, 84),

				Image = Textures.TexDifficultyLine3,
				BackgroundNormal   = FlatColors.ButtonHUD,
				BackgroundPressed = FlatColors.ButtonPressedHUD,
				ImageColor = FractionDifficultyHelper.GetColor(FractionDifficulty.DIFF_3),
				ImageAlignment = HUDImageAlignment.SCALE,

				Click = (s, a) => Play(FractionDifficulty.DIFF_3),
			});
			
			AddElement(new HUDLabel
			{
				TextAlignment = HUDAlignment.BOTTOMRIGHT,
				Alignment = HUDAlignment.BOTTOMRIGHT,
				RelativePosition = new FPoint(4, 4),
				Size = new FSize(236, 32),

				Font = Textures.HUDFontRegular,
				FontSize = 32,

				Text = _meta.Username,
				WordWrap = HUDWordWrap.Ellipsis,
				TextColor = FlatColors.GreenSea,
			});

			#endregion

		}

		private void ToggleStar()
		{
			//TODO
		}

		private void Play(FractionDifficulty d)
		{
			//TODO
		}
	}
}
