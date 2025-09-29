using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class Clouds : MonoBehaviour
{
    [SerializeField] private float opacity = 0f;
    [Range(0.1f, 2f)]
    [SerializeField] private float cloudSpeed = 0.5f;

    [SerializeField] private Renderer[] childRenderer;
    [SerializeField] private SpriteRenderer[] childSpriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isAnimatorMirrored = false;

    // Start is called before the first frame update
    void Start()
    {
        childRenderer = GetComponentsInChildren<Renderer>();
        childSpriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.speed = cloudSpeed;
        //reverse animation
        if(isAnimatorMirrored)
        {
            animator.Play("clouds(mirror)");
        }
        //get materials color
        foreach (Renderer renderer in childRenderer)
        {
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                Color color = renderer.sharedMaterials[i].color;
                color.a = 0;
                renderer.material.color = color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //fade in-out
        if (childRenderer != null)
        {
            foreach (Renderer renderer in childRenderer)
            {
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    Color color = renderer.sharedMaterials[i].color;
                    color.a = Mathf.Lerp(color.a, opacity, 3 * Time.deltaTime);
                    renderer.material.color = color;
                }
            }
        }
    }
}
