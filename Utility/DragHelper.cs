// Decompiled with JetBrains decompiler
// Type: Rufilities.Utility.DragHelper
// Assembly: Rufilities, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FC4E2F2-423B-45D2-9FF7-D0CCE3066F9C
// Assembly location: C:\Users\Thomas\Documents\Mother4Restored\Mother4\bin\Debug\Rufilities.dll

using System;
using System.Runtime.InteropServices;

namespace Rufilities.Utility
{
  internal class DragHelper
  {
    [DllImport("comctl32.dll")]
    public static extern bool InitCommonControls();

    [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
    public static extern bool ImageList_BeginDrag(
      IntPtr himlTrack,
      int iTrack,
      int dxHotspot,
      int dyHotspot);

    [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
    public static extern bool ImageList_DragMove(int x, int y);

    [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
    public static extern void ImageList_EndDrag();

    [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
    public static extern bool ImageList_DragEnter(IntPtr hwndLock, int x, int y);

    [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
    public static extern bool ImageList_DragLeave(IntPtr hwndLock);

    [DllImport("comctl32.dll", CharSet = CharSet.Auto)]
    public static extern bool ImageList_DragShowNolock(bool fShow);

    static DragHelper() => DragHelper.InitCommonControls();
  }
}
