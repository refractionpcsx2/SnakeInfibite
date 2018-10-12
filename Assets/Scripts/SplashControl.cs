using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashControl : MonoBehaviour {
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image blackImage;
    [SerializeField]
    private List<Sprite> splashImages;
    [SerializeField]
    private float fadeSpeed = 1f;
    [SerializeField]
    private float stayTime = 2f;
    private float currentStayTime;
    private float fadeState;


    // Use this for initialization
    void Start () {
        image.sprite = splashImages[0];
	}
	
	// Update is called once per frame
	void Update () {
		if(fadeState == 0 || fadeState == 3)
        {
            Color tempColour = blackImage.color;
            tempColour.a -= fadeSpeed * Time.deltaTime;
            blackImage.color = tempColour;
            if (blackImage.color.a <= 0)
            {
                fadeState++;
                currentStayTime = stayTime;
            }
        }
        if (fadeState == 1 || fadeState == 4)
        {
            currentStayTime -= Time.deltaTime;
            if (currentStayTime <= 0f)
            {
                fadeState++;
            }
        }
        if (fadeState == 2 || fadeState == 5)
        {
            Color tempColour = blackImage.color;
            tempColour.a += fadeSpeed * Time.deltaTime;
            blackImage.color = tempColour;
            if (blackImage.color.a >= 1.0f)
            {
                image.sprite = splashImages[1];
                fadeState++;
            }
        }
        if(fadeState == 6)
        {
            SceneManager.LoadScene(1);
        }
    }

}
