using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace iOSBackupLib
{
	public class Mbdx
	{
		private FileStream _fsMbdx;

		public string HeaderId;
		public uint RecordCount;
		public MbdxRecordCollection MbdxRecords = new MbdxRecordCollection();

		public Mbdx(string fileName)
		{
			_fsMbdx = File.OpenRead(fileName);
		}

		public void Read()
		{
			byte[] bBuffer = new byte[6];
			_fsMbdx.Read(bBuffer, 0, bBuffer.Length);

			this.HeaderId = BitConverter.ToString(bBuffer);

			if (this.HeaderId != InternalUtilities.MBDX_HEADER_ID) // 6D-62-64-78-02-00 <==> m-b-d-x-0x2-0x0
				throw new FormatException("The MBDX file specified is invalid. Expected \"" + InternalUtilities.MBDX_HEADER_ID + "\" got \"" + this.HeaderId + "\".");

			bBuffer = new byte[4];
			_fsMbdx.Read(bBuffer, 0, bBuffer.Length);

			this.RecordCount = InternalUtilities.GetUInt32(bBuffer, 0);
			
			MbdxRecord mbdxRec;
			StringBuilder sbKey = new StringBuilder();
			bBuffer = new byte[26];
			for (int i = 0; i < this.RecordCount; i++)
			{
				sbKey.Clear();

				_fsMbdx.Read(bBuffer, 0, bBuffer.Length);

				for (int j = 0; j < 20; j++)
				{
					sbKey.Append(BitConverter.ToString(bBuffer, j, 1).ToLower());
				}

				mbdxRec = new MbdxRecord();
				mbdxRec.FileKey = sbKey.ToString();
				mbdxRec.MbdbOffset = InternalUtilities.GetInt32(bBuffer, 20);
				mbdxRec.FileMode = InternalUtilities.GetUInt16(bBuffer, 24);

				this.MbdxRecords.Add(mbdxRec);
			}

			return;
		}

		public struct MbdxRecord
		{
			public string FileKey;
			public int MbdbOffset;
			public ushort FileMode;

			public MbdxRecord(string fileKey, int mbdbOffset, ushort fileMode)
			{
				this.FileKey = fileKey;
				this.MbdbOffset = mbdbOffset;
				this.FileMode = fileMode;
			}
		}

		public class MbdxRecordCollection : ICollection<MbdxRecord>
		{
			private List<MbdxRecord> _lstMbdx = new List<MbdxRecord>();

			public void Add(MbdxRecord item)
			{
				if (_lstMbdx.Contains(item))
					return;

				_lstMbdx.Add(item);
			}

			public void Clear()
			{
				_lstMbdx.Clear();
			}

			public bool Contains(MbdxRecord item)
			{
				return _lstMbdx.Contains(item);
			}

			public void CopyTo(MbdxRecord[] array, int arrayIndex)
			{
				_lstMbdx.CopyTo(array, arrayIndex);
			}

			public int Count
			{
				get { return _lstMbdx.Count; }
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public bool Remove(MbdxRecord item)
			{
				return _lstMbdx.Remove(item);
			}

			public IEnumerator<MbdxRecord> GetEnumerator()
			{
				return _lstMbdx.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return (System.Collections.IEnumerator)_lstMbdx.GetEnumerator();
			}
		}
	}
}
