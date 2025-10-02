using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private Animator loadingScreen;
    public static Loading instance;
    bool startLoading = false;
    bool stillLoading = false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        loadingScreen = GetComponent<Animator>();
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (stillLoading && AudioListener.volume > 0)
        {
            AudioListener.volume -= 0.02f;
        }
        else if(stillLoading && !startLoading && AudioListener.volume <= 0)
        {
            startLoading = true;
            StartCoroutine(LoadSceneAsynchronously(lvl));
        }
        if (!stillLoading && AudioListener.volume < 0.5f)
        {
            AudioListener.volume += 0.01f;
        }
    }

    //build index
    int lvl = 0;
    public void StartAnim(int lvlIdx)
    {
        lvl = lvlIdx;
        loadingBar.value = 0;
        loadingScreen.SetBool("in",true);
    }
    public void LoadScene()
    {
        stillLoading = true;
    }
    IEnumerator LoadSceneAsynchronously(int levelIdx)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIdx);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
        stillLoading = false;
        startLoading = false;
        loadingScreen.SetBool("in",false);
    }
}
