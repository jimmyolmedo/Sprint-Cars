using UnityEngine;

public class Coin : MonoBehaviour
{
    public SpawnCoin spawner;
    [SerializeField] int coinCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
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
