using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour, Obstacles
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerCollision playerController))
        {
            playerController.Controller.MudState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerCollision playerController))
        {
            playerController.Controller.MudState(false);
        }
    }
}
