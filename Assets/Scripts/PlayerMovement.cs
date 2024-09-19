using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;
    [SerializeField] private float invincibilityDuration = 1.0f;
    [SerializeField] private Vector2 knockBack = new Vector2(10f, 10f);
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip damageSound;

    private AudioSource audioSource;
    private BoxCollider2D feetCollider2D;
    private CapsuleCollider2D bodyCollider2D;
    private Animator animator;
    private Vector2 moveInput;
    private Rigidbody2D rigidbody2D;
    private float startingGravityScale;
    private bool isAlive = true;
    private bool isInvincible = false;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider2D = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();  

        startingGravityScale = rigidbody2D.gravityScale;
    }
    private void Update()
    {
        if (!isAlive) { return; }
        Run();
        Flip();
        ClimbLadder();
        CheckDamage();
    }
    private void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }
    private void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!feetCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (value.isPressed)
        {
            rigidbody2D.velocity += new Vector2(0f, jumpForce);
            audioSource.PlayOneShot(jumpSound);
        }
    }
    private void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }
    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rigidbody2D.velocity.y);
        rigidbody2D.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }
    private void Flip()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2D.velocity.x), 1f);
        }
    }
    private void ClimbLadder()
    {
        if (!bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rigidbody2D.gravityScale = startingGravityScale;
            animator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(rigidbody2D.velocity.x, moveInput.y * climbSpeed);
        rigidbody2D.velocity = climbVelocity;
        animator.SetBool("isClimbing", true);
        rigidbody2D.gravityScale = 0f;       
    }
    private void CheckDamage()
    {
        if (bodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazard")) && !isInvincible)
        {
            audioSource.PlayOneShot(damageSound);
            rigidbody2D.velocity = knockBack;
            StartCoroutine(HandleDamage());
        }
    }
    private IEnumerator HandleDamage()
    {
        isInvincible = true;
        GameSession.instance.ProcessPlayerHit();  
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(invincibilityDuration);  
        spriteRenderer.color = Color.white;
        isInvincible = false;  
    }
}
