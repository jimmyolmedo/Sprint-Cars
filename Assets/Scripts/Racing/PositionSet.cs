using System.Collections.Generic;
using UnityEngine;

public class PositionSet : MonoBehaviour
{
    [SerializeField] Transform[] positions;
    public List<PlayerController> playerControllers = new List<PlayerController>();

    private void Start()
    {
        SetPlayerPosition();
    }

    public void SetPlayerPosition()
    {
        playerControllers = new List<PlayerController>(PlayersManager.instance.players);
        for (int i = 0; i < playerControllers.Count; i++)
        {
            playerControllers[i].transform.position = positions[i].position;
            playerControllers[i].transform.rotation = positions[i].rotation;

        }
    }
}
