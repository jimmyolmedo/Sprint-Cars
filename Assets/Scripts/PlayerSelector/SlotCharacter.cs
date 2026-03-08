using UnityEngine;
using UnityEngine.InputSystem;

public class SlotCharacter : MonoBehaviour
{
    //variables
    [SerializeField] string playerName;
    [SerializeField] SlotCharacterManager manager;
    [SerializeField] SOcars currentCar;
    [SerializeField] GameObject disconnectedPanel;
    PlayerController player;
    [SerializeField] GameObject connectedPanel;
    bool isConnected;
    int currentIndex;

    //properties
    public bool IsConnected {  get => isConnected; }

    //methods
    public void Connected(PlayerController _player)
    {
        disconnectedPanel.SetActive(false);
        connectedPanel.SetActive(true);
        _player.slot = this;
        player = _player;           
        isConnected = true;
        SetCharacter(manager.PlayerCount);
        manager.PlayerCount++;
    }

    public void TryChangeCharacter(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if(context.started)
        {
            if (value < 0)
            {
                currentIndex--;
                SetCharacter(currentIndex);
            }
            else if (value > 0)
            {
                currentIndex++;
                SetCharacter(currentIndex);
            }
        }
        Debug.Log(currentIndex);
    }

    void SetCharacter(int value)
    {
        currentIndex = value;
        if(currentIndex < 0)
        {
            currentIndex = manager.SOcars.Count - 1;
        }
        else if(currentIndex >= manager.SOcars.Count)
        {
            currentIndex = 0;
        }
        currentCar = manager.SOcars[currentIndex];
        player.ChangeCar(currentCar);
    }
}
