using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Player
{
    public class PlayerView : MonoBehaviour
    {
        private SpriteRenderer _sprite;
        private Animator _animator;
        private Rigidbody2D _rb;

        public Animator Animator {get => _animator; set => _animator = value;}

        private PlayerController controller;

        // Start is called before the first frame update
        void Start()
        {
            PlayerModel model = new PlayerModel();
            controller = new PlayerController(model, this);
        }

        private void Awake()
        {
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Run(Vector3 direction, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
            _sprite.flipX = direction.x < 0.0f; //rotate hero
        }

        public void Jump(Rigidbody2D rb, float jumpForce)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }        
    
        // Update is called once per frame
        void Update()
        {
            controller.Update();
        }

        void FixedUpdate()
        {
            controller.CheckGround();
        }
    }
}
