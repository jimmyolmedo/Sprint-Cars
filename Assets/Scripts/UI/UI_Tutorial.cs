using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UI_Tutorial : UI_StartGame
{
    private void OnEnable()
    {
        PlayerController.onPressStart += startGame;
    }

    private void OnDisable()
    {
        PlayerController.onPressStart -= startGame;
    }

    private void Start()
    {
        foreach (PlayerController player in PlayersManager.instance.players)
        {
            player.RestoreValues();
        }
    }

    void startGame()
    {
        foreach (PlayerController player in PlayersManager.instance.players)
        {
            if (player.IsReady == false)
            {
                StopAllCoroutines();
                startPanel.SetActive(false);
                return;
            }
        }

        if (LevelsManager.instance.Pressed == false) { StartCoroutine(TimeToStartGame()); }
        ;
    }
}
