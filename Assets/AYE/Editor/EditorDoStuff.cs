using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;
public class EditorDoStuff
{
    [MenuItem("¦Û­q/´ú¸ÕAAA")]
    public static void TestAAA()
    {
        EditorCoroutineUtility.StartCoroutineOwnerless(ITestAAA());
    }
    static IEnumerator ITestAAA()
    {
        yield return null;
        Debug.Log("A");
        yield return null;
        Debug.Log("A");
        yield return null;
        Debug.Log("A");
    }
}
