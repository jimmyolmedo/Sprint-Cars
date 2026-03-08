using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] BuyPanel[] Panels;

    private void OnEnable()
    {
        int count = PlayersManager.instance.players.Count;
        for (int i = 0; i < Panels.Length; i++)
        {
            if(i < count)
            {
                Panels[i].gameObject.SetActive(true);
                Panels[i].InitializeUpgrades(PlayersManager.instance.players[i]);
            }
            else
            {
                Panels[i].gameObject.SetActive(false);
            }
        }
    }


    public void NextLevel()
    {
        LevelsManager.instance.NextLevel();
    }
}
