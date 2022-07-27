using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fNbt;

namespace Rufilities.Strings
{
	// Token: 0x0200000B RID: 11
	public class StringFile
	{
		// Token: 0x06000059 RID: 89 RVA: 0x000048A5 File Offset: 0x00002AA5
		public StringFile()
		{
			this.InitializePaths("en_US");
			this.Reload();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000048BE File Offset: 0x00002ABE
		public StringFile(string languageCode)
		{
			this.InitializePaths(languageCode);
			this.Reload();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000048D3 File Offset: 0x00002AD3
		private void InitializePaths(string languageCode)
		{
			this.stringsFilename = languageCode + ".dat";
			this.stringsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Godot\\app_userdata\\Land Under Wave\\"+ this.stringsFilename;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004908 File Offset: 0x00002B08
		private void AddMetaTags()
		{
			if (this.file.RootTag.Contains("_meta"))
			{
				return;
			}
			NbtCompound newTag = new NbtCompound("_meta");
			this.file.RootTag.Add(newTag);
			foreach (KeyValuePair<string, string> metaTag in StringFile.metaTags)
			{
				NbtString newTag2 = new NbtString(metaTag.Key, metaTag.Value);
				newTag.Add(newTag2);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000049A4 File Offset: 0x00002BA4
		public void Reload()
		{
			this.file = ((!File.Exists(this.stringsPath)) ? new NbtFile(new NbtCompound("strings")) : new NbtFile(this.stringsPath));
			this.AddMetaTags();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000049DC File Offset: 0x00002BDC
		private void FixPeriodNames(NbtTag tag)
		{
			if (!(tag is NbtCompound))
			{
				if (!(tag is NbtString))
				{
					return;
				}
			}
			else
			{
				using (IEnumerator<NbtTag> enumerator = ((IEnumerable<NbtTag>)new List<NbtTag>(((NbtCompound)tag).Tags)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NbtTag tag2 = enumerator.Current;
						this.FixPeriodNames(tag2);
					}
					return;
				}
			}
			if (Enumerable.Contains<char>(tag.Name, '.'))
			{
				NbtCompound parent = (NbtCompound)tag.Parent;
				parent.Remove(tag);
				string[] strArray = tag.Name.Split(new char[]
				{
					'.'
				});
				tag.Name = strArray[strArray.Length - 1];
				NbtCompound nbtCompound = parent;
				for (int index = 0; index < strArray.Length - 1; index++)
				{
					if (!nbtCompound.Contains(strArray[index]))
					{
						NbtCompound newTag = new NbtCompound(strArray[index]);
						nbtCompound.Add(newTag);
						nbtCompound = newTag;
					}
					else
					{
						nbtCompound = nbtCompound.Get<NbtCompound>(strArray[index]);
					}
				}
				nbtCompound.Add(tag);
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004ADC File Offset: 0x00002CDC
		public void FixPeriodNames()
		{
			this.FixPeriodNames(this.file.RootTag);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004AF0 File Offset: 0x00002CF0
		public RufiniString Get(string[] nameParts)
		{
			string str = null;
			NbtCompound result = this.file.RootTag;
			for (int index = 0; index < nameParts.Length - 1; index++)
			{
				result.TryGet<NbtCompound>(nameParts[index], out result);
			}
			if (result != null)
			{
				NbtTag result2 = null;
				result.TryGet(nameParts[nameParts.Length - 1], out result2);
				if (result2 is NbtString)
				{
					str = result2.StringValue;
				}
			}
			return new RufiniString(nameParts, str);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004B52 File Offset: 0x00002D52
		public RufiniString Get(string qualifiedName)
		{
			return this.Get(qualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004B6C File Offset: 0x00002D6C
		private NbtCompound GetFolder(string[] nameParts, int namePartCount, bool addMissingTags)
		{
			int num = Math.Max(0, Math.Min(nameParts.Length, namePartCount));
			NbtCompound folder = this.file.RootTag;
			for (int index = 0; index < num; index++)
			{
				NbtCompound result = null;
				folder.TryGet<NbtCompound>(nameParts[index], out result);
				if (result == null)
				{
					if (!addMissingTags)
					{
						folder = null;
						break;
					}
					result = new NbtCompound(nameParts[index]);
					folder.Add(result);
				}
				folder = result;
			}
			return folder;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004BCE File Offset: 0x00002DCE
		private NbtCompound GetFolder(string[] nameParts, bool addMissingTags)
		{
			return this.GetFolder(nameParts, nameParts.Length, addMissingTags);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004BDB File Offset: 0x00002DDB
		public bool PutFolder(string[] nameParts)
		{
			return this.GetFolder(nameParts, true) != null;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004BE8 File Offset: 0x00002DE8
		public bool PutFolder(string qualifiedName)
		{
			return this.PutFolder(qualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004C04 File Offset: 0x00002E04
		public bool Put(string[] nameParts, string value)
		{
			bool flag = false;
			NbtCompound folder = this.GetFolder(nameParts, nameParts.Length - 1, true);
			if (folder != null)
			{
				bool flag2 = true;
				string namePart = nameParts[nameParts.Length - 1];
				NbtTag result = null;
				if (folder.TryGet(namePart, out result))
				{
					if (result is NbtString)
					{
						folder.Remove(result);
					}
					else
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					NbtTag newTag = new NbtString(namePart, value);
					folder.Add(newTag);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004C69 File Offset: 0x00002E69
		public bool Put(string qualifiedName, string value)
		{
			return this.Put(qualifiedName.Split(new char[]
			{
				'.'
			}), value);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004C84 File Offset: 0x00002E84
		public bool Remove(string[] nameParts)
		{
			bool flag = false;
			NbtCompound rootTag = this.file.RootTag;
			NbtCompound folder = this.GetFolder(nameParts, nameParts.Length - 1, false);
			if (folder != null)
			{
				NbtTag result = null;
				folder.TryGet(nameParts[nameParts.Length - 1], out result);
				if (result != null)
				{
					folder.Remove(result);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public bool Remove(string qualifiedName)
		{
			return this.Remove(qualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004CEC File Offset: 0x00002EEC
		private NbtTag GetTag(string[] nameParts, int namePartCount)
		{
			int num = Math.Max(0, Math.Min(nameParts.Length, namePartCount));
			NbtTag tag = this.file.RootTag;
			for (int index = 0; index < num; index++)
			{
				NbtTag result = null;
				if (!(tag is NbtCompound))
				{
					if (tag is NbtString)
					{
						if (index < num - 1)
						{
							tag = null;
							break;
						}
						break;
					}
				}
				else
				{
					((NbtCompound)tag).TryGet(nameParts[index], out result);
					if (result == null)
					{
						tag = null;
						break;
					}
					tag = result;
				}
			}
			return tag;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004D5C File Offset: 0x00002F5C
		private bool MoveTag(NbtTag oldTag, string[] oldNameParts, string[] newNameParts)
		{
			bool flag = false;
			if (oldTag.Parent is NbtCompound)
			{
				((NbtCompound)oldTag.Parent).Remove(oldTag);
				string[] strArray = new string[newNameParts.Length - 1];
				Array.Copy(newNameParts, strArray, strArray.Length);
				NbtCompound folder = this.GetFolder(strArray, false);
				if (folder != null)
				{
					folder.Add(oldTag);
					flag = true;
				}
			}
			if (oldTag is NbtCompound)
			{
				foreach (NbtTag oldTag2 in ((IEnumerable<NbtTag>)new List<NbtTag>(((NbtCompound)oldTag).Tags)))
				{
					string[] strArray2 = new string[oldNameParts.Length + 1];
					Array.Copy(oldNameParts, strArray2, oldNameParts.Length);
					strArray2[strArray2.Length - 1] = oldTag2.Name;
					string[] strArray3 = new string[newNameParts.Length + 1];
					Array.Copy(newNameParts, strArray3, newNameParts.Length);
					strArray3[strArray3.Length - 1] = oldTag2.Name;
					flag &= this.MoveTag(oldTag2, strArray2, strArray3);
				}
			}
			return flag;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004E64 File Offset: 0x00003064
		public bool Move(string[] oldNameParts, string[] newNameParts)
		{
			return this.MoveTag(this.GetTag(oldNameParts, oldNameParts.Length), oldNameParts, newNameParts);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004E78 File Offset: 0x00003078
		public bool Move(string oldQualifiedName, string newQualifiedName)
		{
			return this.Move(oldQualifiedName.Split(new char[]
			{
				'.'
			}), newQualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004EA4 File Offset: 0x000030A4
		public void Save()
		{
			string directoryName = Path.GetDirectoryName(this.stringsPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			this.file.SaveToFile(this.stringsPath, NbtCompression.GZip);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004EE0 File Offset: 0x000030E0
		public void Save(string root)
		{
			string str = Path.Combine(root, this.stringsFilename);
			string directoryName = Path.GetDirectoryName(str);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			this.file.SaveToFile(str, NbtCompression.GZip);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004F20 File Offset: 0x00003120
		private StringNode BuildNode(StringNode parent, NbtTag nodeTag)
		{
			bool isContainer = nodeTag is NbtCompound;
			StringNode parent2 = new StringNode(parent, nodeTag.Name, isContainer);
			if (isContainer)
			{
				foreach (NbtTag tag in ((NbtCompound)nodeTag).Tags)
				{
					StringNode stringNode = this.BuildNode(parent2, tag);
					parent2.Children.Add(stringNode);
				}
			}
			return parent2;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004FA0 File Offset: 0x000031A0
		internal List<StringNode> ToNodes()
		{
			List<StringNode> nodes = new List<StringNode>();
			foreach (NbtTag nodeTag in this.file.RootTag)
			{
				StringNode stringNode = this.BuildNode(null, nodeTag);
				nodes.Add(stringNode);
			}
			return nodes;
		}

		// Token: 0x04000040 RID: 64
		private const string META_TAG_NAME = "_meta";

		// Token: 0x04000041 RID: 65
		private const string DEFAULT_LANG_CODE = "en_US";

		// Token: 0x04000042 RID: 66
		private const string STRINGS_SUBDIR = "Text";

		// Token: 0x04000043 RID: 67
		private const string STRING_FILE_EXT = "dat";

		// Token: 0x04000044 RID: 68
		private static readonly Dictionary<string, string> metaTags = new Dictionary<string, string>
		{
			{
				"code",
				"en_US"
			},
			{
				"language",
				"English (US)"
			},
			{
				"author",
				"<empty>"
			},
			{
				"address",
				"<empty>"
			},
			{
				"version",
				"1.0"
			}
		};

		// Token: 0x04000045 RID: 69
		private string stringsFilename;

		// Token: 0x04000046 RID: 70
		private string stringsPath;

		// Token: 0x04000047 RID: 71
		private NbtFile file;
	}
}
