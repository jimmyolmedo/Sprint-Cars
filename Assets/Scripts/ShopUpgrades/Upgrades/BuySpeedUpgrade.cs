using UnityEngine;

public class BuySpeedUpgrade : BuyUpgrade
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Buy()
    {
        playerReference.MaxSpeed += UpgradeValue;
        Debug.Log($"ahora tu velocidad es de {playerReference.MaxSpeed}");
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
