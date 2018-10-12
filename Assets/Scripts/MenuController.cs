using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private GameObject menuScreen;
    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private Image blackImage;
    private bool isFaded;
    
    private void Start()
    {
        isFaded = false;
        menuScreen.SetActive(true);
        loadingScreen.SetActive(false);
    }

    public void Update()
    {
        if(!isFaded)
        {
            Color tempColour = blackImage.color;
            tempColour.a -= 1f * Time.deltaTime;
            blackImage.color = tempColour;
            if (blackImage.color.a <= 0)
            {
                isFaded = true;
                blackImage.enabled = false;
            }
        }
    }
    public void PlayGame()
    {
        menuScreen.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadGameLevel());
    }

    IEnumerator LoadGameLevel()
    {
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(2);

        while(!loadProgress.isDone)
        {
            float progress = loadProgress.progress / 0.9f;
            slider.value = progress;

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
