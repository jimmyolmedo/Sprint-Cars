using UnityEngine;

public class Coin : MonoBehaviour, Interactable
{
    public SpawnCoin spawner;
    [SerializeField] int coinCount;

    public void Interact(GameObject obj)
    {
        if (obj.TryGetComponent(out PlayerController player))
        {
            player.ChangeCoins(coinCount);
            //animacion del player de ganar monedas
            die();
        }
    }

    void die()
    {
        spawner.DespawnCoin(this.gameObject);
        Destroy(this.gameObject);
    }
}
