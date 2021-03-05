using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ToolGenerateCode : MonoBehaviour
{
    static readonly List<System.Type> scanTypes = new List<System.Type> { typeof(Text), typeof(Button) };
    /// <summary>
    /// 获取transform下所有子物体的名字，以代码的形式输出
    /// </summary>
    /// <param name="t"></param>
    /// <param name="prefix"></param>
    /// <param name="output"></param>
    static void GenerateCode(Transform t, ref List<string> declareStringList, ref List<string> assignStringList, string prefix = "")
    {
        foreach (System.Type type in scanTypes)
        {
            if (t.GetComponent(type))
            {
                string name = t.name.Clone() as string;
                name = char.ToLower(name[0]) + name.Substring(1);//处理一下以将帕斯卡命名法改为驼峰命名法
                string declareString = string.Format("private {0} {1};", type.ToString(), name);
                string assignString = string.Format("{0} = transform.Find(\"{1}\").GetComponent<{2}>();", name, prefix + t.name, type.ToString());
                declareStringList.Add(declareString);
                assignStringList.Add(assignString);

                //这里这个break假设每个gameobject只有一个需要生成的组件
                //之后有需要再改
                break;
            }
        }

        foreach (Transform child in t)
        {
            GenerateCode(child, ref declareStringList, ref assignStringList, prefix + t.name + "/");
        }
    }

    /// <summary>
    /// 对于选中目标，生成getcomponent的UI代码
    /// </summary>
    [MenuItem("MyTools/GenerateUICode")]
    static void MainFunc()
    {
        GameObject targetObj = Selection.activeGameObject;
        List<string> declareStringList = new List<string>();
        List<string> assignStringList = new List<string>();

        foreach (Transform t in targetObj.transform)
        {
            GenerateCode(t, ref declareStringList, ref assignStringList);
        }


        const string beginComment = "// === auto generated code begin === \n";
        const string endComment = "// === auto generated code end === \n";
        string outputString1 = beginComment + string.Join("\n", declareStringList) + "\n" + endComment;
        string outputString2 = beginComment + string.Join("\n", assignStringList) + "\n" + endComment;
        const string formatString = "{0}\nprivate void Awake(){{\n{1}\n}}";
        UnityEngine.GUIUtility.systemCopyBuffer = string.Format(formatString, outputString1, outputString2);
        Debug.Log("结果已复制到剪切板");
    }
}
