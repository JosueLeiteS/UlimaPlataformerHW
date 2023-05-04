using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float runSpeed = 4f;
    [SerializeField]
    private float jumpSpeed = 10f;
    [SerializeField]
    private float powerDistance = 4f;

    private Vector2 mMoveInput;
    private Rigidbody2D mRb;
    private Animator mAnimator;
    private SpriteRenderer mRenderer;
    private CapsuleCollider2D mCollider;
    private bool IsJumping = false;
    private bool IsFacingRight = true;
    private bool IsAlive = true;
    private RaycastHit2D raycastHit;
    private RaycastHit2D raycastHitLeft;

    private LayerMask ground;

    public GameObject powerBar;   
    public Image powerBarImage;

    private void Start()
    {
        GameManager.Instance.OnPlayerDeath += OnPlayerDeathDelegate;

        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mCollider = GetComponent<CapsuleCollider2D>();
        mRenderer = GetComponent<SpriteRenderer>();

        powerBar = GameObject.Find("PowerBar");
        powerBarImage = powerBar.GetComponent<Image>();

        ground = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if(!IsAlive)
        {
            return;
        }

        mRb.velocity = new Vector2(
            mMoveInput.x * runSpeed,
            mRb.velocity.y
        );

        if (Mathf.Abs(mRb.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(mRb.velocity.x),
                transform.localScale.y,
                transform.localScale.z
            );
            
            if(Mathf.Sign(mRb.velocity.x) < 0)
            {
                IsFacingRight = false;
            } else 
            {
                IsFacingRight = true;
            }

            // Idle -> Running
            mAnimator.SetBool("IsRunning", true);
        }else {
            mAnimator.SetBool("IsRunning", false);
        }

        if (mRb.velocity.y < 0f)
        {
            mAnimator.SetBool("IsJumping", false);
            mAnimator.SetBool("IsFalling", true);
        }

        if (mCollider.IsTouchingLayers(ground))
        {
            // Toco el suelo
            mAnimator.SetBool("IsFalling", false);
        }
    }

    private void OnMove(InputValue value)
    {
        mMoveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        // Verificar si estamos en pleno salto o no
        if (mCollider.IsTouchingLayers(ground))
        {
            if (value.isPressed)
            {
                // Saltar
                mRb.velocity = new Vector2(
                    mRb.velocity.x,
                    jumpSpeed
                );
                mAnimator.SetBool("IsJumping", true);
            }
        }
    }

    private void OnPower(InputValue value)
    {
        if (powerBarImage.fillAmount == 1)
        {
            raycastHit = Physics2D.Raycast(
                transform.position, 
                IsFacingRight ? Vector2.right : Vector2.left, 
                powerDistance, 
                ground.value
            );

            if(raycastHit.collider != null)
            {
                transform.Translate(
                    IsFacingRight ? powerDistance : -powerDistance,
                    0,
                    0
                );
                GameManager.Instance.PlayerDeath();
            } else 
            {
                transform.Translate(
                    IsFacingRight ? powerDistance : -powerDistance,
                    0,
                    0
                );
            }
            powerBarImage.fillAmount = 0;
        }
    }

    private void OnPlayerDeathDelegate(object sender, EventArgs e)
    {
        GameManager.Instance.PlayerDrain();

        mRb.bodyType = RigidbodyType2D.Static;
        mAnimator.SetTrigger("HasDied");

        IsAlive = false;
    }

    private void DisableSprite()
    {
        mRenderer.enabled = false;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
