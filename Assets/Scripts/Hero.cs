using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 3f; // speed move
    [SerializeField] private int score = 0; // score counter
    [SerializeField] private float jumpForce = 2f; // Force jump
    private bool isGrounded = false;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    public static Hero Instance {get;set;} //singltone

    private States State
    {
        get { return (States)animator.GetInteger("state"); }
        set { animator.SetInteger("state", (int)value); }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(State);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded) State = States.idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();      
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        Instance = this;
    }

    private void FixedUpdate()
    {
        CheckGroud();
        
    }

    private void Run()
    {
        if (isGrounded) State = States.go;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position =Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f; //rotate hero
    }
    
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGroud()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.9f);
        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = States.jump;
        
    }

    public void GetScore(int point)
    {
        score += point;
        Debug.Log(score);
    }
}
