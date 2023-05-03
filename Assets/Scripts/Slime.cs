using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{
    
    private Animator mAnimator;
    private BoxCollider2D mCollider;
    private bool alive = true;
    public GameObject powerBar;   
    public Image bar;

    private void Start() {
        mAnimator = GetComponent<Animator>();
        mCollider = GetComponent<BoxCollider2D>();

        powerBar = GameObject.Find("Bar");
        bar = powerBar.GetComponent<Image>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if (other.transform.tag == "Player" && alive)
        {
            ContactPoint2D contacto = other.GetContact(0);

            if (contacto.normal.y < -0.25f)
            {
                mAnimator.SetBool("IsDead", true);

                bar.fillAmount += 0.5f;

                alive = false;

                mCollider.size = new Vector2(mCollider.size.x, 0.2f);
                mCollider.offset = new Vector2(mCollider.offset.x, 0.1f);
            }
        }
    }

}
