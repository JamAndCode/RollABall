using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Game Settings")]
    private GameSettings gameSettings;

    [Header("Enemy Spawning")]
    public Toggle enemyToggle;

    [Header("Number of Obstacles Input")]
    public TMP_InputField numCollectiblesInput;

    [Header("Jump Collectibles Slider")]
    public Slider jumpCollectiblesSlider;
    public TextMeshProUGUI bridgesPercentText;
    public TextMeshProUGUI columnPercentText;

    [Header("Number of Obstacles Input")]
    public TMP_InputField numObstaclesInput;

    [Header("Obstacles Slider")]
    public Slider obstaclesSlider;
    public TextMeshProUGUI bumperPercentText;
    public TextMeshProUGUI trapPercentText;

    private int numCollectibles;
    private int numObstacles;
    private bool spawnEnemy;

    void Start()
    {
        gameSettings = GameSettings.Instance;
        //Autofill inputs in case none were given
        numCollectibles = gameSettings.numCollectibles != 0 ? gameSettings.numCollectibles : 20;
        numObstacles = gameSettings.numNonScoreCollectibles != 0 ? gameSettings.numNonScoreCollectibles : 10;
        jumpCollectiblesSlider.value = gameSettings.bridgesPercent != 0 ? gameSettings.bridgesPercent : 50;
        obstaclesSlider.value = gameSettings.bumperPercent != 0 ? gameSettings.bumperPercent : 50;
        spawnEnemy = gameSettings.enemySpawning;

        //Update labels
        numObstaclesInput.text = numObstacles.ToString();
        numCollectiblesInput.text = numCollectibles.ToString();
        enemyToggle.isOn = spawnEnemy;
        JumpSlider();
        ObstacleSlider();
        //why must it update labels outside of method?? dumb and stupid
        bridgesPercentText.text = (100 - jumpCollectiblesSlider.value).ToString("F0") + "%";
        columnPercentText.text = jumpCollectiblesSlider.value.ToString("F0") + "%";
        bumperPercentText.text = (100 - obstaclesSlider.value).ToString("F0") + "%";
        trapPercentText.text = obstaclesSlider.value.ToString("F0") + "%";
    }
    
    public void StartGame()
    {
        gameSettings.InitializeSettings(numCollectibles, numObstacles, (int)jumpCollectiblesSlider.value, (int)obstaclesSlider.value, spawnEnemy);
        SceneManager.LoadScene(1);
    }

    public void EnemyToggle()
    {
        spawnEnemy = enemyToggle.isOn;
    }

    public void SetNumCollectibles(string num)
    {
        if (int.TryParse(num, out int parsedNum))
        {
            numCollectibles = parsedNum;
        }
    }

    public void SetNumNonScoreCollectibles(string num)
    {        
        if (int.TryParse(num, out int parsedNum))
        {
            numObstacles = parsedNum;
        }
    }

    public void JumpSlider()
    {
        UpdateSlider(jumpCollectiblesSlider, bridgesPercentText, columnPercentText);
    }

    public void ObstacleSlider()
    {
        UpdateSlider(obstaclesSlider, bumperPercentText, trapPercentText);
    }

    public void UpdateSlider(Slider slider, TextMeshProUGUI label1, TextMeshProUGUI label2)
    {
        slider.onValueChanged.AddListener((v) =>
        {
            float range = (slider.value / slider.maxValue) * 10;
            int ceil = Mathf.CeilToInt(range);
            int val = ceil * 10;
            slider.value = val;
            label1.text = (100 - val).ToString("F0") + "%";
            label2.text = val.ToString("F0") + "%";
        });
    }
    
}
