using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private GameManager gameManager;
    private DataManager dataManager;
    private MonsterSpawner monsterSpawner;
    private Player player;

    [SerializeField] private Transform stageParent;

    private List<Monster> waveMonsters = new List<Monster>();
    private StageData stageData;
    private MapInfo mapInfo;

    private Coroutine stageCoroutine;
    private int curStageKey;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
        player = gameManager.Player;
        dataManager = gameManager.DataManager;

        monsterSpawner = GetComponentInChildren<MonsterSpawner>();

        if (monsterSpawner)
            monsterSpawner.Init(dataManager);
    }

    public void SetStage(int stageKey)
    {
        if (stageCoroutine != null)
        {
            StopAllCoroutines();

            foreach (var monster in waveMonsters)
            {
                Destroy(monster.gameObject);
            }

            waveMonsters.Clear();
            mapInfo = null;
        }

        curStageKey = stageKey;
        StartStage();
    }

    private void StartStage()
    {
        stageData = dataManager.GetStageDataByStageKey(curStageKey);

        if (stageData != null)
        {
            SetMap();
            player.StartStage(mapInfo.GetPlayerPosition());
            stageCoroutine = StartCoroutine(StageCoroutine());
        }
    }

    private void SetMap()
    {
        if (stageParent.childCount > 0)
        {
            Destroy(stageParent.GetChild(0).gameObject);
        }

        mapInfo = Instantiate(stageData.stagePrefab, stageParent).GetComponent<MapInfo>();
    }

    IEnumerator StageCoroutine()
    {
        foreach (var wave in stageData.waves)
        {
            SpawnWave(wave, () =>
            {
                player.SetMonsters(waveMonsters);
            });

            yield return new WaitUntil(() => IsWaveCleared());
            yield return new WaitForSeconds(wave.delayToNextWave);
        }

        if (stageData.hasBoss)
        {
            SpawnBoss(stageData.bossType, () =>
            {
                player.SetMonsters(waveMonsters);
            });
            
            yield return new WaitUntil(() => IsWaveCleared());
        }

        yield return new WaitForSeconds(3f);
        StartStage();
    }

    private void SpawnWave(WaveData wave, Action onCompleted)
    {
        waveMonsters.Clear();

        foreach (var spawn in wave.monsterSpawns)
        {
            for (int i = 0; i < spawn.count; i++)
            {
                Monster monster = monsterSpawner.SpawnMonster(spawn.monsterType, mapInfo.GetRandomSpawnPosition());
                monster.Init(this, player, curStageKey);

                waveMonsters.Add(monster);
            }
        }

        onCompleted?.Invoke();
    }

    private void SpawnBoss(string bossType, Action onCompleted)
    {
        Monster boss = monsterSpawner.SpawnMonster(bossType, mapInfo.GetRandomSpawnPosition());
        waveMonsters.Add(boss);

        onCompleted?.Invoke();
    }

    private bool IsWaveCleared()
    {
        return waveMonsters.Count == 0;
    }

    public void RemoveMonster(Monster monster)
    {
        if (waveMonsters.Contains(monster))
            waveMonsters.Remove(monster);
    }
}
