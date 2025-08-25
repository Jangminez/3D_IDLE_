using System.Collections.Generic;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    private GameManager gameManager;
    private DataManager dataManager;

    [SerializeField] GameObject stageSlotPrefab;
    [SerializeField] Transform stageContent;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        dataManager = gameManager.DataManager;

        SetStage();
    }

    private void SetStage()
    {
        Dictionary<int, StageData> stageDict = dataManager.GetStageDict();

        foreach (var key in stageDict.Keys)
        {
            if (Instantiate(stageSlotPrefab, stageContent).TryGetComponent(out StageSlot stageSlot))
            {
                stageSlot.Init(this, stageDict[key]);
            }
        }
    }

    public void StartStage(StageData stageData)
    {
        gameManager.ChangeStage(stageData.stageKey);
    }
}
