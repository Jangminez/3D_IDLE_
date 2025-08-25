using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSlot : MonoBehaviour
{
    private StageUI stageUI;
    private StageData stageData;

    private Button stageButton;
    [SerializeField] TextMeshProUGUI stageText;

    public void Init(StageUI stageUI, StageData stageData)
    {
        this.stageUI = stageUI;
        this.stageData = stageData;

        stageText.text = stageData.stageKey.ToString();
        
        if (TryGetComponent(out stageButton))
        {
            stageButton.onClick.AddListener(SelectStage);
        }
    }

    private void SelectStage()
    {
        if (stageData == null) return;

        stageUI.StartStage(stageData);
    }
}
