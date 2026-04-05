using UnityEngine;

public class BuyAcelerationUpgrade : BuyUpgrade
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Buy()
    {
        playerReference.AcelerationSpeed += UpgradeValue;
        Debug.Log($"ahora tu aceleracion es de {playerReference.AcelerationSpeed}");
        base.Buy();
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void TryBuy()
    {
        base.TryBuy();
    }

    public override void InitializePlayer(PlayerController player)
    {
        base.InitializePlayer(player);
    }
}
