using UnityEngine;

public class AcelerateObstacle : MonoBehaviour, Obstacles
{
    [SerializeField] float burstTime = .5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            player.BurstAcelerate(burstTime);
        }
    }
}
