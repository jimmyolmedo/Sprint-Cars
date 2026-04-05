using UnityEngine;

public class propeller : Item
{
    [SerializeField] float burstTime;
    public override void Use()
    {
        playerUser.BurstAcelerate(burstTime);
        base.Use();
    }
}
