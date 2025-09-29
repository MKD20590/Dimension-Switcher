using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public class titleSwitchButton : MonoBehaviour
{
    [SerializeField] private Animator titleTextAnimator;
    [SerializeField] private AudioSource bgm;

    bool isClicked = false;
    int audioBeat = 0;
    // Update is called once per frame
    void Update()
    {
        if (!isClicked)
        {
            if(bgm.time < 22)
            {
               audioBeat = 0;
            }
            else if (bgm.time > 22.7f+(24* audioBeat) && bgm.time <= 23f + (24 * audioBeat) &&audioBeat< 14)
            {
                audioBeat++;
                Click();
            }
        }
    }
    //for gameplay scene
    public void Click()
    {
        isClicked = true;
        this.gameObject.GetComponent<Animator>().SetTrigger("click");
    }
    //for gameplay scene
    public void ResetClick()
    {
        isClicked = false;
        this.gameObject.GetComponent<Animator>().ResetTrigger("click");
        titleTextAnimator.ResetTrigger("click");
    }
    //for main menu scene
    public void ChangeDimension()
    {
        titleTextAnimator.SetTrigger("click");
    }
}
