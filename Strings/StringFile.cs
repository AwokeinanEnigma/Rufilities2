// Decompiled with JetBrains decompiler
// Type: Rufilities.Strings.StringFile
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

using fNbt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rufilities.Strings
{
  public class StringFile
  {
    private const string META_TAG_NAME = "_meta";
    private const string DEFAULT_LANG_CODE = "en_US";
    private const string STRINGS_SUBDIR = "Text";
    private const string STRING_FILE_EXT = "dat";
    private static readonly Dictionary<string, string> metaTags = new Dictionary<string, string>()
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
            this.stringsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Godot\\app_userdata\\Land Under Wave\\" + this.stringsFilename;
        }

        private void AddMetaTags()
    {
      if (this.file.RootTag.Contains("_meta"))
        return;
      NbtCompound newTag1 = new NbtCompound("_meta");
      this.file.RootTag.Add((NbtTag) newTag1);
      foreach (KeyValuePair<string, string> metaTag in StringFile.metaTags)
      {
        NbtString newTag2 = new NbtString(metaTag.Key, metaTag.Value);
        newTag1.Add((NbtTag) newTag2);
      }
    }

    public void Reload()
    {
      this.file = !File.Exists(this.stringsPath) ? new NbtFile(new NbtCompound("strings")) : new NbtFile(this.stringsPath);
      this.AddMetaTags();
    }

    private void FixPeriodNames(NbtTag tag)
    {
      switch (tag)
      {
        case NbtCompound _:
          using (IEnumerator<NbtTag> enumerator = ((IEnumerable<NbtTag>) new List<NbtTag>(((NbtCompound) tag).Tags)).GetEnumerator())
          {
            while (enumerator.MoveNext())
              this.FixPeriodNames(enumerator.Current);
            break;
          }
        case NbtString _:
          if (!tag.Name.Contains<char>('.'))
            break;
          NbtCompound parent = (NbtCompound) tag.Parent;
          parent.Remove(tag);
          string[] strArray = tag.Name.Split('.');
          tag.Name = strArray[strArray.Length - 1];
          NbtCompound nbtCompound = parent;
          for (int index = 0; index < strArray.Length - 1; ++index)
          {
            if (!nbtCompound.Contains(strArray[index]))
            {
              NbtCompound newTag = new NbtCompound(strArray[index]);
              nbtCompound.Add((NbtTag) newTag);
              nbtCompound = newTag;
            }
            else
              nbtCompound = nbtCompound.Get<NbtCompound>(strArray[index]);
          }
          nbtCompound.Add(tag);
          break;
      }
    }

    public void FixPeriodNames() => this.FixPeriodNames((NbtTag) this.file.RootTag);

    public RufiniString Get(string[] nameParts)
    {
      string str = (string) null;
      NbtCompound result1 = this.file.RootTag;
      for (int index = 0; index < nameParts.Length - 1; ++index)
        result1.TryGet<NbtCompound>(nameParts[index], out result1);
      if (result1 != null)
      {
        NbtTag result2 = (NbtTag) null;
        result1.TryGet(nameParts[nameParts.Length - 1], out result2);
        if (result2 is NbtString)
          str = result2.StringValue;
      }
      return new RufiniString(nameParts, str);
    }

    public RufiniString Get(string qualifiedName) => this.Get(qualifiedName.Split('.'));

    private NbtCompound GetFolder(
      string[] nameParts,
      int namePartCount,
      bool addMissingTags)
    {
      int num = Math.Max(0, Math.Min(nameParts.Length, namePartCount));
      NbtCompound folder = this.file.RootTag;
      for (int index = 0; index < num; ++index)
      {
        NbtCompound result = (NbtCompound) null;
        folder.TryGet<NbtCompound>(nameParts[index], out result);
        if (result == null)
        {
          if (addMissingTags)
          {
            result = new NbtCompound(nameParts[index]);
            folder.Add((NbtTag) result);
          }
          else
          {
            folder = (NbtCompound) null;
            break;
          }
        }
        folder = result;
      }
      return folder;
    }

    private NbtCompound GetFolder(string[] nameParts, bool addMissingTags) => this.GetFolder(nameParts, nameParts.Length, addMissingTags);

    public bool PutFolder(string[] nameParts) => this.GetFolder(nameParts, true) != null;

    public bool PutFolder(string qualifiedName) => this.PutFolder(qualifiedName.Split('.'));

    public bool Put(string[] nameParts, string value)
    {
      bool flag1 = false;
      NbtCompound folder = this.GetFolder(nameParts, nameParts.Length - 1, true);
      if (folder != null)
      {
        bool flag2 = true;
        string namePart = nameParts[nameParts.Length - 1];
        NbtTag result = (NbtTag) null;
        if (folder.TryGet(namePart, out result))
        {
          if (result is NbtString)
            folder.Remove(result);
          else
            flag2 = false;
        }
        if (flag2)
        {
          NbtTag newTag = (NbtTag) new NbtString(namePart, value);
          folder.Add(newTag);
          flag1 = true;
        }
      }
      return flag1;
    }

    public bool Put(string qualifiedName, string value) => this.Put(qualifiedName.Split('.'), value);

    public bool Remove(string[] nameParts)
    {
      bool flag = false;
      NbtCompound rootTag = this.file.RootTag;
      NbtCompound folder = this.GetFolder(nameParts, nameParts.Length - 1, false);
      if (folder != null)
      {
        NbtTag result = (NbtTag) null;
        folder.TryGet(nameParts[nameParts.Length - 1], out result);
        if (result != null)
        {
          folder.Remove(result);
          flag = true;
        }
      }
      return flag;
    }

    public bool Remove(string qualifiedName) => this.Remove(qualifiedName.Split('.'));

    private NbtTag GetTag(string[] nameParts, int namePartCount)
    {
      int num = Math.Max(0, Math.Min(nameParts.Length, namePartCount));
      NbtTag tag = (NbtTag) this.file.RootTag;
      for (int index = 0; index < num; ++index)
      {
        NbtTag result = (NbtTag) null;
        switch (tag)
        {
          case NbtCompound _:
            ((NbtCompound) tag).TryGet(nameParts[index], out result);
            if (result == null)
            {
              tag = (NbtTag) null;
              goto label_9;
            }
            else
            {
              tag = result;
              break;
            }
          case NbtString _:
            if (index < num - 1)
            {
              tag = (NbtTag) null;
              goto label_9;
            }
            else
              goto label_9;
        }
      }
label_9:
      return tag;
    }

    private bool MoveTag(NbtTag oldTag, string[] oldNameParts, string[] newNameParts)
    {
      bool flag = false;
      if (oldTag.Parent is NbtCompound)
      {
        ((NbtCompound) oldTag.Parent).Remove(oldTag);
        string[] strArray = new string[newNameParts.Length - 1];
        Array.Copy((Array) newNameParts, (Array) strArray, strArray.Length);
        NbtCompound folder = this.GetFolder(strArray, false);
        if (folder != null)
        {
          folder.Add(oldTag);
          flag = true;
        }
      }
      if (oldTag is NbtCompound)
      {
        foreach (NbtTag oldTag1 in (IEnumerable<NbtTag>) new List<NbtTag>(((NbtCompound) oldTag).Tags))
        {
          string[] strArray1 = new string[oldNameParts.Length + 1];
          Array.Copy((Array) oldNameParts, (Array) strArray1, oldNameParts.Length);
          strArray1[strArray1.Length - 1] = oldTag1.Name;
          string[] strArray2 = new string[newNameParts.Length + 1];
          Array.Copy((Array) newNameParts, (Array) strArray2, newNameParts.Length);
          strArray2[strArray2.Length - 1] = oldTag1.Name;
          flag &= this.MoveTag(oldTag1, strArray1, strArray2);
        }
      }
      return flag;
    }

    public bool Move(string[] oldNameParts, string[] newNameParts) => this.MoveTag(this.GetTag(oldNameParts, oldNameParts.Length), oldNameParts, newNameParts);

    public bool Move(string oldQualifiedName, string newQualifiedName) => this.Move(oldQualifiedName.Split('.'), newQualifiedName.Split('.'));

    public void Save()
    {
      /*string directoryName = Path.GetDirectoryName(this.stringsPath);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);*/
      this.file.SaveToFile(this.stringsPath, NbtCompression.GZip);
    }

        public void Save(string root, bool combine = false)
        {
            if (!combine)
            {
                string str = Path.Combine(root, this.stringsFilename);
                string directoryName = Path.GetDirectoryName(str);
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
                this.file.SaveToFile(str, NbtCompression.GZip);
            }
            else {
                string str = root + "\\" + this.stringsFilename;
                this.file.SaveToFile(str, NbtCompression.GZip);
            }
        }

    private StringNode BuildNode(StringNode parent, NbtTag nodeTag)
    {
      bool isContainer = nodeTag is NbtCompound;
      StringNode parent1 = new StringNode(parent, nodeTag.Name, isContainer);
      if (isContainer)
      {
        foreach (NbtTag tag in ((NbtCompound) nodeTag).Tags)
        {
          StringNode stringNode = this.BuildNode(parent1, tag);
          parent1.Children.Add(stringNode);
        }
      }
      return parent1;
    }

    internal List<StringNode> ToNodes()
    {
      List<StringNode> nodes = new List<StringNode>();
      foreach (NbtTag nodeTag in this.file.RootTag)
      {
        StringNode stringNode = this.BuildNode((StringNode) null, nodeTag);
        nodes.Add(stringNode);
      }
      return nodes;
    }
  }
}
