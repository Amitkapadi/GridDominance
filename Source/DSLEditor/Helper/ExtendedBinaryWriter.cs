﻿using System.IO;

namespace GridDominance.DSLEditor.Helper
{
	class ExtendedBinaryWriter : BinaryWriter
	{
		public ExtendedBinaryWriter(Stream stream) : base(stream) { }

		public new void Write7BitEncodedInt(int i)
		{
			base.Write7BitEncodedInt(i);
		}
	}
}
