using System;
using System.IO;
using System.Text;

namespace iOSBackupLib
{
	internal static class InternalUtilities
	{
		internal const string MBDX_HEADER_ID = "6D-62-64-78-02-00";
		internal const string MBDB_HEADER_ID = "6D-62-64-62-05-00";

		internal static string GetString(Stream sMobileBackup)
		{
			int byteFirst = sMobileBackup.ReadByte();
			int byteSecond = sMobileBackup.ReadByte();

			if (byteFirst == 255 && byteSecond == 255)
				return "NA";

			sMobileBackup.Seek(-2, SeekOrigin.Current);

			ushort stringLen = InternalUtilities.GetUInt16(sMobileBackup);

			byte[] bStringBuff = new byte[stringLen];
			sMobileBackup.Read(bStringBuff, 0, bStringBuff.Length);

			string decodedString = Encoding.UTF8.GetString(bStringBuff, 0, stringLen);

			return decodedString.Normalize(NormalizationForm.FormC);
		}

		internal static Int16 GetInt16(Stream sMobileBackup)
		{
			byte[] bBuffer = new byte[2];
			sMobileBackup.Read(bBuffer, 0, bBuffer.Length);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToInt16(bBuffer, 0);
		}

		internal static UInt16 GetUInt16(Stream sMobileBackup)
		{
			byte[] bBuffer = new byte[2];
			sMobileBackup.Read(bBuffer, 0, bBuffer.Length);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToUInt16(bBuffer, 0);
		}

		internal static Int32 GetInt32(Stream sMobileBackup)
		{
			byte[] bBuffer = new byte[4];
			sMobileBackup.Read(bBuffer, 0, bBuffer.Length);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToInt32(bBuffer, 0);
		}

		internal static UInt32 GetUInt32(Stream sMobileBackup)
		{
			byte[] bBuffer = new byte[4];
			sMobileBackup.Read(bBuffer, 0, bBuffer.Length);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToUInt32(bBuffer, 0);
		}

		internal static UInt64 GetUInt64(Stream sMobileBackup)
		{
			byte[] bBuffer = new byte[8];
			sMobileBackup.Read(bBuffer, 0, bBuffer.Length);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bBuffer, 0, bBuffer.Length);

			return BitConverter.ToUInt64(bBuffer, 0);
		}

		internal static string GetPropertyValue(Stream sMobileBackup)
		{
			int byteFirst = sMobileBackup.ReadByte();
			int byteSecond = sMobileBackup.ReadByte();

			if (byteFirst == 255 && byteSecond == 255)
				return "NA";

			sMobileBackup.Seek(-2, SeekOrigin.Current);

			ushort stringLen = InternalUtilities.GetUInt16(sMobileBackup);

			byte[] bStringBuff = new byte[stringLen];
			sMobileBackup.Read(bStringBuff, 0, bStringBuff.Length);

			bool foundUnprintable = false;
			for (int i = 0; i < bStringBuff.Length; i++)
			{
				if (bStringBuff[i] < 32 || bStringBuff[i] >= 128)
				{
					foundUnprintable = true;
					break;
				}
			}

			if (!foundUnprintable)
				return Encoding.UTF8.GetString(bStringBuff, 0, bStringBuff.Length);

			StringBuilder sbTemp = new StringBuilder();
			for (int i = 0; i < bStringBuff.Length; i++)
			{
				sbTemp.Append(((int)bStringBuff[i]).ToString("X").ToLower());
			}

			return sbTemp.ToString();
		}

		internal static string EpochTimeToString(int epochTime)
		{
			DateTime dtEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			return dtEpoch.AddSeconds(epochTime).ToString("MM/dd/yyyy hh:mm:ss tt");
		}

	}
}
