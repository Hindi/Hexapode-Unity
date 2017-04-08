using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelegateOneArg<T>(T packet);
public class EventManager<T>
{
    private static event DelegateOneArg<T> eventOneArg;

    public static void Register(DelegateOneArg<T> function)
    {
        eventOneArg += function;
    }

    public static void Unregister(DelegateOneArg<T> function)
    {
        eventOneArg -= function;
    }

    public static void Trigger(T param)
    {
        if (eventOneArg != null)
            eventOneArg(param);
    }
}
