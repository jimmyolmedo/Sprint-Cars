using UnityEngine;

public class BuyPanel : MonoBehaviour
{
    [SerializeField] BuyUpgrade[] upgrades;

    public void InitializeUpgrades(PlayerController player)
    {
        player.RestoreValues();
        player.SwitchState(PlayerState.enable);
        foreach (BuyUpgrade upgrade in upgrades)
        {
            upgrade.InitializePlayer(player);
        }
    }
}
