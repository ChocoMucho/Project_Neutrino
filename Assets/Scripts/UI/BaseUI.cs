using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public abstract class BaseUI : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // 컴포넌트 자동 매핑 (Enum 타입 이용)
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = FindChild(gameObject, names[i], true);
            else
                objects[i] = FindChild<T>(gameObject, names[i], true);
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        if (_objects.TryGetValue(typeof(T), out UnityEngine.Object[] objects))
            return objects[idx] as T;
        return null;
    }

    // 헬퍼 메서드: 이름으로 자식 컴포넌트 찾기
    private T FindChild<T>(GameObject go, string name, bool recursive) where T : UnityEngine.Object
    {
        if (go == null) return null;
        if (!recursive)
        {
            Transform transform = go.transform.Find(name);
            if (transform != null) return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }

    private GameObject FindChild(GameObject go, string name, bool recursive)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        return transform?.gameObject;
    }
}