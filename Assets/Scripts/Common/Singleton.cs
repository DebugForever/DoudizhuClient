using System;



public class Singleton<T> where T : Singleton<T>, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instance)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                        _instance.Init();
                    }
                }
            }
            return _instance;
        }
    }

    protected virtual void Init()
    {

    }
}

