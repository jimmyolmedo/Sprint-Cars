using System.Collections;
using UnityEngine;

public class PickUpItem : MonoBehaviour, Interactable
{
    [SerializeField] SOitem[] item;
    [SerializeField] Collider2D coll;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float timeToRespawn;

    public void Interact(GameObject obj)
    {
        if (obj.TryGetComponent(out PlayerController player))
        {
            int index = Random.Range(0, item.Length);
            //guarda el item en el jugador y apaga este objeto
            player.GetItem(item[index]);
            coll.enabled = false;
            spriteRenderer.enabled = false;
            StartCoroutine(RespawnObj());
        }
    }

    IEnumerator RespawnObj()
    {
        yield return new WaitForSeconds(timeToRespawn);

        coll.enabled = true;
        spriteRenderer.enabled = true;
    }
}
