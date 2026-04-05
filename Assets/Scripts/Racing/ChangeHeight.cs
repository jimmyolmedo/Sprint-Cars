using UnityEngine;

public class ChangeHeight : MonoBehaviour
{
    GameObject obj;
    SpriteRenderer spriteRenderer;

    [SerializeField] int height;
    [SerializeField] int defaultLayer;
    [SerializeField] int sortingLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Ibridge _obj))
        {
            Debug.Log("tung tung sahur");
            _obj.SetComponent(ref obj, ref spriteRenderer);
            SwitchHeight(obj);
            spriteRenderer.sortingOrder = sortingLayer;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Ibridge _obj))
        {
            _obj.SetComponent(ref obj, ref spriteRenderer);
            obj.layer = defaultLayer;
            spriteRenderer.sortingOrder = 0;
        }
    }

    void SwitchHeight(GameObject obj)
    {
        obj.layer = height;
    }
}
