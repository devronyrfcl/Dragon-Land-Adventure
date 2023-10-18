using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;   // Array of enemy prefabs to spawn
    public Transform[] spawnPoints;     // Array of spawn points
    public float spawnInterval = 2f;    // Time interval between spawns
    public int maxEnemies = 10;         // Maximum number of enemies to spawn

    private Transform playerTransform;

    private void Start()
    {
        // Find the player's transform by searching for the object with the "Player" tag
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Start spawning enemies
        StartCoroutine(SpawnEnemies());
        KillCountManager.Instance.ResetCurrentSeasonKillCount();
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)  // Keep spawning enemies as long as needed
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
            {
                // Get a random enemy prefab from the array
                GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                // Find the second closest spawn point to the player's position
                Transform secondClosestSpawnPoint = GetSecondClosestSpawnPoint();

                // Instantiate the random enemy prefab at the second closest spawn point
                Instantiate(randomEnemyPrefab, secondClosestSpawnPoint.position, Quaternion.identity);
            }

            // Wait for the specified spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Transform GetSecondClosestSpawnPoint()
    {
        Transform closestSpawn = null;
        Transform secondClosestSpawn = null;
        float closestDistance = Mathf.Infinity;
        float secondClosestDistance = Mathf.Infinity;

        foreach (Transform spawnPoint in spawnPoints)
        {
            float distance = Vector3.Distance(spawnPoint.position, playerTransform.position);

            if (distance < closestDistance)
            {
                secondClosestSpawn = closestSpawn;
                secondClosestDistance = closestDistance;

                closestSpawn = spawnPoint;
                closestDistance = distance;
            }
            else if (distance < secondClosestDistance)
            {
                secondClosestSpawn = spawnPoint;
                secondClosestDistance = distance;
            }
        }

        return secondClosestSpawn;
    }
}