using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] RacingManager RacingManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            RacingManager.CheckGoal(player);
        }
    }
}
