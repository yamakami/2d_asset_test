using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
public abstract class CharactorBase : MonoBehaviour {

    [Range(0.1f, 2.0f)]
    public float moveSpeed;
    public int hitpoint;
    public int attack;

    protected float direction;
    protected bool isDamaged;

    protected Rigidbody2D rg2;
    protected Animator animator;

    // Use this for initialization
    protected virtual void Start ()
    {
        isDamaged = false;

        rg2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //protected virtual void Update()
    //{
    //}

    protected virtual void Damage(CharactorBase cb)
    {
        isDamaged = true;

        int superLayer = LayerMask.NameToLayer("SuperLayer");
        gameObject.layer = superLayer;

        hitpoint -= cb.attack;
        if (hitpoint < 1)
        {
            Dead();
            return;
        }

        animator.SetTrigger("Damage");
    }

    protected virtual void DamageEnd(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        gameObject.layer = layer;
        isDamaged = false;
    }

    protected virtual void Dead()
    {
        animator.SetTrigger("Dead");
    }

    protected virtual void DeadEnd()
    {
        Destroy(gameObject);
    }
}
