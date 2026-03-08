using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour, Obstacles
{
    private List<PlayerController> playersInTrigger = new List<PlayerController>();

    private List<int> playersMaxSpeed = new List<int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            playersInTrigger.Add(player);
            playersMaxSpeed.Add(player.MaxSpeed);
            player.LimitSpeed((int)player.MaxSpeed / 2, player.CurrentSpeed / 2);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            for (int i = 0; i < playersInTrigger.Count; ++i)
            {
                if (playersInTrigger[i] == player)
                {
                    player.LimitSpeed(playersMaxSpeed[i], player.CurrentSpeed);
                    playersInTrigger.RemoveAt(i);
                    playersMaxSpeed.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
