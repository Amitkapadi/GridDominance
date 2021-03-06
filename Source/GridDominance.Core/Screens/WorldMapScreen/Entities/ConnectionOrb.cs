﻿using GridDominance.Shared.Resources;
using GridDominance.Shared.Screens.NormalGameScreen.Fractions;
using Microsoft.Xna.Framework;
using MonoSAMFramework.Portable.BatchRenderer;
using MonoSAMFramework.Portable.ColorHelper;
using MonoSAMFramework.Portable.GameMath;
using MonoSAMFramework.Portable.GameMath.Geometry;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.Screens.Entities;

namespace GridDominance.Shared.Screens.WorldMapScreen.Entities
{
	class ConnectionOrb : GameEntity
	{
		public  const float DIAMETER = LevelNode.DIAMETER * 0.15f;
		private const float SPEED = 150f;
		private const float MAX_LIFETIME = 15f;
		private const float GROW_SPEED = 0.3f;

		public override FSize DrawingBoundingBox { get; } = new FSize(DIAMETER, DIAMETER);
		public override Color DebugIdentColor { get; } = Color.MediumOrchid;

		private readonly LevelNodePipe _pipe;
		private readonly FractionDifficulty _diff;

		private FPoint _pos;
		public override FPoint Position => _pos;

		private float _spawnPercentage = 0f;
		private float _movementTime = 0f;

		public ConnectionOrb(GameScreen scrn, LevelNodePipe pipe, FractionDifficulty diff) : base(scrn, GDConstants.ORDER_MAP_ORB)
		{
			_pos = pipe.GetOrbPosition(0);
			_diff = diff;
			_pipe = pipe;
		}

		public override void OnInitialize(EntityManager manager)
		{
			//
		}

		public override void OnRemove()
		{
			//
		}

		protected override void OnUpdate(SAMTime gameTime, InputState istate)
		{
			if (_spawnPercentage < 1)
			{
				_spawnPercentage = FloatMath.LimitedInc(_spawnPercentage, gameTime.ElapsedSeconds / GROW_SPEED, 1f);
			}
			else
			{
				_movementTime += gameTime.ElapsedSeconds;

				_pos = _pipe.GetOrbPosition(_movementTime * SPEED);

				if (_movementTime * SPEED > _pipe.Length) Remove();
			}

			if (Lifetime > MAX_LIFETIME) Remove();
		}

		protected override void OnDraw(IBatchRenderer sbatch)
		{
			Color col;

			float alpha = 1;
			if (_diff == FractionDifficulty.NEUTRAL)
			{
				if (_spawnPercentage >= 1) alpha = 1 - (_movementTime * SPEED / (_pipe.Length / 2));
				col = FlatColors.Asbestos;
			}
			else
			{
				col = FractionDifficultyHelper.GetColor(_diff);
			}

			if (alpha < 0) return;

			sbatch.DrawCentered(Textures.TexParticle[14], Position, DIAMETER * _spawnPercentage, DIAMETER * _spawnPercentage, col * alpha);
		}
	}
}
