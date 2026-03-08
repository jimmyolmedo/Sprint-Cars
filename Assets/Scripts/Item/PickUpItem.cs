using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] SOitem item;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            //guarda el item en el jugador y destruye este objeto
            player.GetItem(item);
            //Destroy(gameObject);
        }
    }
}
