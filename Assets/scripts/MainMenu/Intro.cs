using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] private AudioSource audioAwal;
    [SerializeField] private AudioSource audioLoop;
    static bool isOpen = false;

    public void PlayAudio()
    {
        audioAwal.Play();
    }
    public void PlayBgm()
    {
        audioLoop.Play();
        isOpen = true;
        this.gameObject.GetComponent<Animator>().SetBool("open", isOpen);
    }
}
