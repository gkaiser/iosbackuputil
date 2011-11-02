using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iOSBackupLib
{
	/// <summary>
	/// 
	/// </summary>
	public class MbdbRecordCollection : ICollection<MbdbRecord>
	{
		private List<MbdbRecord> _lstMbdb = new List<MbdbRecord>();

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add(MbdbRecord item)
		{
			if (_lstMbdb.Contains(item))
				return;

			_lstMbdb.Add(item);
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_lstMbdb.Clear();
		}

		/// <summary>
		/// Determines whether [contains] [the specified item].
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(MbdbRecord item)
		{
			return _lstMbdb.Contains(item);
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(MbdbRecord[] array, int arrayIndex)
		{
			_lstMbdb.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get { return _lstMbdb.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
		/// </value>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		public bool Remove(MbdbRecord item)
		{
			return _lstMbdb.Remove(item);
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<MbdbRecord> GetEnumerator()
		{
			return _lstMbdb.GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)_lstMbdb.GetEnumerator();
		}
	}
}
