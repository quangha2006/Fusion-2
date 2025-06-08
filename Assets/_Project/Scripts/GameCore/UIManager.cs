using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField] private NameInputPopup nameInputPopup;
    [SerializeField] private GameObject LoadingCover;
    [SerializeField] private PlayerHUDController playerHUDController;

    public PlayerHUDController PlayerHUDController => playerHUDController;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += onSceneLoaded;
    }
    public void ShowNameInputPopup(Action<string, Color> onSubmit)
    {
        nameInputPopup.Init(onSubmit);
        LoadingCover.SetActive(false);
    }
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadingCover.SetActive(false);
    }
    public void SetActiveLoadingScreen(bool active)
    {
        LoadingCover.SetActive(active);
    }
    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;
    }
}
