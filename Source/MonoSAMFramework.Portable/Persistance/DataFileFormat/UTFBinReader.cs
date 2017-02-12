﻿using System;
using System.Globalization;

namespace MonoSAMFramework.Portable.Persistance.DataFileFormat
{
	public class UTFBinReader : IDataReader
	{
		private readonly int datalength; // next char
		private readonly string data;
		private int position; // next char

		public UTFBinReader(string input, int headerLength)
		{
			data = input.Substring(headerLength).Replace("\r", "").Replace("\n", "");
			datalength = data.Length;
			position = 0;
		}

		public int ReadInteger()
		{
			var length = ReadSimpleUnsignedInteger(2);

			if (length == 0) return 0;

			return ReadSimpleUnsignedInteger(length);
		}

		private int ReadSimpleUnsignedInteger(int length)
		{
			int r = 0;

			int neg = 1;
			for (int i = 0; i < length; i++)
			{
				if (position >= datalength)
					throw new DataWriterException("Unexpected EOF found");

				char chr = data[position];

				if (i == 0 && chr == '-')
				{
					neg = -1;
					position++;
					continue;
				}

				if (chr < '0' || chr > '9')
					throw new DataWriterException("the character chr(" + (int)chr + ") is not a digit for SimpleInteger deserialization");

				r = r * 10 + (chr - '0');
				position++;
			}

			return r*neg;
		}

		public string ReadString()
		{
			int len = ReadInteger();

			string raw = ReadRawString(len);

			return PersistanceHelper.UnescapeString(raw);
		}

		public double ReadDouble()
		{
			var len = ReadSimpleUnsignedInteger(2);

			var raw = ReadRawString(len);

			double r;
			if (double.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out r))
				return r;

			throw new DataWriterException("the string '" + raw + "' is not a valid value for Double deserialization");
		}

		public bool ReadBool()
		{
			if (position >= datalength)
				throw new DataWriterException("Unexpected EOF found");

			char chr = data[position];
			position++;

			if (chr == '0')
				return false;
			if (chr == '1')
				return true;

			throw new DataWriterException("the character chr(" + (int)chr + ") is not a valid value for Boolean deserialization");
		}

		public SemVersion ReadVersion()
		{
			var mayor = (UInt16)ReadSimpleUnsignedInteger(5);
			var minor = (UInt16)ReadSimpleUnsignedInteger(5);
			var patch = (UInt16)ReadSimpleUnsignedInteger(5);

			return new SemVersion(mayor, minor, patch);
		}

		public byte ReadRawPrintableByte()
		{
			if (position >= datalength)
				throw new DataWriterException("Unexpected EOF found");

			char chr = data[position];
			position++;

			return (byte) chr;
		}

		public string ReadFixedLengthNonEscapedASCII(int length)
		{
			return ReadRawString(length);
		}

		private string ReadRawString(int len)
		{
			if (position >= datalength)
				throw new DataWriterException("Unexpected EOF found");
			if (position + len > datalength)
				throw new DataWriterException("Unexpected EOF found");

			string r = data.Substring(position, len);

			position += len;

			return r;
		}

		public string GetHashOfInput()
		{
			return PersistanceHelper.CreateFileIntegrityHash(data);
		}
	}
}
