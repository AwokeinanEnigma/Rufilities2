using System;

namespace Rufilities.Strings
{
	// Token: 0x02000005 RID: 5
	public struct RufiniString
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00003850 File Offset: 0x00001A50
		public string QualifiedName
		{
			get
			{
				return string.Join('.'.ToString(), this.nameParts);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00003872 File Offset: 0x00001A72
		public string[] Names
		{
			get
			{
				return this.nameParts;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000387A File Offset: 0x00001A7A
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003884 File Offset: 0x00001A84
		public RufiniString(string qualifiedName, string value)
		{
			this.nameParts = qualifiedName.Split(new char[]
			{
				'.'
			});
			this.value = value;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000038B1 File Offset: 0x00001AB1
		public RufiniString(string[] nameParts, string value)
		{
			this.nameParts = new string[nameParts.Length];
			Array.Copy(nameParts, this.nameParts, nameParts.Length);
			this.value = value;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000038D8 File Offset: 0x00001AD8
		public override string ToString()
		{
			string text;
			if (this.Value != null)
			{
				text = (this.value ?? string.Empty).Replace("\n", "");
				if (text.Length > 50)
				{
					int val = text.Substring(0, 50).LastIndexOf(' ');
					int length = Math.Max(50, val);
					text = text.Substring(0, length) + "…";
				}
			}
			else
			{
				text = this.QualifiedName;
			}
			return text;
		}

		// Token: 0x04000020 RID: 32
		public const char SEPARATOR = '.';

		// Token: 0x04000021 RID: 33
		private const int MAX_LENGTH = 50;

		// Token: 0x04000022 RID: 34
		private const string TRAIL = "…";

		// Token: 0x04000023 RID: 35
		private string[] nameParts;

		// Token: 0x04000024 RID: 36
		private string value;
	}
}
