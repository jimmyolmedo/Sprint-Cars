using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject doorObj;
    [SerializeField] float time;
    [SerializeField] bool startActivate = true;

    private void Start()
    {
        doorObj.SetActive(startActivate);
        StartCoroutine(ActiveDoor());
    }

    IEnumerator ActiveDoor()
    {
        while (true)
        {
            if(GameManager.CurrentState == GameState.Gameplay)
            {
                yield return new WaitForSeconds(time);
                doorObj.SetActive(!doorObj.activeSelf);
            }
            else { yield return null; }
        }
    }
}
