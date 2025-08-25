using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    private Player player;

    [SerializeField] TextMeshProUGUI maxHealthText;
    [SerializeField] TextMeshProUGUI attackDamageText;
    [SerializeField] TextMeshProUGUI attackSpeedText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI moveSpeedText;

    public void Init(Player player)
    {
        this.player = player;

        SetStat();
    }

    void OnEnable()
    {
        if (player == null) return;

        SetStat();
    }

    private void SetStat()
    {
        maxHealthText.text = player.TotalMaxHealth.ToString();
        attackDamageText.text = player.TotalAttackDamage.ToString();
        attackSpeedText.text = player.TotalAttackSpeed.ToString();
        defenseText.text = player.TotalDefense.ToString();
        moveSpeedText.text = player.TotalSpeed.ToString();
    }
}
