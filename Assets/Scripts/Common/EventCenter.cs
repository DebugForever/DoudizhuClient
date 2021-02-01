using System;
using System.Collections.Generic;

public class EventCenter
{

    private static Dictionary<EventType, Delegate> eventMap = new Dictionary<EventType, Delegate>();

    /// <summary>
    /// 添加一个事件的监听
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="callback">触发回调</param>
    /// <remarks>一般在Awake()里面调用，不要忘记在OnDestroy()里面调用RemoveListener</remarks>
    public static void AddListener(EventType eventType, Action callback)
    {
        BeforeAddListener(eventType, callback);
        eventMap[eventType] = eventMap[eventType] as Action + callback;
    }

    public static void AddListener<T>(EventType eventType, Action<T> callback)
    {
        BeforeAddListener(eventType, callback);
        eventMap[eventType] = eventMap[eventType] as Action<T> + callback;
    }

    /// <summary>
    /// 移除一个事件的监听
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="callback">触发回调</param>
    /// <remarks>物体销毁后，如果没有移除监听，会报空引用异常。请在OnDestroy里面调用，务必与AddListener配对。</remarks>
    public static void RemoveListener(EventType eventType, Action callback)
    {
        BeforeRemoveListener(eventType, callback);
        eventMap[eventType] = eventMap[eventType] as Action - callback;
        AfterRemoveListener(eventType);
    }

    public static void RemoveListener<T>(EventType eventType, Action<T> callback)
    {
        BeforeRemoveListener(eventType, callback);
        eventMap[eventType] = eventMap[eventType] as Action<T> - callback;
        AfterRemoveListener(eventType);
    }

    /// <summary>
    /// 广播一个事件给其所有的监听者，如果这个事件没有监听者，那么抛出异常。
    /// </summary>
    /// <param name="eventType">要广播的事件类型</param>
    /// <exception cref="ArgumentException">尝试广播一个没有监听者的事件</exception>
    public static void BroadCast(EventType eventType) // 同义词Dispatch
    {
        CheckExist(eventType);
        Action action = eventMap[eventType] as Action;
        action?.Invoke();
    }

    public static void BroadCast<T>(EventType eventType, T arg)
    {
        CheckExist(eventType);
        Action<T> action = eventMap[eventType] as Action<T>;
        action?.Invoke(arg);
    }


    private static void BeforeAddListener(EventType eventType, Delegate callback)
    {
        if (!eventMap.ContainsKey(eventType))
        {
            eventMap.Add(eventType, null);
        }

        if (eventMap[eventType] != null && callback.GetType() != eventMap[eventType].GetType())
        {
            throw new ArgumentException("callback type mismatch!");
        }
    }
    private static void AfterRemoveListener(EventType eventType)
    {
        if (eventMap[eventType] == null)
        {
            eventMap.Remove(eventType);
        }
    }

    private static void BeforeRemoveListener(EventType eventType, Delegate callback)
    {
        if (!eventMap.ContainsKey(eventType))
        {
            throw new ArgumentException(string.Format("EventType {0} not registered!", eventType));
        }

        Delegate dele = eventMap[eventType];
        if (dele == null)
        {
            throw new ArgumentException(string.Format("callback not exist in eventType {0}", eventType));
        }

        if (callback.GetType() != dele.GetType())
        {
            throw new ArgumentException(string.Format("callback type {0} and {1} mismatch!", callback.GetType(), dele.GetType()));
        }

    }



    private static void CheckExist(EventType eventType)
    {
        if (!eventMap.ContainsKey(eventType))
        {
            throw new ArgumentException(string.Format("EventType {0} not registered!", eventType));
        }
    }
}
