using UnityEngine;

public class propeller : Item
{
    public override void Use()
    {
        playerUser.BurstAcelerate();
        base.Use();
    }
}
