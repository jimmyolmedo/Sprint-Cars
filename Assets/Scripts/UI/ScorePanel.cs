using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    public PlayerController player;

    [SerializeField]TextMeshProUGUI playerName;
    [SerializeField]TextMeshProUGUI playerScore;

    public void initialize()
    {
        playerName.text = player.PlayerName;
        playerScore.text = player.Score.ToString();
    }
}
