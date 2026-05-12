using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    PlayerController playerReference;

    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI playerLap;
    [SerializeField] Image winImage;
    [SerializeField] Image healthBar;
    [SerializeField] Image spriteItem;
    [SerializeField] TextMeshProUGUI playerCoins;
    [SerializeField] TextMeshProUGUI CoinFeedBack;
    [SerializeField] GameObject CoinFeedObj;
    public PlayerController PlayerReference { get => playerReference;}

    public void SetPlayer(PlayerController player)
    {
        playerReference = player;
    }

    private void OnEnable()
    {
        if(playerReference != null)
        {
            playerReference.onChangeCoin += changeCoin;
            playerReference.onGetItem += GetItem;
            PlayerReference.onLoseItem += LoseItem;
        }
    }

    private void OnDisable()
    {
        if(playerReference != null)
        {
            playerReference.onChangeCoin -= changeCoin;
            playerReference.onGetItem -= GetItem;
            PlayerReference.onLoseItem -= LoseItem;
        }
    }

    private void Update()
    {
        if (playerReference.Win)
        {
            winImage.gameObject.SetActive(true);
        }
        else
        {
            winImage.gameObject.SetActive(false);
        }
        if(playerReference != null)
        {
            playerName.text = playerReference.PlayerName;
            playerLap.text = $"{playerReference.CurrentLap.ToString()}/{RacingManager.instance.MaxLap}";
            playerCoins.text = playerReference.CurrentCoins.ToString();
            CalculateHealthPlayer();
        }
    }

    //calcula la barra de vida del jugador, ademas de cambiar el color si le queda mas o menos vida
    void CalculateHealthPlayer()
    {
        float currentHealth = playerReference.CurrentHealth;
        int maxHealth = playerReference.MaxHealth;

        float healtValue = currentHealth / maxHealth;//calculo de la vida actual / por la vida maxima

        healthBar.fillAmount = healtValue; //devuelve un numero de 0 a 1, con eso cambia la barra de vida

        if(healtValue > 0.5f)//si esta por encima de la mitad color verde
        {
            healthBar.color = Color.green;
        }

        else if(healtValue < 0.5f && healtValue > 0.3f)//si esta por debajo de la mitad pero le queda mas de un tercio, color amarillo
        {
            healthBar.color = Color.yellow;
        }
        else { healthBar.color = Color.red;}//le queda un tercio o menos, color rojo
        
    }

    void changeCoin(int value)
    {
        CoinFeedObj.SetActive(false);
        if(value > 0)
        {
            CoinFeedBack.color = Color.green;
            CoinFeedBack.text = "+" + value.ToString();
        }
        else
        {
            CoinFeedBack.color = Color.red;
            CoinFeedBack.text = "-" + value.ToString();
        }
        CoinFeedObj.SetActive(true);

    }

    void GetItem(SOitem _item)
    {
        spriteItem.sprite = _item.icon;
        spriteItem.gameObject.SetActive(true);
    }

    void LoseItem()
    {
        spriteItem.sprite = null;
        spriteItem.gameObject.SetActive(false);
    }
}
