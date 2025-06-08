using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private PlayerHud playerHUDPrefab;
    private List<PlayerHud> playerHUD = new List<PlayerHud>();

    public void RegisterPlayer(Player player)
    {
        var newHUD = Instantiate(playerHUDPrefab,  transform);
        newHUD.SetPlayer(player);
        playerHUD.Add(newHUD);
    }
    public void UnregisterPlayer(Player player) 
    {
        PlayerHud? hudtoRemove = null;
        foreach (PlayerHud hud in playerHUD)
        {
            if (hud.currentPlayer == player)
            {
                hudtoRemove = hud;
            }
        }
        if (hudtoRemove != null)
        {
            playerHUD.Remove(hudtoRemove);
            GameObject.Destroy(hudtoRemove.gameObject);
        }
    }
}
