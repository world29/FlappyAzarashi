﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ServiceLocator
{
    private static Dictionary<Type, object> m_container = new Dictionary<Type, object>();

    public static T Resolve<T>()
    {
        return (T)m_container[typeof(T)];
    }

    public static void Register<T>(T instance)
    {
        if (m_container.ContainsKey(typeof(T)))
        {
            m_container.Remove(typeof(T), c => (c as IDisposable)?.Dispose());
        }
        m_container.Add(typeof(T), instance);
    }
}
