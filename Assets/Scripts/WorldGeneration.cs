using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldGeneration : MonoBehaviour
{
    public LayerMask excludeLayer = new LayerMask();

    [Header("World Spawning")]
    public GameObject enemyPrefab;
    public GameObject floorCollectible;
    public List<GameObject> jumpCollectibles;
    public List<GameObject> obstacles;
    public int maxCollectibles;
    private int totalCollectiblesScore;
    private int jumpCollectiblesScore;
    public int maxObstacles;
    private int numObstacles;
    private int xCordRange = 9;
    private int zCordRange = 9;
    
    [Header("Spawn Probabilities")]
    public int bridgesPercent;
    public int bumperPercent;
    private int[] jumpObstaclesSpawnChanceArray;
    private int[] obstaclesSpawnChanceArray;

    public void Generate()
    {
        GameSettings settings = GameSettings.Instance;
        //enemy generation
        if (settings.enemySpawning) {
            Instantiate(enemyPrefab, new Vector3(Random.Range(-xCordRange, xCordRange), 0.5f, Random.Range(-zCordRange, zCordRange)), Quaternion.identity);
        }
        maxCollectibles = settings.numCollectibles - 1; //load-bearing collectible
        bridgesPercent = settings.bridgesPercent;
        jumpObstaclesSpawnChanceArray = GenerateSpawnChanceArray(bridgesPercent);
        GenerateCollectibles();
        maxObstacles = settings.numNonScoreCollectibles;
        bumperPercent = settings.bumperPercent;
        obstaclesSpawnChanceArray = GenerateSpawnChanceArray(bumperPercent);
        GenerateObstacles();
    }

    private int[] GenerateSpawnChanceArray(int percentChange)
    {
        int[] spawnChanceArray = new int[10];
        for (int i = 0; i < spawnChanceArray.Length; i++)
        {
            spawnChanceArray[i] = (i < percentChange / 10) ? 1 : 0;
        }
        return spawnChanceArray;
    }

    private void GenerateCollectibles() {
        int maxAttempts = 50; // Maximum attempts to prevent infinite loops
        int attempts = 0;
        while (totalCollectiblesScore < maxCollectibles && attempts < maxAttempts) {
            if (jumpCollectiblesScore < maxCollectibles / 2) {
                int listIndex = Random.Range(0, jumpObstaclesSpawnChanceArray.Length);
                GameObject toSpawn = jumpCollectibles[jumpObstaclesSpawnChanceArray[listIndex]];
                float yCord = toSpawn.CompareTag("ColumnCollectible") ? 0.5f : 1f; 
                CheckCollision(new Vector3(Random.Range(-xCordRange, xCordRange), yCord, Random.Range(-zCordRange, zCordRange)), 0.5f, toSpawn);
            } else {
                CheckCollision(new Vector3(Random.Range(-xCordRange, xCordRange), 1, Random.Range(-zCordRange, zCordRange)), 0.5f, floorCollectible);
            }
            attempts++;
        }
    }

    private void GenerateObstacles() {
        int maxAttempts = 50;
        int attempts = 0;
        while (numObstacles < maxObstacles && attempts < maxAttempts) {
            int listIndex = Random.Range(0, obstaclesSpawnChanceArray.Length);
            GameObject toSpawn = obstacles[obstaclesSpawnChanceArray[listIndex]];
            float yCord = toSpawn.CompareTag("Trap") ? 0.05f : 0.5f;
            CheckCollision(new Vector3(Random.Range(-xCordRange, xCordRange), yCord, Random.Range(-zCordRange, zCordRange)), 0.5f, toSpawn);
            attempts++;
        }
    }

    private void CheckCollision(Vector3 coordinates, float radius, GameObject toSpawn) {
        int maxRetries = 20;
        int retries = 0;

        // Create a layer mask that includes all layers except the excluded one
        int layerMask = ~excludeLayer;

        while (retries < maxRetries) {
            //TODO: look into replacing this with checking with colliders rather than overlapsphere
            //currently doesn't account for how big the bridge is and allows collisions
            Collider[] hitColliders = Physics.OverlapSphere(coordinates, radius, layerMask);
            if (hitColliders.Length == 0) {
                GameObject spawnObject = Instantiate(toSpawn, coordinates, Quaternion.identity);
                //rotate for variety
                spawnObject.transform.Rotate(0, Random.Range(0, 360), 0);

                // Add NavMeshObstacle to all objects except floor collectibles (PickUp) and Trap
                if (!toSpawn.CompareTag("PickUp") && !toSpawn.CompareTag("Trap")) {
                    NavMeshObstacle meshObstacle = spawnObject.AddComponent<NavMeshObstacle>();
                    meshObstacle.shape = NavMeshObstacleShape.Box;
                    meshObstacle.carving = true;

                    // Match the NavMeshObstacle size to the object's collider
                    Collider objCollider = spawnObject.GetComponent<Collider>();
                    if (objCollider != null) {
                        Vector3 colliderSize = objCollider.bounds.size;
                        meshObstacle.size = colliderSize;
                        meshObstacle.center = objCollider.bounds.center - spawnObject.transform.position;
                    }
                }

                IncreaseCollectibleCounts(toSpawn);
                return;
            }
            //TODO: this sucks. Fix or make pretty
            float yCord = toSpawn.CompareTag("Trap") ?  0.05f : !toSpawn.CompareTag("BridgeCollectible") ? 0.5f : 1f; 
            coordinates = new Vector3(Random.Range(-xCordRange, xCordRange), yCord, Random.Range(-zCordRange, zCordRange));
            retries++;
        }

        // exhausted retries, log a warning
        Debug.LogWarning("Failed to find a valid spawn location for collectible after " + maxRetries + " attempts");
    }

    private void IncreaseCollectibleCounts(GameObject collectible) {
        if (collectible.CompareTag("ColumnCollectible") || collectible.CompareTag("BridgeCollectible")) {
            int increaseCount = collectible.CompareTag("ColumnCollectible") ? 1 : 2;
            //have to increase both counts because the collectible is both a jump collectible and a regular collectible
            jumpCollectiblesScore += increaseCount;
            totalCollectiblesScore += increaseCount;
        } else if (collectible.CompareTag("PickUp")) {
            totalCollectiblesScore++;
        } else
        {
            numObstacles++;
        }
    }
}
