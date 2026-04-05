using UnityEngine;

public class PlayerCollision : MonoBehaviour, Ibridge
{
    [SerializeField] PlayerController controller;

    public void SetComponent(ref GameObject obj, ref SpriteRenderer spriteRenderer)
    {
        obj = this.gameObject;
        spriteRenderer = controller.Sp;
    }
}
