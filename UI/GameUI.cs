using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerEventHandler playerEvent;
    [SerializeField] Image expBar;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI stageText;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        playerEvent = gameManager.Player.Events;

        OnEnable();
    }

    void OnEnable()
    {
        if (playerEvent == null) return;

        playerEvent.onLevelChanged += UpdateLevelUI;
        playerEvent.onExpChanged += UpdateExpUI;
        playerEvent.onGoldChanged += UpdateGoldUI;
        gameManager.onStageChanged += UpdateStageUI;
    }

    void OnDisable()
    {
        playerEvent.onLevelChanged -= UpdateLevelUI;
        playerEvent.onExpChanged -= UpdateExpUI;
        playerEvent.onGoldChanged -= UpdateGoldUI;
        gameManager.onStageChanged -= UpdateStageUI;
    }

    private void UpdateLevelUI(int level)
    {
        levelText.text = $"Lv.{level}";
    }

    private void UpdateExpUI(int currentExp, int requiredExp)
    {
        expBar.fillAmount = (float)currentExp / (float)requiredExp;
    }

    private void UpdateGoldUI(int gold)
    {
        goldText.text = gold.ToString();
    }

    private void UpdateStageUI(int stageKey)
    {
        stageText.text = stageKey.ToString();
    }
}
