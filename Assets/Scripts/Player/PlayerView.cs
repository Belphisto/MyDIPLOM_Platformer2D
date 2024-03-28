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
        void FixedUpdate()
        {
            // Проверка, находится ли игрок на земле
            controller.CheckGround();
        }
    }
}
