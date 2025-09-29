using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
//using UnityEngine.Rendering.PostProcessing;
using static Unity.Burst.Intrinsics.X86.Avx;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private AudioSource SFX_click;
    GameManager gameManager;
    ColorAdjustments color;
    Bloom bloom;
    LensDistortion lensDistortion;
    ChromaticAberration chromaticAberration;
    Vignette vignette;
    [Range(-10f, 0f)]
    [SerializeField] private float colorExposure;
    [Range(0f, 5f)]
    [SerializeField] private float bloomIntensity;
    [Range(-0.5f, 0.5f)]
    [SerializeField] private float lensDistortionIntensity;
    [Range(0f,1f)]
    [SerializeField] private float chromaticAberrationIntensity;
    [Range(0f,1f)]
    [SerializeField] private float vignetteIntensity;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private CinemachineVirtualCamera camera3D;
    [SerializeField] private CinemachineVirtualCamera camera2D;
    CinemachineVirtualCamera currentCamera;
    bool isDimensionType2D;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        gameManager = FindObjectOfType<GameManager>();
        currentCamera = camera3D;
        if (camera2D == currentCamera)
        {
            camera2D.Priority = 20;
            camera3D.Priority = 10;
        }
        else if (camera3D == currentCamera)
        {
            camera2D.Priority = 10;
            camera3D.Priority = 20;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //camera switch effects
        if(globalVolume.profile.TryGet<Bloom>(out bloom))
        {
            bloom.intensity.value = bloomIntensity;
        }
        if (globalVolume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.intensity.value = lensDistortionIntensity;
        }
        if (globalVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            chromaticAberration.intensity.value = chromaticAberrationIntensity;
        }
        if (globalVolume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = vignetteIntensity; ;
        }
        if (globalVolume.profile.TryGet<ColorAdjustments>(out color))
        {
            color.postExposure.value = colorExposure;
        }

        if (camera2D == currentCamera)
        {
            camera2D.Priority = 20;
            camera3D.Priority = 10;
        }
        else if (camera3D == currentCamera)
        {
            camera2D.Priority = 10;
            camera3D.Priority = 20;
        }
    }
    public void PlayAnim(bool is2D)
    {
        //the 'if' condition will work if the currentCamera animation state's name is idle
        if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            isDimensionType2D = is2D;
            this.GetComponent<Animator>().Play("change");
        }
    }
    public void SwitchCamera()
    {
        if (isDimensionType2D)
        { 
            currentCamera = camera2D;
        }
        else
        {
            currentCamera = camera3D;
        }
    }
    public void Click()
    {
        SFX_click.Play();
    }
}
