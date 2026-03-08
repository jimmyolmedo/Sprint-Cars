using UnityEngine;

public class ChangeHeight : MonoBehaviour
{
    [SerializeField] int height;
    [SerializeField] int defaultLayer;
    [SerializeField] int sortingLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            SwitchHeight(collision.gameObject);
            player.Sp.sortingOrder = sortingLayer;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            collision.gameObject.layer = defaultLayer;
            player.Sp.sortingOrder = 0;
        }
    }

    void SwitchHeight(GameObject obj)
    {
        obj.layer = height;
    }
}
