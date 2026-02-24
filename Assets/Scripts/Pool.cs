using UnityEngine;
using System.Collections.Generic;

public class Pool<T> where T : Component, IPoolable
{
    private T _prefab;
    private Transform _root;
    private Stack<T> _stack = new Stack<T>();


    public Pool(T prefab, Transform root, int initialCount = 10)
    {
        _prefab = prefab;
        _root = root;

        for (int i = 0; i < initialCount; i++)
        {
            _stack.Push(CreateNewItem());
        }
    }

    private T CreateNewItem()
    {
        T item = Object.Instantiate(_prefab, _root);
        item.gameObject.SetActive(false);
        item.name = _prefab.name;
        return item;
    }

    public T Spawn()
    {
        T item = _stack.Count > 0 ? _stack.Pop() : CreateNewItem(); // Create new item if pool is empty
        item.gameObject.SetActive(true);
        item.OnSpawn();
        return item;
    }

    public void Despawn(T item)
    {
        item.OnDespawn();
        item.gameObject.SetActive(false);
        _stack.Push(item);
    }
}
