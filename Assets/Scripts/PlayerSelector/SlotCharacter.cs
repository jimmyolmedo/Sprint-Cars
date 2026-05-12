using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlotCharacter : MonoBehaviour
{
    //variables
    [SerializeField] string playerName;
    [SerializeField] SlotCharacterManager manager;
    [SerializeField] SOcars currentCar;
    [SerializeField] GameObject disconnectedPanel;
    PlayerController player;
    [SerializeField] Image playerSprite;
    [SerializeField] GameObject connectedPanel;
    bool isConnected;
    int currentIndex;
    [SerializeField] TextMeshProUGUI ReadyValue;
    [SerializeField] Color colorPlayer;

    [Header("statsBar")]
    [SerializeField] Image speedBar;
    [SerializeField] Image acelerationBar;
    [SerializeField] Image rotationBar;
    [SerializeField] Image healthBar;
    [SerializeField] Image attackBar;

    //properties
    public bool IsConnected {  get => isConnected; }

    public bool Ready {  get; private set; }

    public PlayerController Player { get => player; }

    //methods
    private void Update()
    {
        if(playerSprite != null && player != null) { playerSprite.sprite = player.Sp.sprite; }
    }

    public void Connected(PlayerController _player)
    {
        disconnectedPanel.SetActive(false);
        connectedPanel.SetActive(true);
        _player.slot = this;
        player = _player;
        player.setColor(colorPlayer);
        isConnected = true;
        SetCharacter(manager.PlayerCount);
        manager.PlayerCount++;
        readyText();
    }

    public void TryChangeCharacter(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if(context.started && Ready == false)
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

    public void SetReady(bool value)
    {
        Ready = value;
        readyText();
    }

    void readyText()
    {
        if(Ready == true)
        {
            ReadyValue.text = "Ready!";
        }
        else { ReadyValue.text = "not ready...";}
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
        SetBarValues();
    }


    void SetBarValues()
    {
        speedBar.fillAmount = (float)currentCar.defaultSpeed/StatsPlayer.instance.MaxSpeed;
        acelerationBar.fillAmount = (float)currentCar.aceleration/StatsPlayer.instance.MaxAcceleration;
        rotationBar.fillAmount = (float)currentCar.rotationSpeed/StatsPlayer.instance.MaxRotation;
        healthBar.fillAmount = (float)currentCar.defaultHealth/StatsPlayer.instance.MaxHealth;
        attackBar.fillAmount = (float)currentCar.attack/StatsPlayer.instance.MaxAttack;
    }
}
