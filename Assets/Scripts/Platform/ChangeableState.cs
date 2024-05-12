using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer2D.Platform
{
    public class ChangeableState : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer stateColorless;
        [SerializeField] protected SpriteRenderer stateColor;

        protected virtual void Awake()
        {
            stateColorless = transform.GetChild(0).GetComponent<SpriteRenderer>();
            stateColor = transform.GetChild(1).GetComponent<SpriteRenderer>();
            stateColor.gameObject.SetActive(false);
        }

        public virtual void ChangeState()
        {   
            //Debug.Log("ChangeState");
            stateColor.gameObject.SetActive(true);
            stateColorless.gameObject.SetActive(false);
        }
    }
}

