using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType 
{ 
    Unknown, 
    LobbyScene, 
    GameScene 
}

public class SceneManagerEx : MonoBehaviour
{
    private static SceneManagerEx _instance;
    public static SceneManagerEx Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("@SceneManager");
                _instance = go.AddComponent<SceneManagerEx>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public BaseScene CurrentScene => FindAnyObjectByType<BaseScene>();

    public void LoadScene(SceneType type)
    {
        if (CurrentScene != null)
            CurrentScene.Clear();

        SceneManager.LoadScene(type.ToString());
    }
}