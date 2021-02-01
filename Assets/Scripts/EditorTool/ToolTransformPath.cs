using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolTransformPath : MonoBehaviour
{
    [MenuItem("MyTools/CopyTransformPath")]
    public static void GetTransformPath()
    {
        GameObject targetObj = Selection.activeGameObject;
        Transform t = targetObj.transform;
        List<string> nameList = new List<string>();
        while (t)
        {
            nameList.Add(t.name);
            t = t.parent;
        }
        nameList.Reverse();
        string path = string.Join("/", nameList);
        GUIUtility.systemCopyBuffer = path;
        Debug.Log("结果已复制到剪切板");
    }
}
