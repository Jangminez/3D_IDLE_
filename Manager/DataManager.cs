using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Stage Data
    [SerializeField] private StageData[] stageDatas;
    private Dictionary<int, StageData> stageDataDict;

    // Monster Data
    [SerializeField] private Monster[] monsterPrefabs;
    private Dictionary<string, Monster> monsterDataDict;

    // Item Data
    [SerializeField] private ItemData[] itemDatas;
    private Dictionary<int, ItemData> itemDataDict;

    void Awake()
    {
        stageDatas = Resources.LoadAll<StageData>("Data/Stage");
        monsterPrefabs = Resources.LoadAll<Monster>("Data/Monster");
        itemDatas = Resources.LoadAll<ItemData>("Data/Item");

        stageDataDict = new Dictionary<int, StageData>();

        foreach (var stage in stageDatas)
        {
            stageDataDict.Add(stage.stageKey, stage);
        }

        monsterDataDict = new Dictionary<string, Monster>();

        foreach (var monster in monsterPrefabs)
        {
            monsterDataDict.Add(monster.name, monster);
        }

        itemDataDict = new Dictionary<int, ItemData>();

        foreach (var item in itemDatas)
        {
            itemDataDict.Add(item.itemKey, item);
        }
    }

    public StageData GetStageDataByStageKey(int stageKey)
    {
        if (stageDataDict.ContainsKey(stageKey))
        {
            return stageDataDict[stageKey];
        }

        return null;
    }

    public Dictionary<int, StageData> GetStageDict()
    {
        if (stageDataDict == null) return null;

        return stageDataDict;
    }

    public Monster GetMonsterByType(string type)
    {
        if (monsterDataDict.ContainsKey(type))
        {
            return monsterDataDict[type];
        }

        return null;
    }

    public ItemInstance GetItemByKey(int key)
    {
        if (itemDataDict.ContainsKey(key))
        {
            return new ItemInstance(itemDataDict[key]);
        }

        return null;
    }

    public Dictionary<int, ItemData> GetItemDict()
    {
        if (itemDataDict == null) return null;
        
        return itemDataDict;
    }
}
