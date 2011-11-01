using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace iOSBackupLib
{
	public class Mbdb
	{
		private FileStream _fsMbdb;

		public string HeaderId;
		public uint RecordCount;
		public MbdbRecordCollection MbdbRecords = new MbdbRecordCollection();

		public Mbdb(string fileName)
		{

		}

		public struct MbdbRecord
		{

		}

		public class MbdbRecordCollection : ICollection<MbdbRecord>
		{
			private List<MbdbRecord> _lstMbdb = new List<MbdbRecord>();

			public void Add(MbdbRecord item)
			{
				if (_lstMbdb.Contains(item))
					return;

				_lstMbdb.Add(item);
			}

			public void Clear()
			{
				_lstMbdb.Clear();
			}

			public bool Contains(MbdbRecord item)
			{
				return _lstMbdb.Contains(item);
			}

			public void CopyTo(MbdbRecord[] array, int arrayIndex)
			{
				_lstMbdb.CopyTo(array, arrayIndex);
			}

			public int Count
			{
				get { return _lstMbdb.Count; }
			}

			public bool IsReadOnly
			{
				get { return false; }
			}

			public bool Remove(MbdbRecord item)
			{
				return _lstMbdb.Remove(item);
			}

			public IEnumerator<MbdbRecord> GetEnumerator()
			{
				return _lstMbdb.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return (System.Collections.IEnumerator)_lstMbdb.GetEnumerator();
			}
		}

	}
}
