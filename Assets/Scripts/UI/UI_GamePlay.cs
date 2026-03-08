using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePlay : MonoBehaviour
{
    [SerializeField] List<PlayerPanel> uiPanels = new List<PlayerPanel>();

    private void Start()
    {
        //apaga todos los paneles de la escena
        for (int i = 0; i < uiPanels.Count; i++)
        {
            uiPanels[i].gameObject.SetActive(false);
        }

        //prende los paneles segun los jugadores que haya en la escena
        for (int i = 0; i < PlayersManager.instance.players.Count; i++)
        {
            uiPanels[i].SetPlayer(PlayersManager.instance.players[i]);
            uiPanels[i].gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        Sort();
    }

    public void Sort()
    {
        int maxPlayers = PlayersManager.instance.players.Count;
        List<PlayerPanel> items = GetComponentsInChildren<PlayerPanel>().Take(maxPlayers).ToList();

        // Orden ascendente (menor arriba)
        items = items.OrderByDescending(i => i.PlayerReference.CurrentLap).ToList();

        for (int i = 0; i < items.Count; i++)
        {
            items[i].transform.SetSiblingIndex(i);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
