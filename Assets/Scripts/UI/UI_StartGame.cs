using System.Collections;
using TMPro;
using UnityEngine;

public class UI_StartGame : MonoBehaviour
{
    [SerializeField] protected GameObject startPanel;
    protected float timer;
    [SerializeField] protected int timeToStartGame = 5;
    [SerializeField] protected bool isTutorial;
    [SerializeField] protected TextMeshProUGUI textStartCount;

    protected virtual IEnumerator TimeToStartGame()
    {
        timer = timeToStartGame;
        if(startPanel != null)startPanel.SetActive(true);
        while (timer > 0)
        {
            if(textStartCount != null)textStartCount.text = timer.ToString();
            timer--;
            yield return new WaitForSeconds(1);
        }
        if(isTutorial) {StartTutorial(); } else { LevelsManager.instance.NextLevel(); }
    }

    void StartTutorial()
    {
        SceneManager.instance.LoadScene("Tutorial");
    }

}
