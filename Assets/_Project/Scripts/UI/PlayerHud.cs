using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private float offset;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    private Player player;
    public Camera mainCamera;
    public Player currentPlayer => player;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        if (player != null)
        {
            var worldPos = player.transform.position;
            worldPos.y += offset;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
            rectTransform.position = screenPos;

            textMeshProUGUI.gameObject.SetActive(screenPos.z > 0);
            image.enabled = (screenPos.z > 0);
        }
    }
    public void SetPlayer(Player player)
    {
        this.player = player;
        textMeshProUGUI.text = player.playerName.ToString();
        this.player.onPlayerNameChanged += OnPlayerNameChanged;
    }
    private void OnPlayerNameChanged(string newName)
    {
        textMeshProUGUI.text = newName;
    }
}
