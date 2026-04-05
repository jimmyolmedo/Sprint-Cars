using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotCharacterManager : UI_StartGame
{
    public List<SOcars> SOcars = new List<SOcars>();
    [SerializeField] SlotCharacter[] Slots;
    [SerializeField] PositionSet positionSets;
    int playerCount;

    public int PlayerCount {  get => playerCount; set => playerCount = value; }

    private void OnEnable()
    {
        PlayersManager.OnPlayersAdded += JoinCharacter;
        PlayerController.onPressStart += startGame;
    }

    private void OnDisable()
    {
        PlayersManager.OnPlayersAdded -= JoinCharacter;
        PlayerController.onPressStart -= startGame;
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

    void startGame()
    {
        foreach (SlotCharacter slot in Slots)
        {
            if(slot.IsConnected && slot.Player.IsReady == false)
            {
                StopAllCoroutines();
                startPanel.SetActive(false);
                return;
            }
        }

        if(LevelsManager.instance.Pressed == false){StartCoroutine(TimeToStartGame());};
    }

    protected override IEnumerator TimeToStartGame()
    {
        return base.TimeToStartGame();
    }
}
