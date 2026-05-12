using UnityEngine;

public class Bomb : Item, Interactable
{
    public void Interact(GameObject obj)
    {
        if (obj.TryGetComponent(out PlayerCollision player))
        {
            //le quita la vida actual del jugador, de esta forma lo destruye al instante, luego este objeto se destruye
            player.Controller.GetDamage(player.Controller.CurrentHealth);
            Destroy(gameObject);
        }
    }
}
