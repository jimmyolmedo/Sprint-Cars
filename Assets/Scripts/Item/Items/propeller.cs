using UnityEngine;

public class propeller : Item
{
    [SerializeField] int burstTime;
    public override void Use()
    {
        playerUser.BurstAcelerate(burstTime);
        base.Use();
    }
}
