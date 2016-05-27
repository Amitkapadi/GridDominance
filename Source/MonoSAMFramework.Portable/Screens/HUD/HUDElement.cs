﻿#if DEBUG
//#define DEBUG_HUDBOUNDS
#endif

using Microsoft.Xna.Framework;
using MonoSAMFramework.Portable.BatchRenderer;
using MonoSAMFramework.Portable.DebugTools;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.Interfaces;
using MonoSAMFramework.Portable.MathHelper;
using System;
using MonoSAMFramework.Portable.MathHelper.FloatClasses;

namespace MonoSAMFramework.Portable.Screens.HUD
{
	public abstract class HUDElement : ISAMLayeredDrawable, ISAMUpdateable
	{
		public HUDContainer Owner = null; // Only set on add to HUD (the OnInitialize is called)
		public GameHUD HUD = null; // Only set on add to HUD (the OnInitialize is called)
		public bool Alive = true;
		public bool Initialized { get; private set; } = false;

		public virtual int DeepInclusiveCount => 1;

		private FPoint _relativePosition = FPoint.Zero;
		public FPoint RelativePosition
		{
			get { return _relativePosition;}
			set { _relativePosition = value; InvalidatePosition(); }
		}

		private FSize _size = FSize.Empty;
		public FSize Size
		{
			get { return _size; }
			set { _size = value; InvalidatePosition(); }
		}

		private HUDAlignment _alignment = HUDAlignment.TOPLEFT;
		public HUDAlignment Alignment
		{
			get { return _alignment; }
			set { _alignment = value; InvalidatePosition(); }
		}

		public FPoint Position { get; protected set; } = FPoint.Zero;
		public FRectangle BoundingRectangle { get; protected set; } = FRectangle.Empty;

		public Vector2 RelativeCenter
		{
			get { return new Vector2(RelativePosition.X + Size.Width / 2f, RelativePosition.Y + Size.Height / 2f);}
			set { RelativePosition = new FPoint(value.X - Size.Width / 2f, value.Y - Size.Height / 2f);}
		}

		public float Top => BoundingRectangle.Top;
		public float Left => BoundingRectangle.Left;

		public float Bottom => BoundingRectangle.Bottom;
		public float Right => BoundingRectangle.Right;

		public float Width => BoundingRectangle.Width;
		public float Height => BoundingRectangle.Height;

		public float CenterX => Position.X + Size.Width / 2f;
		public float CenterY => Position.Y + Size.Height / 2f;

		public Vector2 Center => new Vector2(CenterX, CenterY);

		protected bool IsPointerDownOnElement = false;
		protected bool PositionInvalidated = false;

		public abstract int Depth { get; }

		protected HUDElement()
		{
			//
		}

		public void Remove()
		{
			Alive = false;
		}

		public void Initialize()
		{
			Initialized = true;

			InvalidatePosition();

			OnInitialize();
		}

		public virtual void DrawBackground(IBatchRenderer sbatch)
		{
			DoDrawBackground(sbatch, BoundingRectangle);
		}

		public virtual void DrawForeground(IBatchRenderer sbatch)
		{
			DoDraw(sbatch, BoundingRectangle);

#if DEBUG
			if (DebugSettings.Get("DebugHUDBorders"))
			{
				DrawDebugHUDBorders(sbatch);
			}
#endif
		}

		protected abstract void DoDraw(IBatchRenderer sbatch, FRectangle bounds);

		protected virtual void DoDrawBackground(IBatchRenderer sbatch, FRectangle bounds)
		{
			/* OVERRIDE ME */
		}

		protected virtual void DrawDebugHUDBorders(IBatchRenderer sbatch)
		{
			sbatch.DrawRectangle(BoundingRectangle, Color.Magenta, 2f);
		}

		public virtual void Update(GameTime gameTime, InputState istate)
		{
			if (PositionInvalidated) RecalculatePosition();


			if (istate.IsJustDown && BoundingRectangle.Contains(istate.PointerPosition))
			{
				OnPointerDown(istate.PointerPosition - Position, istate);
				IsPointerDownOnElement = true;
			}
			else if (istate.IsJustUp && BoundingRectangle.Contains(istate.PointerPosition))
			{
				OnPointerUp(istate.PointerPosition - Position, istate);

				if (IsPointerDownOnElement)
					OnPointerClick(istate.PointerPosition - Position, istate);
			}

			if (!istate.IsDown) IsPointerDownOnElement = false;

			DoUpdate(gameTime, istate);
		}

		public virtual void InvalidatePosition()
		{
			PositionInvalidated = true;
		}

		protected virtual void RecalculatePosition()
		{
			if (Owner == null) return;

			OnBeforeRecalculatePosition();

			float px;
			float py;

			switch (Alignment)
			{
				case HUDAlignment.TOPLEFT:
				case HUDAlignment.BOTTOMLEFT:
				case HUDAlignment.CENTERLEFT:
					px = Owner.Left + RelativePosition.X;
					break;
				case HUDAlignment.TOPRIGHT:
				case HUDAlignment.BOTTOMRIGHT:
				case HUDAlignment.CENTERRIGHT:
					px = Owner.Right - Size.Width - RelativePosition.X;
					break;
				case HUDAlignment.TOPCENTER:
				case HUDAlignment.BOTTOMCENTER:
				case HUDAlignment.CENTER:
					px = Owner.CenterX - Size.Width / 2 + RelativePosition.X;
					break;
				case HUDAlignment.ABSOLUTE:
					px = RelativePosition.X;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (Alignment)
			{
				case HUDAlignment.TOPLEFT:
				case HUDAlignment.TOPRIGHT:
				case HUDAlignment.TOPCENTER:
					py = Owner.Top + RelativePosition.Y;
					break;
				case HUDAlignment.BOTTOMLEFT:
				case HUDAlignment.BOTTOMRIGHT:
				case HUDAlignment.BOTTOMCENTER:
					py = Owner.Bottom - Size.Height - RelativePosition.Y;
					break;
				case HUDAlignment.CENTERLEFT:
				case HUDAlignment.CENTERRIGHT:
				case HUDAlignment.CENTER:
					py = Owner.CenterY - Size.Height / 2 + RelativePosition.Y;
					break;
				case HUDAlignment.ABSOLUTE:
					py = RelativePosition.Y;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			Position = new FPoint(px, py);

			BoundingRectangle = new FRectangle(Position, Size);

			PositionInvalidated = false;

			OnAfterRecalculatePosition();
		}

		public abstract void OnInitialize();
		public abstract void OnRemove();     // Only called on manual remove - not on HUD/Screen remove

		protected abstract void DoUpdate(GameTime gameTime, InputState istate);

		protected virtual void OnBeforeRecalculatePosition()
		{
			/* OVERRIDE ME */
		}

		protected virtual void OnAfterRecalculatePosition()
		{
			/* OVERRIDE ME */
		}

		protected virtual void OnPointerUp(FPoint relPositionPoint, InputState istate)
		{
			/* OVERRIDE ME */
		}

		protected virtual void OnPointerDown(FPoint relPositionPoint, InputState istate)
		{
			/* OVERRIDE ME */
		}

		protected virtual void OnPointerClick(FPoint relPositionPoint, InputState istate)
		{
			/* OVERRIDE ME */
		}
	}
}
