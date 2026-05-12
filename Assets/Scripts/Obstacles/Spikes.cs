using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour, Obstacles
{
    [SerializeField] bool permanent;

    [Header("not permanence")]
    [SerializeField] float timeToActivate;
    [SerializeField] float timeToWarning;
    [SerializeField] GameObject WarningObj;
    [SerializeField] GameObject spikesObj;
    [SerializeField] Collider2D coll;
    [SerializeField] bool startActivate;

    private void Start()
    {
        spikesObj.SetActive(startActivate);
        if (!permanent)
        {
            //coroutine para encender y apagar las espinas
            StartCoroutine(TimeToActivateSpikes());
        }
    }

    IEnumerator TimeToActivateSpikes()
    {
        while (true)
        {
            if(GameManager.CurrentState == GameState.Gameplay)
            {
                yield return new WaitForSeconds(timeToActivate);

                if (!spikesObj.activeSelf)
                {
                    WarningObj.SetActive(true);
                    yield return new WaitForSeconds(timeToWarning);
                    WarningObj.SetActive(false);
                }

                spikesObj.SetActive(!spikesObj.activeSelf);
                coll.enabled = spikesObj.activeSelf;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerCollision player))
        {
            player.Controller.GetDamage(player.Controller.CurrentHealth);
        }
    }
}
