using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, object> _pools = new Dictionary<string, object>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool<T>(T prefab, int count = 10) where T : Component, IPoolable
    {
        string key = prefab.name;
        if(_pools.ContainsKey(key)) return;
        _pools.Add(key, new Pool<T>(prefab, this.transform, count));
    }

    public T Spawn<T>(T prefab) where T : Component, IPoolable
    {
        if (!_pools.ContainsKey(prefab.name)) CreatePool(prefab);
        return ((Pool<T>)_pools[prefab.name]).Spawn();
    }

    public void Despawn<T>(T item) where T : Component, IPoolable
    {
        string key = item.name;
        if (_pools.ContainsKey(key))
            ((Pool<T>)_pools[key]).Despawn(item);
        else
            Debug.LogWarning($"No pool found for item: {key}");
    }
}
