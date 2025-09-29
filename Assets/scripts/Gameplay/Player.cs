using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : Entity, IDamageable
{
    GameManager gameManager;
    [SerializeField] private AudioSource sfx_walking;
    [SerializeField] private AudioSource sfx_jumping;
    [SerializeField] private AudioSource sfx_landing;
    [SerializeField] private GameObject camera2D_target; //for the 2D camera to follow player's x and y axis only
    [SerializeField] private GameObject daisy;
    [SerializeField] private Animator switcher; //button UI
    [SerializeField] private GameObject warning;

    [Header("camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CameraManager cameraManager;

    [Header("Player stats")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float playerToGroundDistance;

    [Header("Rigidbodies")]
    [SerializeField] private Rigidbody rigidbody_3D;
    [SerializeField] private Rigidbody2D rigidbody_2D;

    [Header("Colliders")]
    [SerializeField] private BoxCollider collider_3D;
    [SerializeField] private BoxCollider2D collider_2D;

    [Header("Dimension type")]
    public bool isDimension2D = false;

    [Header("3D")]
    [SerializeField] private PhysicMaterial colliderMaterial3D;
    [SerializeField] private Vector3 offsetCollider3D;
    [SerializeField] private Vector3 sizeCollider3D;
    Vector3 movement3D = Vector3.zero;
    RaycastHit hit3D;

    [Header("2D")]
    [SerializeField] private PhysicsMaterial2D colliderMaterial2D;
    [SerializeField] private Vector2 offsetCollider2D;
    [SerializeField] private Vector2 sizeCollider2D;
    Vector2 movement2D = Vector2.zero;
    //RaycastHit2D hit2D;

    Animator animator;

    bool isTransitioning = false;
    bool isCollidingWithGround = false;
    bool is2D = false;
    bool is3D = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = daisy.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        collider_3D = GetComponent<BoxCollider>();
        rigidbody_3D = GetComponent<Rigidbody>();
        mainCamera.orthographic = isDimension2D;
        cameraManager = FindObjectOfType<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        camera2D_target.transform.position = new Vector3(transform.position.x, transform.position.y, camera2D_target.transform.position.z);

        //3D ground check
        Ray groundRay3D = new Ray(transform.position, Vector3.down);
        if (!isDimension2D && Physics.Raycast(groundRay3D, out hit3D, 1 << LayerMask.NameToLayer("Ground")))
        {
            playerToGroundDistance = hit3D.distance - 1.5f;
        }

        if (!isDimension2D && playerToGroundDistance <= 0.15f)
        {
            isGrounded = true;
        }
        else if(!isDimension2D && playerToGroundDistance > 0.15f)
        {
            isGrounded = false;
        }

        //2D ground check
        else if(isDimension2D)
        {
            playerToGroundDistance = 100;
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeDimension();
        }

        //rigidbody management
        //2D movement
        if(isDimension2D && !isTransitioning && !gameManager.isGameOver)
        {
            if (!is2D)
            {
                is2D = true;
                is3D = false;
                Destroy(GetComponent<Rigidbody>());
                Destroy(rigidbody_3D);
                rigidbody_3D = null;

                Destroy(GetComponent<BoxCollider>());
                Destroy(collider_3D);
                collider_3D = null;

                add_2D = true;
                Invoke("AddComponent", 0.01f);
            }
            if(rigidbody_2D)
            {
                if(Input.GetAxisRaw("Horizontal") != 0)
                {
                    movement2D.x = Input.GetAxisRaw("Horizontal") * speed;
                }
                else if(Input.GetAxisRaw("Vertical") != 0)
                {
                    movement2D.x = Input.GetAxisRaw("Vertical") * speed;
                }
                else
                {
                    movement2D.x = 0;
                }
                if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    movement2D.y = jumpForce;
                    rigidbody_2D.velocity = movement2D;
                }
                else
                {
                    movement2D.y = 0;
                    rigidbody_2D.velocity = new Vector2(movement2D.x, rigidbody_2D.velocity.y);
                }
            }
        }
        //3D movement
        else if(!isDimension2D && !isTransitioning && !gameManager.isGameOver)
        {
            if (!is3D)
            {
                is3D = true;
                is2D = false;
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(rigidbody_2D);
                rigidbody_2D = null;

                Destroy(GetComponent<BoxCollider2D>());
                Destroy(collider_2D);
                collider_2D = null;

                add_2D = false;
                Invoke("AddComponent",0.01f);
            }
            if(rigidbody_3D)
            {
                movement3D.z = -Input.GetAxisRaw("Horizontal") * speed;
                movement3D.x = Input.GetAxisRaw("Vertical") * speed;
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    movement3D.y = jumpForce;
                    rigidbody_3D.velocity = movement3D;
                }
                else
                {
                    movement3D.y = 0;
                    rigidbody_3D.velocity = new Vector3(movement3D.x, rigidbody_3D.velocity.y, movement3D.z);
                }
            }
        }

        //position
        daisy.transform.position = transform.position;

        //rotation
        if(isDimension2D && movement2D.x != 0)
        {
            Vector3 rotation;
            rotation = new Vector3(movement2D.x * speed, 0, 0);
            rotation.Normalize();
            Quaternion rotate = Quaternion.LookRotation(rotation);
            daisy.transform.rotation = Quaternion.RotateTowards(daisy.transform.rotation, rotate, 600f * Time.deltaTime);
        }
        else if(!isDimension2D)
        {
            if(movement3D.x != 0 || movement3D.z != 0)
            { 
                Vector3 rotation = movement3D * speed;
                rotation.y = 0f;
                rotation.Normalize();
                Quaternion rotate = Quaternion.LookRotation(rotation, Vector3.up);
                daisy.transform.rotation = Quaternion.RotateTowards(daisy.transform.rotation, rotate, 600f * Time.deltaTime);
            }
        }

        //animations
        //walking
        if (movement2D.x != 0 || movement3D.x != 0 || movement3D.z != 0)
        {
            animator.SetBool("walking",true);
        }
        else
        {
            animator.SetBool("walking", false);
        }
        //jumping
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetTrigger("jump");
            JumpSfx();
        }
        //while in the air
        animator.SetBool("air", !isGrounded);
    }
    public void WalkingSfx()
    {
        sfx_walking.pitch = Random.Range(0.9f, 1.2f);
        sfx_walking.Play();
    }
    public void JumpSfx()
    {
        sfx_jumping.Play();
    }
    public void LandSfx()
    {
        animator.ResetTrigger("jump");
        sfx_landing.Play();
    }

    public void ChangeDimension()
    {
        if(isGrounded && !isTransitioning && !gameManager.isGameOver)
        {
            isDimension2D = !isDimension2D;
            
            switcher.ResetTrigger("change");
            switcher.SetTrigger("change");

            //change Signor's collider and rigidbody type
            Signor signor = FindObjectOfType<Signor>();
            signor.RemoveComponent();

            //if isTransitioning is true, then the player won't be able to move
            isTransitioning = true;
            movement2D = Vector2.zero;
            movement3D = Vector3.zero;
            if (rigidbody_2D)
            {
                rigidbody_2D.velocity = movement2D;
            }
            if (rigidbody_3D)
            {
                rigidbody_3D.velocity = movement3D;
            }
            cameraManager.PlayAnim(isDimension2D);
            CancelInvoke("CameraDone");
            Invoke("CameraDone", 1.5f);
        }    
    }
    public void ButtonHover(bool hover)
    {
        if (hover)
        {
            warning.GetComponent<Image>().color = Color.white;
        }
        else
        {
            warning.GetComponent<Image>().color = Color.clear;
        }
    }

    //camera done isTransitioning
    void CameraDone()
    {
        isTransitioning = false;
    }

    //adding rigidbody and collider component (2D or 3D) - because 2D components can't be added alongside 3D components
    bool add_2D;
    void AddComponent()
    {
        if(add_2D)
        {
            rigidbody_2D = gameObject.AddComponent<Rigidbody2D>();
            rigidbody_2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            rigidbody_2D.sharedMaterial = colliderMaterial2D;

            collider_2D = gameObject.AddComponent<BoxCollider2D>();
            collider_2D.size = sizeCollider2D;
            collider_2D.offset = offsetCollider2D;
        }
        else
        {
            rigidbody_3D = gameObject.AddComponent<Rigidbody>();
            rigidbody_3D.constraints = RigidbodyConstraints.FreezeRotation;

            collider_3D = gameObject.AddComponent<BoxCollider>();
            collider_3D.material = colliderMaterial3D;
            collider_3D.size = sizeCollider3D;
            collider_3D.center = offsetCollider3D;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        collisionController(collision, null);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionController(null,collision);
    }
    void collisionController(Collider collision3D, Collider2D collision2D)
    {
        if (collision3D != null && collision3D.tag == "Obstacle"
            ||
            collision2D != null && collision2D.tag == "Obstacle")
        {
            TakeDamage(3);
            if (healthPoints <= 0)
            {
                gameManager.Lose();
            }
        }
        else if (collision3D != null && collision3D.tag == "Cooper"
            ||
            collision2D != null && collision2D.tag == "Cooper")
        {
            gameManager.Win();
        }
        else if (collision3D != null && collision3D.tag == "Ground"
            ||
            collision2D != null && collision2D.tag == "Ground")
        {
            gameManager.Win();
        }
    }
}
