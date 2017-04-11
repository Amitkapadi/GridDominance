﻿using MonoSAMFramework.Portable.DebugTools;

namespace GridDominance.Shared.Screens.OverworldScreen
{
	public class OverworldDebugMinimap : DebugMinimap
	{
		public OverworldDebugMinimap(GDOverworldScreen scrn) : base(scrn)
		{
		}

		protected override float MaxSize => 192;
		protected override float Padding => 32;
	}
}