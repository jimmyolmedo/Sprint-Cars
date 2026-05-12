using System.Collections;
using TMPro;
using UnityEngine;

public class UI_StartGame : MonoBehaviour
{
    [SerializeField] protected GameObject startPanel;
    protected float timer;
    [SerializeField] protected int timeToStartGame = 5;
    [SerializeField] protected TextMeshProUGUI textStartCount;

    protected virtual IEnumerator TimeToStartGame()
    {
        timer = timeToStartGame;
        startPanel.SetActive(true);
        while (timer > 0)
        {
            textStartCount.text = timer.ToString();
            timer--;
            yield return new WaitForSeconds(1);
        }
        LevelsManager.instance.NextLevel();
    }

}
