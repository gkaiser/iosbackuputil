using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace iOSBackupLib
{
	internal static class InternalUtilities
	{
		internal const string MBDX_HEADER_ID = "6D-62-64-78-02-00";
		internal const string MBDB_HEADER_ID = "6D-62-64-62-05-00";

		internal static string GetString(byte[] bBuffer, int startOffset, int stringLength)
		{
			int byteFirst = bBuffer[0];
			int byteSecond = bBuffer[1];

			if (byteFirst == 255 && byteSecond == 255)
				return "NA";

			string decodedString = Encoding.UTF8.GetString(bBuffer, startOffset, stringLength);

			return decodedString.Normalize(NormalizationForm.FormC);
		}

		internal static Int16 GetInt16(byte[] bBuffer, int startOffset)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToInt16(bBuffer, startOffset);
		}

		internal static UInt16 GetUInt16(byte[] bBuffer, int startOffset)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToUInt16(bBuffer, startOffset);
		}

		internal static Int32 GetInt32(byte[] bBuffer, int startOffset)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToInt32(bBuffer, startOffset);
		}

		internal static UInt32 GetUInt32(byte[] bBuffer, int startOffset)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToUInt32(bBuffer, startOffset);
		}

		internal static Int64 GetInt64(byte[] bBuffer, int startOffset)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToInt64(bBuffer, startOffset);
		}

	}
}
