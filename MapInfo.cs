using Unity.AI.Navigation;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    [Header("Player SpawnPosition")]
    [SerializeField] Transform playerPosition;

    [Header("Monster SpawnPositions")]
    [SerializeField] Transform[] spawnPositions;

    void Awake()
    {
        var surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    public Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
        Vector3 randomPosition = spawnPosition + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

        return randomPosition;
    }

    public Vector3 GetPlayerPosition()
    {
        if (playerPosition == null) return Vector3.zero;

        return playerPosition.position;
    }
}
