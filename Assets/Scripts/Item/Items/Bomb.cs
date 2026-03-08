using UnityEngine;

public class Bomb : Item
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            //le quita la vida actual del jugador, de esta forma lo destruye al instante, luego este objeto se destruye
            player.GetDamage(player.CurrentHealth);
            Destroy(gameObject);
        }
    }
}
