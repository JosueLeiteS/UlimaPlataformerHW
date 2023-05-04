using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{
    [SerializeField]
    private float speed = -1f;
    private Rigidbody2D mRb;
    private float rayDistance = 2f;

    private Animator mAnimator;
    private BoxCollider2D mCollider;
    private bool IsAlive = true;

    private void Start() {
        mRb = GetComponent<Rigidbody2D>();

        mAnimator = GetComponent<Animator>();
        mCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() 
    {
        if (IsAlive)
        {
            mRb.velocity = new Vector2(
                speed,
                mRb.velocity.y
            );

            if (VerificaCaida() || VerificaChoque())
            {
                speed = -speed;
            }

            if (mCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                GameManager.Instance.PlayerDamage();
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {    
        if (other.transform.tag == "Player" && IsAlive)
        {
            ContactPoint2D contacto = other.GetContact(0);

            if (contacto.normal.y < -0.25f)
            {
                mAnimator.SetBool("IsDead", true);

                IsAlive = false;

                GameManager.Instance.PowerGain();

                mCollider.size = new Vector2(mCollider.size.x, 0.2f);
                mCollider.offset = new Vector2(mCollider.offset.x, 0.1f);
            }
        }
    }

    private bool VerificaCaida() 
    {
        var hit = Physics2D.Raycast(
            new Vector2(
                transform.position.x,
                transform.position.y + 0.5f
            ), 
            new Vector2(
                mRb.velocity.x < 0f ? -1 : 1, 
                -1f
            ),
            rayDistance,
            LayerMask.GetMask("Ground")
        );

        if (hit.collider != null)
        {
            return false;
        }else
        {
            return true;
        }
    }

    private bool VerificaChoque() 
    {
        var hit = Physics2D.Raycast(
            new Vector2(
                transform.position.x,
                transform.position.y + 0.5f
            ), 
            new Vector2(
                mRb.velocity.x < 0f ? -1 : 1, 
                0        
            ),
            rayDistance/3,
            LayerMask.GetMask("Ground")
        );

        if (hit.collider != null)
        {
            return true;
        }else
        {
            return false;
        }
    }

}
