using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public SceneType SceneType { get; protected set; } = SceneType.Unknown;

    protected virtual void Awake()
    {
        Init();
    }

    /// <summary>
    /// Call When Awake
    /// </summary>
    protected abstract void Init();
    public abstract void Clear();
}