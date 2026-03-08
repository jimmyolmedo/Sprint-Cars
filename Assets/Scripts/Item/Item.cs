using UnityEngine;

public enum SpawnPos//indicar si el objeto lo lanzara el jugador por delante o por detras
{
    Back,
    Front
}

public enum ItemType//que tipo de objeto es, objeto que se spawnea o que se gasta.
{
    spawneable,
    utility
}

public class Item : MonoBehaviour//todos los items del juego heredan de item para que el jugador sepa por donde tiene que instanciarlos
{
    public SpawnPos spawnPos;
    public ItemType type;
    public PlayerController playerUser;

    public virtual void SetPlayerUser(PlayerController _playerUser) {  playerUser = _playerUser; }

    public virtual void InitialTarget(GameObject originObj)//para los objetos que necesiten un objetivo se les entrega el origen de base
    {

    }

    public virtual void Use()
    {

    }

    protected virtual void die()
    {
        Destroy(gameObject);
    }
}
