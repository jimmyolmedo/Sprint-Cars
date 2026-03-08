using System.Collections.Generic;
using UnityEngine;

public class SlotCharacterManager : MonoBehaviour
{
    public List<SOcars> SOcars = new List<SOcars>();
    [SerializeField] SlotCharacter[] Slots;
    [SerializeField] PositionSet positionSets;
    int playerCount;

    public int PlayerCount {  get => playerCount; set => playerCount = value; }

    private void OnEnable()
    {
        PlayersManager.OnPlayersAdded += JoinCharacter;
    }

    private void OnDisable()
    {
        PlayersManager.OnPlayersAdded -= JoinCharacter;
    }

    void JoinCharacter(PlayerController player)
    {
        foreach (SlotCharacter slot in Slots)
        {
            if(!slot.IsConnected)
            {
                slot.Connected(player);
                positionSets.SetPlayerPosition();
                return;
            }
        }
    }

}
