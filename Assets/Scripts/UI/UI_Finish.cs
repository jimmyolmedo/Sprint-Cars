using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Finish : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winnerText;
    int currentScore;

    private void Start()
    {
        List<PlayerController> players = new List<PlayerController>(PlayersManager.instance.players);

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Score > currentScore)
            {
                currentScore = players[i].Score;
                winnerText.text = players[i].PlayerName;
            }
            else if (players[i].Score == currentScore)
            {
                winnerText.text += $", {players[i].PlayerName}" ;
            }
        }

        winnerText.text += " Win!!";

        foreach (PlayerController player in players)
        {
            player.SwitchState(PlayerState.disable);
        }
    }

   public void NextLevel()
    {
        PlayersManager.instance.destroyManager();
        Destroy(LevelsManager.instance.gameObject);
        SceneManager.instance.LoadScene("Title");
    }
}
