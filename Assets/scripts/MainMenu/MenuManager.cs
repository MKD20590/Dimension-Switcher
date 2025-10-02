using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    Loading loading;
    [Header("Animator")]
    [SerializeField] private Animator daisy;
    [SerializeField] private Animator cooper;
    [SerializeField] private Animator signor;
    [SerializeField] private Animator canvasAnimator;
    [SerializeField] private Animator titleSpriteAnimator;
    CameraManager cameraManager;
    [Header("Audio settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource sfx_click;
    [SerializeField] private AudioSource sfx_hover;

    [SerializeField] private Slider bgm_slider;
    [SerializeField] private Slider sfx_slider;
    // Start is called before the first frame update
    void Start()
    {
        loading = Loading.instance;
        cameraManager = FindObjectOfType<CameraManager>();
        Time.timeScale = 1;
        AudioListener.pause = false;
        //audio settings
        if(!PlayerPrefs.HasKey("bgm") || !PlayerPrefs.HasKey("sfx"))
        {
            PlayerPrefs.SetFloat("bgm", 1);
            PlayerPrefs.SetFloat("sfx", 1);
        }
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
    }

    //menu options
    public void HoverSlider(AudioSource SFX_hover)
    {
        SFX_hover.Play();
    }
    public void Hover(GameObject button)
    {
        if(button.GetComponent<Button>().interactable)
        {
            sfx_hover.Play();
        }
    }
    public void StartGame()
    {
        daisy.SetBool("in",true);
        cooper.SetBool("in",true);
        signor.SetBool("in",true);
        titleSpriteAnimator.Play("out");
        canvasAnimator.Play("out");
        cameraManager.PlayAnim(true);
    }
    public void StartLevel(int level)
    {
        loading.StartAnim(level);
    }
    public void BackTitle()
    {
        daisy.SetBool("in", false);
        cooper.SetBool("in", false);
        signor.SetBool("in", false);
        titleSpriteAnimator.Play("in");
        canvasAnimator.Play("in");
        cameraManager.PlayAnim(false);
    }
    public void QuitGame()
    {
        sfx_click.Play();
        Application.Quit();
    }
}
