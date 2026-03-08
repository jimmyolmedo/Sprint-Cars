using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreScene : MonoBehaviour
{
    [SerializeField] GameObject scorePanel;//pantalla para mostrar el puntaje actual de los jugadores
    [SerializeField] GameObject UpgradePanel;//pantalla donde los jugadores podran mejorarse a cambio de monedas
    [SerializeField] int timeToChangePanel = 2;//tiempo que se mostrara el puntaje, para luego cambiar a la pantalla de mejoras

    [SerializeField] List<ScorePanel> scorePanels = new List<ScorePanel>();//lista de paneles, donde se muestra el nombre y el puntaje de cada jugador

    private void Awake()
    {
        for (int i = 0; i < PlayersManager.instance.players.Count; i++)//recorre la lista de jugadores que hay actualmente en la escena
        {
            scorePanels[i].player = PlayersManager.instance.players[i];//entrega los datos del jugador a su respectivo panel
            scorePanels[i].initialize();//hace que el panel actualice los datos con los del jugador entregado
        }

        //recorre los paneles y deja encendidos solo los que corresponden al numero de jugadores actuales
        for(int i = 0; i < scorePanels.Count; i++)
        {
            if(i < PlayersManager.instance.players.Count)
            {
                scorePanels[i].gameObject.SetActive(true);
            }
            else
            {
                scorePanels[i].gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(ActiveShop());//activa la coroutine que 

        int maxPlayers = PlayersManager.instance.players.Count;
        List<ScorePanel> items = GetComponentsInChildren<ScorePanel>().Take(maxPlayers).ToList();

        // Orden ascendente (menor arriba)
        items = items.OrderByDescending(i => i.player.Score).ToList();

        for (int i = 0; i < items.Count; i++)
        {
            items[i].transform.SetSiblingIndex(i);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }



    IEnumerator ActiveShop()
    {
        //if(LevelsManager.instance.LevelsCount == 0) { LevelsManager.instance.NextLevel(); yield break;}

        yield return new WaitForSeconds(timeToChangePanel);

        scorePanel.SetActive(false);
        UpgradePanel.SetActive(true);
    }
}
