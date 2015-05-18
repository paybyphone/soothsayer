using System;
using System.Collections.Generic;
using System.Collections;

namespace soothsayer.Infrastructure
{
	public class Slice<T> : IEnumerable<T>
	{
		private readonly int _startIndex;
		private readonly int _endIndex;
		private readonly T[] _array;
		
		public Slice(T[] array, int startIndex, int endIndex)
		{
			_array = array;
			_startIndex = startIndex;
			_endIndex = endIndex;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = _startIndex; i < _endIndex; i++)
			{
				yield return _array[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			for (int i = _startIndex; i < _endIndex; i++)
			{
				yield return _array[i];
			}
		}

		public int Length
		{
			get
			{
				return _endIndex - _startIndex + 1;
			}
		}

		public T this[int i]
		{
			get
			{
				if (i < 0 || i + _startIndex >= _endIndex)
				{
					throw new IndexOutOfRangeException();
				}

				return _array[i + _startIndex];
			}
			set
			{
				if (i < 0 || i + _startIndex >= _endIndex)
				{
					throw new IndexOutOfRangeException();
				}

				_array[i +  _startIndex] = value;
			}
		}
	}
}

