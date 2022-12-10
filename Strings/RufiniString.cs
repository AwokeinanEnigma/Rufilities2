using System;

namespace Rufilities.Strings
{
    public struct RufiniString
    {
        public string QualifiedName => string.Join('.'.ToString(), nameParts);
        public string[] Names => nameParts;
        public string Value => value;
        public RufiniString(string qualifiedName, string value)
        {
            nameParts = qualifiedName.Split(new char[]
            {
                '.'
            });
            this.value = value;
        }
        public RufiniString(string[] nameParts, string value)
        {
            this.nameParts = new string[nameParts.Length];
            Array.Copy(nameParts, this.nameParts, nameParts.Length);
            this.value = value;
        }
        public override string ToString()
        {
            string text;
            if (Value != null)
            {
                text = (value ?? string.Empty).Replace("\n", "");
                if (text.Length > 50)
                {
                    int val = text.Substring(0, 50).LastIndexOf(' ');
                    int length = Math.Max(50, val);
                    text = text.Substring(0, length) + "…";
                }
            }
            else
            {
                text = QualifiedName;
            }
            return text;
        }
        public const char SEPARATOR = '.';
        private const int MAX_LENGTH = 50;
        private const string TRAIL = "…";
        private readonly string[] nameParts;
        private readonly string value;
    }
}
