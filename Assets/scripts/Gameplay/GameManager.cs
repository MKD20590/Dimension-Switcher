using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    Loading loading;
    public bool isPaused = false;
    public static bool isMouseBusy = false; //unused - it's for cutscene only 
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Animator gameOverScreen; //child 0 = win screen, child 1 = lose screen

    [SerializeField] private AudioSource sfx_win;
    [SerializeField] private AudioSource sfx_lose;

    [Header("Camera")]
    [SerializeField] private Volume globalVolume;

    [Header("Audio settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource bgm;

    [SerializeField] private AudioSource buttonHover;
    [SerializeField] private AudioSource buttonClick;

    [SerializeField] private Slider bgm_slider;
    [SerializeField] private Slider sfx_slider;

    public bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        Time.timeScale = 1;
        loading = FindObjectOfType<Loading>();
        pauseMenu.SetActive(false);
        //audio settings
        bgm_slider.value = PlayerPrefs.GetFloat("bgm");
        sfx_slider.value = PlayerPrefs.GetFloat("sfx");
    }

    // Update is called once per frame
    void Update()
    {
        //audio settings
        PlayerPrefs.SetFloat("bgm", bgm_slider.value);
        PlayerPrefs.SetFloat("sfx", sfx_slider.value);
        audioMixer.SetFloat("bgm", Mathf.Log10(PlayerPrefs.GetFloat("bgm")) * 20);
        audioMixer.SetFloat("sfx", Mathf.Log10(PlayerPrefs.GetFloat("sfx")) * 20);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
    public void Lose()
    {
        sfx_lose.Play();
        isGameOver = true;
        Time.timeScale = 0;
        gameOverScreen.Play("in");
        gameOverScreen.transform.GetChild(0).gameObject.SetActive(false);
        gameOverScreen.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void Win()
    {
        sfx_win.Play();
        isGameOver = true;
        Time.timeScale = 0;
        gameOverScreen.Play("in");
        gameOverScreen.transform.GetChild(0).gameObject.SetActive(true);
        gameOverScreen.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void Hover()
    {
        buttonHover.Play();
    }
    public void HoverSlider(AudioSource hover2)
    {
        hover2.Play();
    }
    //isPaused options
    public void Pause()
    {
        if(!isGameOver)
        {
            isPaused = !isPaused;
            if(isPaused)
            {
                Hover();
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            pauseMenu.SetActive(isPaused);
        }
    }
    public void Restart()
    {
        buttonClick.Play();
        loading.StartAnim(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMenu()
    {
        buttonClick.Play();
        loading.StartAnim(0);
    }
}
