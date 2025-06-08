using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private FusionNetworkManager networkManager;
    private string playerName;
    private Color playerColor;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(Instance);
    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        uiManager.ShowNameInputPopup(OnNameSubmitted);
    }
    void OnNameSubmitted(string name, Color color)
    {
        playerName = name;
        playerColor = color;
        uiManager.SetActiveLoadingScreen(true);
        SceneManager.LoadSceneAsync("MainScene");
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        networkManager.ConnectToSharedRoom(playerName, playerColor);
    }
}
