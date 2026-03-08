using System.Collections.Generic;
using UnityEngine;

public class UI_playerSelector : MonoBehaviour
{
    bool started;
    public void StartGame()
    {
        if(started) return;
        if (PlayersManager.instance.players.Count >= 1)
        {
            for(int i = 0; i < PlayersManager.instance.players.Count; i++)
            {
                PlayersManager.instance.players[i].PlayerName = "player " + (i+1);
            }

            LevelsManager.instance.NextLevel();
        }
        else
        {
            Debug.Log("jugadores insuficientes");
        }
    }
}
