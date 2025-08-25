using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    DataManager dataManager;

    public void Init(DataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public Monster SpawnMonster(string monsterType, Vector3 spawnPosition)
    {
        Monster monster = dataManager.GetMonsterByType(monsterType);

        if (monster != null)
        {
            GameObject obj = Instantiate(monster.gameObject, spawnPosition, Quaternion.identity);
            return obj.GetComponent<Monster>();
        }

        return null;
    }
}
