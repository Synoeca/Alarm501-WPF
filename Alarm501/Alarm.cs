using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Alarm501
{
	/// <summary>
	/// A class defines ICollection that implemented IAlarm Interface
	/// </summary>
	public class Alarm : ICollection<IAlarmItem>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		/// <summary>
		/// Event triggered when a property changes
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Event triggered when a collection changes
		/// </summary>
		public event NotifyCollectionChangedEventHandler? CollectionChanged;

		/// <summary>
		/// A private backing field for the public ICollection property Alarms
		/// </summary>
		private ICollection<IAlarmItem> _alrams = [];

		/// <summary>
		/// A public property which is ICollection of IAlarm
		/// </summary>
		public ICollection<IAlarmItem> Alarms
		{
			get => _alrams;
			set
			{
				List<IAlarmItem> old = _alrams.ToList();
				_alrams = value;
				CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, old, _alrams.ToList()));
				//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(?)));
			}
		}

		/// <summary>
		/// Selected Item in the collection
		/// </summary>
		public IAlarmItem SelectedItem { get; set; } = default!;

		/// <summary>
		/// Maximum number of items in the collection
		/// </summary>
		public int MaximumItem { get; set; } = 5;

		/// <summary>
		/// A private backing field for the property SnoozeTime
		/// </summary>
		private int _snoozeTime = 1;

		/// <summary>
		/// Snooze time period by minutes (default is 1)
		/// </summary>
		public int SnoozeTime
		{
			get => _snoozeTime;
			set
			{
				if (value >= 0 && value <= 30)
				{
					_snoozeTime = value;
				}
				else if (value < 0)
				{
					_snoozeTime = 0;
				}
				else
				{
					_snoozeTime = 30;
				}
			}
		}

		/// <summary>
		/// Gets the number of items contained in the current instance
		/// </summary>
		public int Count => Alarms.Count;

		/// <summary>
		/// Indicates whether the current collection of orders is read-only
		/// </summary>
		public bool IsReadOnly => false;

		/// <summary>
		/// Adds an item to the current collection of items
		/// </summary>
		/// <param name="item">The item to add to the current collection</param>
		public void Add(IAlarmItem item)
		{
			Alarms.Add(item);
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
		}

		/// <summary>
		/// Clears all items from the current collection of items.
		/// </summary>
		public void Clear()
		{
			Alarms.Clear();
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		/// <summary>
		/// Determines whether the current collection of items contains a specific value
		/// </summary>
		/// <param name="item">An object to locate in the current collection</param>
		/// <returns>true, if an item is found in the current collection; otherwise, false</returns>
		public bool Contains(IAlarmItem item)
		{
			return Alarms.Contains(item);
		}

		/// <summary>
		/// Copies the elements of the current collection to a Array, starting at the specified index
		/// </summary>
		/// <param name="array">A one-dimensional, zero-based Array that is the destination of the elements copied from the current instance</param>
		/// <param name="arrayIndex">A Int32 that specifies the zero-based index in array at which copying begins</param>
		public void CopyTo(IAlarmItem[] array, int arrayIndex)
		{
			Alarms.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection</returns>
		public IEnumerator<IAlarmItem> GetEnumerator()
		{
			return Alarms.GetEnumerator();
		}

		/// <summary>
		/// Removes the first occurrence of an item from the current collection
		/// </summary>
		/// <param name="item">The item to remove from the current collection</param>
		/// <returns>
		/// true, if item was removed from the current collection
		/// false, if item was not found in the current collection
		/// </returns>
		public bool Remove(IAlarmItem item)
		{
			//return Alarms.Remove(item);
			int index = _alrams.ToList().IndexOf(item);
			if (index == -1)
			{
				return false;
			}
			bool result = Alarms.Remove(item);
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
			return result;
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection
		/// </summary>
		/// <returns>An IEnumerator object that can be used to iterate through the collection</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Alarms.GetEnumerator();
		}

		/// <summary>
		/// Refresh Collection
		/// </summary>
		public void Refresh()
		{
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

	}
}
