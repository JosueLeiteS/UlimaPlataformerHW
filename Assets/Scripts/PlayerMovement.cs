using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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
    private CapsuleCollider2D mCollider;
    private bool IsJumping = false;
    private bool IsFacingRight = true;
    private RaycastHit2D raycastHitRight;
    private RaycastHit2D raycastHitLeft;

    public GameObject powerBar;   
    public Image bar;

    private void Start()
    {
        mRb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mCollider = GetComponent<CapsuleCollider2D>();

        powerBar = GameObject.Find("Bar");
        bar = powerBar.GetComponent<Image>();
    }

    private void Update()
    {
        Vector2 raycastStart = new Vector2(transform.position.x + 0.6f, transform.position.y);

        raycastHitRight = Physics2D.Raycast(raycastStart, Vector2.right, powerDistance);
        raycastHitLeft = Physics2D.Raycast(raycastStart, Vector2.left, powerDistance);

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

        if (mCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
        if (mCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
        if (bar.fillAmount == 1)
        {
            if (IsFacingRight)
            {
                if (raycastHitRight.collider != null && raycastHitRight.collider.name == "Platforms")
                {
                    Debug.Log("MURIO");
                }else
                {
                    transform.Translate(powerDistance,0,0);
                }
            }else
            {
                if (raycastHitLeft.collider != null && raycastHitLeft.collider.name == "Platforms")
                {
                    Debug.Log("MURIO");
                }else
                {
                    transform.Translate(-(powerDistance),0,0);
                }
            }

            bar.fillAmount = 0;
        }
    }
}
