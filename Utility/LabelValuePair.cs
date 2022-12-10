using System;

namespace Rufilities.Utility
{
	public class LabelValuePair<T>
	{
		public string Label
		{
			get
			{
				return this.label;
			}
		}

		public T Value
		{
			get
			{
				return this.value;
			}
		}

		public LabelValuePair(string label, T value)
		{
			this.label = label;
			this.value = value;
		}

		public LabelValuePair(LabelValuePair<T> labelValuePair)
		{
			this.label = labelValuePair.label;
			this.value = labelValuePair.value;
		}

		public override string ToString()
		{
			return this.label;
		}

		private readonly string label;

		private readonly T value;
	}
}
