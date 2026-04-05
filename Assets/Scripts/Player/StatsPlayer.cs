using UnityEngine;

public class StatsPlayer : Singleton<StatsPlayer>
{
    protected override bool persistent => false;

    [SerializeField] int maxSpeed;
    [SerializeField] int maxAcceleration;
    [SerializeField] int maxRotation;
    [SerializeField] int maxAttack;
    [SerializeField] int maxHealth;

    //properties
    public int MaxSpeed { get => maxSpeed;}
    public int MaxAcceleration { get => maxAcceleration;}
    public int MaxRotation { get => maxRotation;}
    public int MaxAttack { get => maxAttack;}
    public int MaxHealth { get => maxHealth;}
}
