using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [Header("Game Configuration")]
    public int numCollectibles;
    public int numNonScoreCollectibles;
    public int bridgesPercent;
    public int bumperPercent;
    public bool enemySpawning;

    private void Awake()
    {
        // Singleton pattern - ensure only one instance exists across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeSettings(int collectibles, int obstacles, int bridges, int bumpers, bool enemy)
    {
        numCollectibles = collectibles;
        numNonScoreCollectibles = obstacles;
        bridgesPercent = bridges;
        bumperPercent = bumpers;
        enemySpawning = enemy;
    }
}
