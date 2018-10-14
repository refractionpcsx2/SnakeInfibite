using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    [SerializeField]
    private int score = 0;
    [SerializeField]
    private int lives = 3;
    [SerializeField]
    private Text livesText;
    [SerializeField]
    private Text resurrectText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text gameOverScoreText;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private Text pauseScoreText;
    [SerializeField]
    private List<AudioClip> resurrectSpeech;
    private AudioSource audioSource;
    private bool isPaused;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
        isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                Time.timeScale = 0;
                pauseScoreText.text = score.ToString();
                pauseScreen.SetActive(true);
                isPaused = true;
            }
            else
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1;
                isPaused = false;
            }
        }
    }
    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        scoreText.text = score.ToString();
    }

    public bool TakeLife()
    {
        int sampleToPlay = lives;
        audioSource.clip = resurrectSpeech[sampleToPlay];
        audioSource.Play();
        livesText.text = lives.ToString();
        if (lives == 0)
        {
            gameOverScoreText.text = score.ToString();
            gameOverScreen.SetActive(true);
            return false;
        }
        else
        {            
            lives -= 1;
            StartCoroutine(ResurrectionFlash());
            return true;
        }
    }

    IEnumerator ResurrectionFlash()
    {
        for(int i = 0; i < 6; i++)
        {
            resurrectText.enabled = !resurrectText.enabled;
            yield return new WaitForSeconds(0.5f);
        }
        resurrectText.enabled = false;
        yield return null;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
