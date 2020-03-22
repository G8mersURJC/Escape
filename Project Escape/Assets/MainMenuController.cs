using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject goMainWindow;
    public GameObject goCreditsWindow;
    public GameObject goRankingWindow;


    public Sprite sMusicEnabled;
    public Sprite sMusicDisabled;

    public Button bMusic;

    private bool musicEnabled = true;


    public void OpenCreditsWindow()
    {
        goMainWindow.SetActive(false);
        goCreditsWindow.SetActive(true);
    }

    public void CloseCreditsWindow()
    {
        goMainWindow.SetActive(true);
        goCreditsWindow.SetActive(false);
    }

    public void OpenRankingWindow()
    {
        goMainWindow.SetActive(false);
        goRankingWindow.SetActive(true);
    }

    public void CloseRankingWindow()
    {
        goMainWindow.SetActive(true);
        goRankingWindow.SetActive(false);
    }

    public void MusicButtonPressed()
    {
        musicEnabled = !musicEnabled;

        bMusic.GetComponent<Image>().sprite = sMusicEnabled;
        if (!musicEnabled)
            bMusic.GetComponent<Image>().sprite = sMusicDisabled;
    }
}
