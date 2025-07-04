using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;


    public float dashDistance = 3f;
    public float dashCooldown = 1f;
    public float dashDuration = 0.1f;
    private bool isDashing;
    private float lastDashTime;


    public GameObject attackArea;            
    public float attackDuration = 0.3f;
    public Vector2 attackOffset = new Vector2(0.2f, 0f); 


    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public AudioClip attackSound;
    public AudioClip footstepSound;
    public AudioClip dashSound;
    private AudioSource audioSource;
    private bool isWalkingSoundPlaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attackArea.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        if (movement.sqrMagnitude > 0.01f)
            lastDirection = movement;

        animator.SetFloat("MoveX", lastDirection.x);
        animator.SetFloat("MoveY", lastDirection.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("IsAttack");
            StartCoroutine(PerformAttack());

            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }

        if (attackArea != null)
        {
            Vector2 offset = Vector2.zero;
            float rotationZ = 0f;

            if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
            {
                
                if (lastDirection.x > 0)
                {
                    offset = new Vector2(attackOffset.x, 0);    
                    rotationZ = 0f;
                }
                else
                {
                    offset = new Vector2(-attackOffset.x, 0);   
                    rotationZ = 180f;
                }
            }
            else if (Mathf.Abs(lastDirection.y) > 0)
            {
                
                if (lastDirection.y > 0)
                {
                    offset = new Vector2(0, attackOffset.x);   
                    rotationZ = 90f;
                }
                else
                {
                    offset = new Vector2(0, -attackOffset.x);   
                    rotationZ = -90f;
                }
            }

            if (movement.sqrMagnitude > 0.01f)
            {
                if (!isWalkingSoundPlaying && footstepSound != null)
                {
                    audioSource.clip = footstepSound;
                    audioSource.loop = true;
                    audioSource.Play();
                    isWalkingSoundPlaying = true;
                }
            }
            else
            {
                if (isWalkingSoundPlaying)
                {
                    audioSource.Stop();
                    isWalkingSoundPlaying = false;
                }
            }

            attackArea.transform.localPosition = offset;
            attackArea.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
        }

    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        audioSource.PlayOneShot(dashSound);

        spriteRenderer.color = Color.cyan;

        Vector2 dashTarget = rb.position + lastDirection * dashDistance;
        rb.MovePosition(dashTarget);

        yield return new WaitForSeconds(dashDuration);

        spriteRenderer.color = Color.white;
        isDashing = false;
    }

    IEnumerator PerformAttack()
    {
        if (attackArea != null)
        {
            attackArea.SetActive(true);
            yield return new WaitForSeconds(attackDuration);
            attackArea.SetActive(false);
        }
    }
}
