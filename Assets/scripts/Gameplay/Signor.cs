using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Signor : MonoBehaviour
{
    Animator animator; //Signor's animator
    [SerializeField] private Animator tutorial;
    [SerializeField] private AudioSource voice;

    Rigidbody rigidbody_3D;
    Rigidbody2D rigidbody_2D;
    BoxCollider collider_3D;
    BoxCollider2D collider_2D;

    [Header("3D")]
    [SerializeField] private Vector3 offsetCollider_3D;
    [SerializeField] private Vector3 sizeCollider_3D;

    [Header("2D")]
    [SerializeField] private Vector2 offsetCollider_2D;
    [SerializeField] private Vector2 sizeCollider_2D;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody_3D = GetComponent<Rigidbody>();
        collider_3D = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RemoveComponent()
    {
        if (FindObjectOfType<Player>().isDimension2D)
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<BoxCollider>());
            rigidbody_3D = null;
            collider_3D = null;
        }
        else
        {
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
            rigidbody_2D = null;
            collider_2D = null;
        }
        Invoke("AddComponent", 0.02f);
    }
    void AddComponent()
    {
        if (FindObjectOfType<Player>().isDimension2D)
        {
            rigidbody_2D = gameObject.AddComponent<Rigidbody2D>();
            rigidbody_2D.constraints = RigidbodyConstraints2D.FreezeAll;

            collider_2D = gameObject.AddComponent<BoxCollider2D>();
            collider_2D.isTrigger = true;
            collider_2D.size = sizeCollider_2D;
            collider_2D.offset = offsetCollider_2D;
        }
        else
        {
            rigidbody_3D = gameObject.AddComponent<Rigidbody>();
            rigidbody_3D.constraints = RigidbodyConstraints.FreezeAll;

            collider_3D = gameObject.AddComponent<BoxCollider>();
            collider_3D.isTrigger = true;
            collider_3D.size = sizeCollider_3D;
            collider_3D.center = offsetCollider_3D;
        }
    }

    public void Speak()
    {
        voice.Play();
    }

    //inherit from Monobehaviour class
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("talking", true);
            tutorial.SetBool("talking", true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("talking", true);
            tutorial.SetBool("talking", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            animator.SetBool("talking", false);
            tutorial.SetBool("talking", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("talking", false);
            tutorial.SetBool("talking", false);
        }
    }
}
