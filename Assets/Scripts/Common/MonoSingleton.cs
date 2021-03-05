using System;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null) //没有示例，就找到第一个挂载该类的对象
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance == null) //没有找到，就创建一个新GameObject并挂载组件
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
                _instance.Init();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 用于初始化，只会在Instance初始化时调用一次
    /// </summary>
    protected virtual void Init()
    {

    }

}
