using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Player
{
    // Класс представления игрового персонажа
    public class PlayerView : MonoBehaviour
    {
        // Ссылки на компоненты игрока
        private SpriteRenderer _sprite;
        private Animator _animator;
        private Rigidbody2D _rb;

        //свойство доступа к аниматору из др. классов
        public Animator Animator {get => _animator; set => _animator = value;}

        //ссылка на контроллер игрока
        private PlayerController controller;

        // Метод при инициализации объекта
        void Start()
        {
            //создание новой модели игрока и контроллера для управления пользовательским вводом
            PlayerModel model = new PlayerModel();
            controller = new PlayerController(model, this);
        }

        // Метод при пробуждении объекта
        private void Awake()
        {
            // получение ссылок на компоненты игрока на сцене из дочернего объекта - спрайта
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            // получение границ коллайдера с объекта на сцене
            _rb = GetComponent<Rigidbody2D>();
        }

        // Метод передвижения персонажа
        public void Run(Vector3 direction, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
            _sprite.flipX = direction.x < 0.0f; //rotate hero
        }

        // Метод прыжка персонажа
        public void Jump(Rigidbody2D rb, float jumpForce)
        {
            // применение силы прыжка
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }    

        public void FlipY()
        {
            _sprite.flipY = !_sprite.flipY;
        }    
    
        // Метод на каждом кадре
        void Update()
        {
            // метод обновления контроллера
            controller.Update();
        }

        // Метод на каждом физическом шаге
 //найти нижнюю точку коллайдера персонажа. От нее сделать влево и вправо небольшой отступ и по ним кидать вниз на короткое расстояние два рейкаста  (рейксастить прямоугольник )
 //boxCast - contactfilter (не считал персонажа) - он не должен быть шире размера персонажа
        void FixedUpdate()
        {
            // Находим нижнюю точку коллайдера персонажа и делаем небольшой отступ вверх
            Vector2 bottomPoint = new Vector2(GetComponent<Collider2D>().bounds.min.x, GetComponent<Collider2D>().bounds.min.y) + Vector2.up * 0.1f;

            // Делаем небольшой отступ влево и вправо
            Vector2 leftPoint = bottomPoint + new Vector2(-0.35f, 0);
            Vector2 rightPoint = bottomPoint + new Vector2(0.35f, 0);

            // Создаем ContactFilter2D, который не учитывает коллайдер персонажа
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.SetLayerMask(~(1 << gameObject.layer));
            contactFilter.useTriggers = false;

            // Кидаем вниз на короткое расстояние два рейкаста
            RaycastHit2D[] leftHit = new RaycastHit2D[1];
            RaycastHit2D[] rightHit = new RaycastHit2D[1];
            int leftHitCount = Physics2D.BoxCast(leftPoint, new Vector2(0.2f, 0.2f), 0f, Vector2.down, contactFilter, leftHit, 0.1f);
            int rightHitCount = Physics2D.BoxCast(rightPoint, new Vector2(0.2f, 0.2f), 0f, Vector2.down, contactFilter, rightHit, 0.1f);

            // Проверяем, находится ли персонаж на земле
            bool isGrounded = (leftHitCount > 0 && leftHit[0].collider != null) || (rightHitCount > 0 && rightHit[0].collider != null);
            Debug.Log("Is player grounded: " + isGrounded);
            controller.CheckGround(isGrounded);
        }




        void OnTriggerEnter2D(Collider2D other)
        {
            // Проверяем, является ли столкнувшийся объект триггером с тегом LowerWordLimit
            if (other.gameObject.CompareTag("LowerWordLimit"))
            {
                // Замораживаем координаты персонажа
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(controller.PlayerFell(true));
            }
        }
    }
}
