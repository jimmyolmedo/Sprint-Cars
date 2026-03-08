using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssignGamepads : MonoBehaviour
{
    public List<PlayerInput> players;

    void Start()
    {
        // Obtener mandos conectados
        var gamepads = Gamepad.all;

        if(gamepads.Count >= players.Count)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].SwitchCurrentControlScheme(gamepads[i]);
            }
        }
    }
}

