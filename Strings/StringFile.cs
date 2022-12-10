using fNbt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rufilities.Strings
{
    public class StringFile
    {
        public StringFile()
        {
            InitializePaths("en_US");
            Reload();
        }
        public StringFile(string languageCode)
        {
            InitializePaths(languageCode);
            Reload();
        }
        private void InitializePaths(string languageCode)
        {
            stringsFilename = languageCode + ".dat";
            stringsPath = Path.Combine("Data", this.stringsFilename);
        }
        private void AddMetaTags()
        {
            if (file.RootTag.Contains("_meta"))
            {
                return;
            }
            NbtCompound newTag = new NbtCompound("_meta");
            file.RootTag.Add(newTag);
            foreach (KeyValuePair<string, string> metaTag in StringFile.metaTags)
            {
                NbtString newTag2 = new NbtString(metaTag.Key, metaTag.Value);
                newTag.Add(newTag2);
            }
        }
        public void Reload()
        {
            file = ((!File.Exists(stringsPath)) ? new NbtFile(new NbtCompound("strings")) : new NbtFile(stringsPath));
            AddMetaTags();
        }
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
                        FixPeriodNames(tag2);
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
        public void FixPeriodNames()
        {
            FixPeriodNames(file.RootTag);
        }
        public RufiniString Get(string[] nameParts)
        {
            string str = null;
            NbtCompound result = file.RootTag;
            for (int index = 0; index < nameParts.Length - 1; index++)
            {
                result.TryGet<NbtCompound>(nameParts[index], out result);
            }
            if (result != null)
            {
                result.TryGet(nameParts[nameParts.Length - 1], out NbtTag result2);
                if (result2 is NbtString)
                {
                    str = result2.StringValue;
                }
            }
            return new RufiniString(nameParts, str);
        }
        public RufiniString Get(string qualifiedName)
        {
            return Get(qualifiedName.Split(new char[]
            {
                '.'
            }));
        }
        private NbtCompound GetFolder(string[] nameParts, int namePartCount, bool addMissingTags)
        {
            int num = Math.Max(0, Math.Min(nameParts.Length, namePartCount));
            NbtCompound folder = file.RootTag;
            for (int index = 0; index < num; index++)
            {
                folder.TryGet<NbtCompound>(nameParts[index], out NbtCompound result);
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
        private NbtCompound GetFolder(string[] nameParts, bool addMissingTags)
        {
            return GetFolder(nameParts, nameParts.Length, addMissingTags);
        }
        public bool PutFolder(string[] nameParts)
        {
            return GetFolder(nameParts, true) != null;
        }
        public bool PutFolder(string qualifiedName)
        {
            return PutFolder(qualifiedName.Split(new char[]
            {
                '.'
            }));
        }
        public bool Put(string[] nameParts, string value)
        {
            bool flag = false;
            NbtCompound folder = GetFolder(nameParts, nameParts.Length - 1, true);
            if (folder != null)
            {
                bool flag2 = true;
                string namePart = nameParts[nameParts.Length - 1];
                if (folder.TryGet(namePart, out NbtTag result))
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
        public bool Put(string qualifiedName, string value)
        {
            return Put(qualifiedName.Split(new char[]
            {
                '.'
            }), value);
        }
        public bool Remove(string[] nameParts)
        {
            bool flag = false;
            NbtCompound rootTag = file.RootTag;
            NbtCompound folder = GetFolder(nameParts, nameParts.Length - 1, false);
            if (folder != null)
            {
                folder.TryGet(nameParts[nameParts.Length - 1], out NbtTag result);
                if (result != null)
                {
                    folder.Remove(result);
                    flag = true;
                }
            }
            return flag;
        }
        public bool Remove(string qualifiedName)
        {
            return Remove(qualifiedName.Split(new char[]
            {
                '.'
            }));
        }
        private NbtTag GetTag(string[] nameParts, int namePartCount)
        {
            int num = Math.Max(0, Math.Min(nameParts.Length, namePartCount));
            NbtTag tag = file.RootTag;
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
        private bool MoveTag(NbtTag oldTag, string[] oldNameParts, string[] newNameParts)
        {
            bool flag = false;
            if (oldTag.Parent is NbtCompound)
            {
                ((NbtCompound)oldTag.Parent).Remove(oldTag);
                string[] strArray = new string[newNameParts.Length - 1];
                Array.Copy(newNameParts, strArray, strArray.Length);
                NbtCompound folder = GetFolder(strArray, false);
                if (folder != null)
                {
                    folder.Add(oldTag);
                    flag = true;
                }
            }
            if (oldTag is NbtCompound)
            {
                foreach (NbtTag oldTag2 in new List<NbtTag>(((NbtCompound)oldTag).Tags))
                {
                    string[] strArray2 = new string[oldNameParts.Length + 1];
                    Array.Copy(oldNameParts, strArray2, oldNameParts.Length);
                    strArray2[strArray2.Length - 1] = oldTag2.Name;
                    string[] strArray3 = new string[newNameParts.Length + 1];
                    Array.Copy(newNameParts, strArray3, newNameParts.Length);
                    strArray3[strArray3.Length - 1] = oldTag2.Name;
                    flag &= MoveTag(oldTag2, strArray2, strArray3);
                }
            }
            return flag;
        }
        public bool Move(string[] oldNameParts, string[] newNameParts)
        {
            return MoveTag(GetTag(oldNameParts, oldNameParts.Length), oldNameParts, newNameParts);
        }
        public bool Move(string oldQualifiedName, string newQualifiedName)
        {
            return Move(oldQualifiedName.Split(new char[]
            {
                '.'
            }), newQualifiedName.Split(new char[]
            {
                '.'
            }));
        }
        public void Save()
        {
            string directoryName = Path.GetDirectoryName(stringsPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            file.SaveToFile(stringsPath, NbtCompression.GZip);
        }
        public void Save(string root)
        {
            string str = Path.Combine(root, stringsFilename);
            string directoryName = Path.GetDirectoryName(str);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            file.SaveToFile(str, NbtCompression.GZip);
        }
        private StringNode BuildNode(StringNode parent, NbtTag nodeTag)
        {
            bool isContainer = nodeTag is NbtCompound;
            StringNode parent2 = new StringNode(parent, nodeTag.Name, isContainer);
            if (isContainer)
            {
                foreach (NbtTag tag in ((NbtCompound)nodeTag).Tags)
                {
                    StringNode stringNode = BuildNode(parent2, tag);
                    parent2.Children.Add(stringNode);
                }
            }
            return parent2;
        }
        internal List<StringNode> ToNodes()
        {
            List<StringNode> nodes = new List<StringNode>();
            foreach (NbtTag nodeTag in file.RootTag)
            {
                StringNode stringNode = BuildNode(null, nodeTag);
                nodes.Add(stringNode);
            }
            return nodes;
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
        private string stringsFilename;
        private string stringsPath;
        private NbtFile file;
    }
}
