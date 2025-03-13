using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Slider healthSlider;
    public Slider levelSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currencyText;
    [SerializeField] private Player player;
    public Image gunIconImage;

    void Start()
    {
        player = FindObjectOfType<Player>();
         // Enable the controls
    }

    void Update()
    {
        PlayerHealthBar();
        PlayerKillCount();
        PlayerCurrencySystem();
        CurrentGunUI();
    // Update crosshair position
    }

    public void PlayerHealthBar()
    {
        if (player != null && healthSlider != null)
        {
            healthSlider.value = (player.health / player.maxHealth) * 100;
            if (healthText != null)
            {
                healthText.text = $"{player.health} / {player.maxHealth}";
            }
        }
    }

    public void PlayerCurrencySystem()
    {
        if (player != null && levelSlider != null && levelText != null)
        {
            if (player.xpToLevelUp != 0)
            {
                levelSlider.value = (float)player.currentXP / player.xpToLevelUp * 100;
                levelText.text = "LvL " + player.level.ToString();
            }
            else
            {
                levelSlider.value = 0;
            }
        }

        if (currencyText != null)
        {
            currencyText.text = player.currentCurrency.ToString();
        }
    }

    public void PlayerKillCount()
    {
        if (killText != null)
        {
            killText.text = player.killCount.ToString();
        }
    }

    public void CurrentGunUI()
    {
        if (player == null)
        {
            Debug.LogWarning("UIManager: Player reference is null!");
            return;
        }

        if (player.currentGun == null)
        {
            Debug.LogWarning("UIManager: Player's current gun is null!");
            return;
        }

        if (gunIconImage == null)
        {
            return;
        }

        gunIconImage.sprite = player.currentGun.gunIcon;
    }
}
