using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class BuyPanel : MonoBehaviour
{
    [SerializeField] BuyUpgrade[] upgrades;
    [SerializeField] MultiplayerEventSystem eventSystem;
    [SerializeField] InputSystemUIInputModule uiModule;
    [SerializeField] TextMeshProUGUI readyText;
    PlayerController controller;
    [SerializeField] Image playerSprite;

    private void Update()
    {
        if(controller != null) { playerSprite.sprite = controller.Sp.sprite; }
    }

    public void InitializeUpgrades(PlayerController player)
    {
        controller = player;
        player.RestoreValues();
        SetEventSystem(player);
        player.SwitchState(PlayerState.enable);
        foreach (BuyUpgrade upgrade in upgrades)
        {
            upgrade.InitializePlayer(player);
            upgrade.gameObject.SetActive(true);
        }
    }

    void SetEventSystem(PlayerController player)
    {
        uiModule.actionsAsset = player.PlayerInput.actions;
    }

    public void SetReadyText(bool _ready)
    {
        if (_ready == true)
        {
            readyText.text = "Ready!";
            eventSystem.gameObject.SetActive(false);
            uiModule.gameObject.SetActive(false);
            foreach (BuyUpgrade upgrade in upgrades)
            {
                upgrade.gameObject.SetActive(false);
            }
        }
        else
        {
            readyText.text = "not ready...";
            eventSystem.gameObject.SetActive(true);
            uiModule.gameObject.SetActive(true);
            foreach (BuyUpgrade upgrade in upgrades)
            {
                upgrade.gameObject.SetActive(true);
            }
        }
    }
}
