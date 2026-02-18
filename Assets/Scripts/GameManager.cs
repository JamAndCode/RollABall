using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI gameOverText;
    public RawImage player1Controls;
    public RawImage player2Controls;
    public Button restartGame;
    public PlayerController player1;
    public PlayerController player2;
    private AudioSource gameOverSound;
    private WorldGeneration worldGen;

    void Start()
    {
        gameOverSound = GetComponent<AudioSource>();
        worldGen = GetComponent<WorldGeneration>();
        worldGen.Generate();
        StartCoroutine(HidePlayerControls());
    }


    //After 5 seconds, hide player control images. Maybe fade out later?
    private IEnumerator HidePlayerControls()
    {
        yield return new WaitForSeconds(5f);
        player1Controls.gameObject.SetActive(false);
        player2Controls.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        Destroy(GameObject.Find("Enemy(Clone)"));
        gameOverSound.Play();
        WhoWon();
        gameOverText.gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void WhoWon()
    {
        if (player1.count > player2.count)
        {
            gameOverText.text = "Game Over! Player 1 wins with " + player1.count + " points!";
        }
        else if (player2.count > player1.count)
        {
            gameOverText.text = "Game Over! Player 2 wins with " + player2.count + " points!";
        }
        else
        {
            gameOverText.text = "Game Over! It's a tie!";
        }
    }


}
