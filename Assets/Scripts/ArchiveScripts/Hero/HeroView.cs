using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.View
{
    public class HeroView : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private Animator animator;
        //private Rigidbody2D rb;

        /*public Rigidbody2D Rb
        {
            get {return rb;}
            set {rb = value;}
        }*/

        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        public void ViewFlip(bool flip)
        {
            sprite.flipX = flip;
        }
        public void UpdateView(Vector3 position, bool isGrounded, States state)
        {
            transform.position = position;
            //sprite.flipX = rb.velocity.x < 0.0f;
            //sprite.flipX = flip
            animator.SetInteger("state", (int)state);
        }


        /*public void RunAnimation(Vector3 direction)
        {
            sprite.flipX = direction.x < 0.0f;
            animator.SetInteger("state", (int)States.go);
        }

        public void IdleAnimation()
        {
            animator.SetInteger("state", (int)States.idle);
        }

        public void JumpAnimation()
        {
            animator.SetInteger("state", (int)States.jump);
        }*/
    }
    
}

