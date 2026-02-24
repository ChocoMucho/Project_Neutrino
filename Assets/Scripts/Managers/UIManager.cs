using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("@UIManager");
                _instance = go.AddComponent<UIManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private GameObject _uiRoot;

    public T ShowUI<T>(string name) where T : BaseUI
    {
        if (_uiRoot == null) _uiRoot = new GameObject("@UI_Root");

        // ResourceManager를 사용하여 UI 생성
        GameObject go = ResourceManager.Instance.Instantiate($"UI/{name}", _uiRoot.transform);
        return go.GetComponent<T>();
    }
}