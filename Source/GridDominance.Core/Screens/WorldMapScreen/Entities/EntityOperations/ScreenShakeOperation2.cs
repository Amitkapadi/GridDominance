﻿using Microsoft.Xna.Framework;
using MonoSAMFramework.Portable.Extensions;
using MonoSAMFramework.Portable.GameMath;
using MonoSAMFramework.Portable.GameMath.Geometry;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.UpdateAgents.Impl;

namespace GridDominance.Shared.Screens.WorldMapScreen.Entities.EntityOperations
{
	class ScreenShakeOperation2 : FixTimeOperation<BaseWorldNode>
	{
		public const float SHAKE_OFFSET = 16f;

		private readonly FPoint centeringStartOffset;
		private readonly float rot;
		private readonly GDWorldMapScreen _screen;

		public override string Name => "Node::CenterShake";

		public ScreenShakeOperation2(BaseWorldNode node, GDWorldMapScreen screen) : base(LevelNode.SHAKE_TIME)
		{
			_screen = screen;
			centeringStartOffset = screen.MapViewportCenter;

			rot = FloatMath.RAD_POS_000;
		}

		protected override void OnStart(BaseWorldNode node)
		{
			//
		}

		protected override void OnProgress(BaseWorldNode node, float progress, SAMTime gameTime, InputState istate)
		{
			var off = (Vector2.UnitX * (FloatMath.Sin(progress * FloatMath.TAU * 6) * SHAKE_OFFSET) * (1 - FloatMath.FunctionEaseInCubic(progress))).Rotate(rot);
			var p = FloatMath.FunctionEaseInOutQuad(progress);

			node.Owner.MapViewportCenterX = centeringStartOffset.X + off.X;
			node.Owner.MapViewportCenterY = centeringStartOffset.Y + off.Y;

			if (_screen.IsDragging) Abort();
		}

		protected override void OnEnd(BaseWorldNode node)
		{
			node.Owner.MapViewportCenterX = centeringStartOffset.X;
			node.Owner.MapViewportCenterY = centeringStartOffset.Y;
		}

		protected override void OnAbort(BaseWorldNode node)
		{
			node.Owner.MapViewportCenterX = centeringStartOffset.X;
			node.Owner.MapViewportCenterY = centeringStartOffset.Y;
		}
	}
}
