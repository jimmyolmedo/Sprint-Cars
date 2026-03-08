using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : Singleton<PlayersManager>
{
    public static event System.Action<PlayerController> OnPlayersAdded;
    //lista de jugadores
    public List<PlayerController> players = new List<PlayerController>();
    //debe permanecer entre las escenas para que otros scrips puedan acceder a la lista de jugadores
    protected override bool persistent => true;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddPlayer(PlayerController player)
    {
        players.Add(player);
        OnPlayersAdded?.Invoke(player);
        player.transform.parent = transform;
        //player.gameObject.SetActive(false);
    }

    public void destroyManager()
    {
        foreach(PlayerController player in players)
        {
            Destroy(player.gameObject);
        }

        Destroy(gameObject);
    }
}
