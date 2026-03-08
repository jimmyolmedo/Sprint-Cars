using TMPro;
using UnityEngine;

public class BuyUpgrade : MonoBehaviour
{
    //variables
    [SerializeField] protected int UpgradeValue;
    [SerializeField] protected int startPrice;
    protected int price;
    [SerializeField] protected int upPrice;
    [SerializeField] protected string savePrice;
    protected PlayerController playerReference;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI currentCoins;

    //properties

    //methods

    protected virtual void OnEnable()
    {
        price = PlayerPrefs.GetInt(savePrice + playerReference.PlayerName, startPrice);
    }

    protected virtual void Start()
    {
        priceText.text = "-" + price.ToString();
        currentCoins.text = playerReference.CurrentCoins.ToString();
    }

    protected virtual void Update()
    {
        if (playerReference.CurrentCoins >= price) { priceText.color = Color.green; } else { priceText.color = Color.red; }
    }

    public virtual void TryBuy()
    {
        if(playerReference.CurrentCoins >= price && !LevelsManager.instance.Pressed)
        {
            Buy();
        }
    }

    protected virtual void Buy()
    {
        //aumentar el valor correspondiente
        playerReference.ChangeCoins(-price);
        price += upPrice;
        PlayerPrefs.SetInt(savePrice + playerReference.PlayerName, price);
        priceText.text = "-" + price.ToString();
        currentCoins.text = playerReference.CurrentCoins.ToString();

    }

    public virtual void InitializePlayer(PlayerController player)
    {
        playerReference = player;
    }
}
