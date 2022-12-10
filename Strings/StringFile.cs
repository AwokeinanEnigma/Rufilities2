using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fNbt;

namespace Rufilities.Strings
{
	public class StringFile
	{
		public StringFile()
		{
			this.InitializePaths("en_US");
			this.Reload();
		}

		public StringFile(string languageCode)
		{
			this.InitializePaths(languageCode);
			this.Reload();
		}

		private void InitializePaths(string languageCode)
		{
			this.stringsFilename = languageCode + ".dat";
			this.stringsPath = Path.Combine("Data", "Text", "en_US", this.stringsFilename);
		}

		private void AddMetaTags()
		{
			if (!this.file.RootTag.Contains("_meta"))
			{
				NbtCompound nbtCompound = new NbtCompound("_meta");
				this.file.RootTag.Add(nbtCompound);
				foreach (KeyValuePair<string, string> keyValuePair in StringFile.metaTags)
				{
					NbtString newTag = new NbtString(keyValuePair.Key, keyValuePair.Value);
					nbtCompound.Add(newTag);
				}
			}
		}

		public void Reload()
		{
			if (File.Exists(this.stringsPath))
			{
				this.file = new NbtFile(this.stringsPath);
			}
			else
			{
				NbtCompound rootTag = new NbtCompound("strings");
				this.file = new NbtFile(rootTag);
			}
			this.AddMetaTags();
		}

		private void FixPeriodNames(NbtTag tag)
		{
			if (tag is NbtCompound)
			{
				IEnumerable<NbtTag> enumerable = new List<NbtTag>(((NbtCompound)tag).Tags);
				using (IEnumerator<NbtTag> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NbtTag tag2 = enumerator.Current;
						this.FixPeriodNames(tag2);
					}
					return;
				}
			}
			if (tag is NbtString && tag.Name.Contains('.'))
			{
				NbtCompound nbtCompound = (NbtCompound)tag.Parent;
				nbtCompound.Remove(tag);
				string[] array = tag.Name.Split(new char[]
				{
					'.'
				});
				tag.Name = array[array.Length - 1];
				NbtCompound nbtCompound2 = nbtCompound;
				for (int i = 0; i < array.Length - 1; i++)
				{
					if (!nbtCompound2.Contains(array[i]))
					{
						NbtCompound nbtCompound3 = new NbtCompound(array[i]);
						nbtCompound2.Add(nbtCompound3);
						nbtCompound2 = nbtCompound3;
					}
					else
					{
						nbtCompound2 = nbtCompound2.Get<NbtCompound>(array[i]);
					}
				}
				nbtCompound2.Add(tag);
			}
		}

		public void FixPeriodNames()
		{
			this.FixPeriodNames(this.file.RootTag);
		}

		public RufiniString Get(string[] nameParts)
		{
			string value = null;
			NbtCompound rootTag = this.file.RootTag;
			for (int i = 0; i < nameParts.Length - 1; i++)
			{
				rootTag.TryGet<NbtCompound>(nameParts[i], out rootTag);
			}
			if (rootTag != null)
			{
				NbtTag nbtTag = null;
				rootTag.TryGet(nameParts[nameParts.Length - 1], out nbtTag);
				if (nbtTag is NbtString)
				{
					value = nbtTag.StringValue;
				}
			}
			return new RufiniString(nameParts, value);
		}

		public RufiniString Get(string qualifiedName)
		{
			return this.Get(qualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		private NbtCompound GetFolder(string[] nameParts, int namePartCount, bool addMissingTags)
		{
			int num = Math.Max(0, Math.Min(nameParts.Length, namePartCount));
			NbtCompound nbtCompound = this.file.RootTag;
			for (int i = 0; i < num; i++)
			{
				NbtCompound nbtCompound2 = null;
				nbtCompound.TryGet<NbtCompound>(nameParts[i], out nbtCompound2);
				if (nbtCompound2 == null)
				{
					if (!addMissingTags)
					{
						nbtCompound = null;
						break;
					}
					nbtCompound2 = new NbtCompound(nameParts[i]);
					nbtCompound.Add(nbtCompound2);
				}
				nbtCompound = nbtCompound2;
			}
			return nbtCompound;
		}

		private NbtCompound GetFolder(string[] nameParts, bool addMissingTags)
		{
			return this.GetFolder(nameParts, nameParts.Length, addMissingTags);
		}

		public bool PutFolder(string[] nameParts)
		{
			NbtCompound folder = this.GetFolder(nameParts, true);
			return folder != null;
		}

		public bool PutFolder(string qualifiedName)
		{
			return this.PutFolder(qualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		public bool Put(string[] nameParts, string value)
		{
			bool result = false;
			NbtCompound folder = this.GetFolder(nameParts, nameParts.Length - 1, true);
			if (folder != null)
			{
				bool flag = true;
				string tagName = nameParts[nameParts.Length - 1];
				NbtTag nbtTag = null;
				if (folder.TryGet(tagName, out nbtTag))
				{
					if (nbtTag is NbtString)
					{
						folder.Remove(nbtTag);
					}
					else
					{
						flag = false;
					}
				}
				if (flag)
				{
					nbtTag = new NbtString(tagName, value);
					folder.Add(nbtTag);
					result = true;
				}
			}
			return result;
		}

		public bool Put(string qualifiedName, string value)
		{
			string[] nameParts = qualifiedName.Split(new char[]
			{
				'.'
			});
			return this.Put(nameParts, value);
		}

		public bool Remove(string[] nameParts)
		{
			bool result = false;
			NbtCompound rootTag = this.file.RootTag;
			NbtCompound folder = this.GetFolder(nameParts, nameParts.Length - 1, false);
			if (folder != null)
			{
				NbtTag nbtTag = null;
				folder.TryGet(nameParts[nameParts.Length - 1], out nbtTag);
				if (nbtTag != null)
				{
					folder.Remove(nbtTag);
					result = true;
				}
			}
			return result;
		}

		public bool Remove(string qualifiedName)
		{
			return this.Remove(qualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		private NbtTag GetTag(string[] nameParts, int namePartCount)
		{
			int num = Math.Max(0, Math.Min(nameParts.Length, namePartCount));
			NbtTag nbtTag = this.file.RootTag;
			for (int i = 0; i < num; i++)
			{
				NbtTag nbtTag2 = null;
				if (nbtTag is NbtCompound)
				{
					NbtCompound nbtCompound = (NbtCompound)nbtTag;
					nbtCompound.TryGet(nameParts[i], out nbtTag2);
					if (nbtTag2 == null)
					{
						nbtTag = null;
						break;
					}
					nbtTag = nbtTag2;
				}
				else if (nbtTag is NbtString)
				{
					if (i < num - 1)
					{
						nbtTag = null;
						break;
					}
					break;
				}
			}
			return nbtTag;
		}

		private bool MoveTag(NbtTag oldTag, string[] oldNameParts, string[] newNameParts)
		{
			bool flag = false;
			if (oldTag.Parent is NbtCompound)
			{
				((NbtCompound)oldTag.Parent).Remove(oldTag);
				string[] array = new string[newNameParts.Length - 1];
				Array.Copy(newNameParts, array, array.Length);
				NbtCompound folder = this.GetFolder(array, false);
				if (folder != null)
				{
					folder.Add(oldTag);
					flag = true;
				}
			}
			if (oldTag is NbtCompound)
			{
				NbtCompound nbtCompound = (NbtCompound)oldTag;
				IEnumerable<NbtTag> enumerable = new List<NbtTag>(nbtCompound.Tags);
				foreach (NbtTag nbtTag in enumerable)
				{
					string[] array2 = new string[oldNameParts.Length + 1];
					Array.Copy(oldNameParts, array2, oldNameParts.Length);
					array2[array2.Length - 1] = nbtTag.Name;
					string[] array3 = new string[newNameParts.Length + 1];
					Array.Copy(newNameParts, array3, newNameParts.Length);
					array3[array3.Length - 1] = nbtTag.Name;
					flag &= this.MoveTag(nbtTag, array2, array3);
				}
			}
			return flag;
		}

		public bool Move(string[] oldNameParts, string[] newNameParts)
		{
			NbtTag tag = this.GetTag(oldNameParts, oldNameParts.Length);
			return this.MoveTag(tag, oldNameParts, newNameParts);
		}

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

		public void Save()
		{
			string directoryName = Path.GetDirectoryName(this.stringsPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			this.file.SaveToFile(this.stringsPath, NbtCompression.GZip);
		}

		public void Save(string root)
		{
			string text = Path.Combine(root, this.stringsFilename);
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			this.file.SaveToFile(text, NbtCompression.GZip);
		}

		private StringNode BuildNode(StringNode parent, NbtTag nodeTag)
		{
			bool flag = nodeTag is NbtCompound;
			StringNode stringNode = new StringNode(parent, nodeTag.Name, flag);
			if (flag)
			{
				NbtCompound nbtCompound = (NbtCompound)nodeTag;
				foreach (NbtTag nodeTag2 in nbtCompound.Tags)
				{
					StringNode item = this.BuildNode(stringNode, nodeTag2);
					stringNode.Children.Add(item);
				}
			}
			return stringNode;
		}

		internal List<StringNode> ToNodes()
		{
			List<StringNode> list = new List<StringNode>();
			foreach (NbtTag nbtTag in this.file.RootTag)
			{
				NbtCompound nodeTag = (NbtCompound)nbtTag;
				StringNode item = this.BuildNode(null, nodeTag);
				list.Add(item);
			}
			return list;
		}

		private const string META_TAG_NAME = "_meta";

		private const string DEFAULT_LANG_CODE = "en_US";

		private const string STRINGS_SUBDIR = "Text";

		private const string STRING_FILE_EXT = "dat";

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
				"Mother 4 Team"
			},
			{
				"address",
				"http://mother4game.com"
			},
			{
				"version",
				"0.1"
			}
		};

		private string stringsFilename;

		private string stringsPath;

		private NbtFile file;
	}
}
