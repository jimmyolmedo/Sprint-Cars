using System.Collections;
using TMPro;
using UnityEngine;

public class ShopManager : UI_StartGame
{
    [SerializeField] BuyPanel[] Panels;

    private void OnEnable()
    {
        PlayerController.onPressStart += NextLevel;
        PlayerController.onPressStart += SetReadyPlayers;

        int count = PlayersManager.instance.players.Count;
        for (int i = 0; i < Panels.Length; i++)
        {
            if(i < count)
            {
                Panels[i].gameObject.SetActive(true);
                Panels[i].InitializeUpgrades(PlayersManager.instance.players[i]);
            }
            else
            {
                Panels[i].gameObject.SetActive(false);
            }
        }
        //SetReadyPlayers();

    }
    private void OnDisable()
    {
        PlayerController.onPressStart -= NextLevel;
        PlayerController.onPressStart -= SetReadyPlayers;
    }

    protected override IEnumerator TimeToStartGame()
    {
        return base.TimeToStartGame();
    }

    public void NextLevel()
    {
        foreach(PlayerController player in PlayersManager.instance.players)
        {
            if (!player.IsReady)
            {
                StopAllCoroutines();
                startPanel.SetActive(false);
                return;
            }
        }

        if (LevelsManager.instance.Pressed == false) { StartCoroutine(TimeToStartGame()); };
    }
    void SetReadyPlayers()
    {
        for (int i = 0;i < PlayersManager.instance.players.Count; i++)
        {
            Panels[i].SetReadyText(PlayersManager.instance.players[i].IsReady);
        }
    }
}
