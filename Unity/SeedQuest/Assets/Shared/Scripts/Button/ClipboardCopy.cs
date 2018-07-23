using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public static class ClipboardCopy {

    public static void CopyToClipboard(this string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }

}

