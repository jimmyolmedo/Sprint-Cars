using System.Collections;
using UnityEngine;

public class OilStain : MonoBehaviour, Obstacles
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController playerController))
        {
            //hacer que el jugador pierda el control del vehiculo
            playerController.LoseControl();
        }
    }
}
