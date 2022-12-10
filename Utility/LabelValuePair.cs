// Decompiled with JetBrains decompiler

namespace Rufilities.Utility
{
    public class LabelValuePair<T>
    {
        private readonly string label;
        private readonly T value;

        public string Label => label;

        public T Value => value;

        public LabelValuePair(string label, T value)
        {
            this.label = label;
            this.value = value;
        }

        public LabelValuePair(LabelValuePair<T> labelValuePair)
        {
            label = labelValuePair.label;
            value = labelValuePair.value;
        }

        public override string ToString()
        {
            return label;
        }
    }
}
