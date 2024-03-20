using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Model;
using Platformer2D.View;

namespace Platformer2D.Controller
{
    public class HeroController : MonoBehaviour
    {
        private HeroModel model;
        private HeroView view;

        private Rigidbody2D rb;
        private bool isGrounded = false;

        // Определение события делегата
        public delegate void ScoreUpdateEvent(int score);
        public static event ScoreUpdateEvent OnScoreUpdate;

        private static HeroController _instance;
        public static HeroController Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            view = GetComponentInChildren<HeroView>(); 
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            model = new HeroModel();
        }

        // Update is called once per frame
        void Update()
        {
            if (isGrounded) view.UpdateView(transform.position, isGrounded, States.idle);
            
            if (Input.GetButton("Horizontal"))
                Run();
            if (isGrounded && Input.GetButtonDown("Jump"))
                Jump();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        private void Run()
        {
            if (isGrounded) view.UpdateView(transform.position, isGrounded, States.go);
        
            Vector3 dir = transform.right * Input.GetAxis("Horizontal");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, model.Speed * Time.deltaTime);
            view.ViewFlip(dir.x < 0.0f);
        }
        
        private void Jump()
        {
            rb.AddForce(transform.up * model.JumpForce, ForceMode2D.Impulse);
        }
        
        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.9f);
            isGrounded = colliders.Length > 1;
        
            if (!isGrounded) view.UpdateView(transform.position, isGrounded, States.jump);
        }
        
        public void GetScore(int point)
        {
            model.IncrementScore(point);
            Debug.Log(model.Score);
            OnScoreUpdate?.Invoke(model.Score);
        }
    }

}

