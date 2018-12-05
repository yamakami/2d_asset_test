using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skelton : CharactorBase
{
    [Range(0.1f, 1.0f)]
    public float moveRange;
    public bool withoutRise = false;

    float initialPositionX;
    float leftLimit;
    float rightLimit;

    readonly float facingLeft  = -1.0f;
    readonly float facingRight = 1.0f;

    bool isAnimationRiseEnd = false;

    Player player;
    Transform playerTrs;

    protected override void Start()
    {
        base.Start();

        hitpoint = 20;
        attack = 25;

        GameObject playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Player>();
        playerTrs = playerObj.GetComponent<Transform>();

        initialPositionX = transform.position.x;
        leftLimit  = initialPositionX - moveRange;
        rightLimit = initialPositionX + moveRange;

        direction = facingLeft;
    }

    void LateUpdate()
    {
        if (!withoutRise && !isAnimationRiseEnd)
            return;

        if(!isDamaged)
            Move();
    }

    void RiseEnd()
    {
        isAnimationRiseEnd = true;
    }

    void Move()
    {
        if (playerTrs && leftLimit <  playerTrs.position.x && playerTrs.position.x < rightLimit)
            direction = transform.position.x < playerTrs.position.x ? facingRight : facingLeft;
        else
        {
            if (transform.position.x < leftLimit)
                direction = facingRight;

            if (rightLimit < transform.position.x)
                direction = facingLeft;
        }

        Quaternion rot = transform.rotation;
        transform.rotation = Quaternion.Euler(rot.x, direction == facingLeft ? 0 : 180, rot.z);

        rg2.velocity = new Vector2(moveSpeed * direction, rg2.velocity.y);
        animator.SetBool("Work", true);
    }

    protected override void Damage(CharactorBase cb)
    {
        base.Damage(cb);
        Quaternion rot = transform.rotation;
        transform.rotation = Quaternion.Euler(rot.x, player.GetPlayerDirection() == facingLeft ? 180 : 0, rot.z);
    }

    protected override void DeadEnd()
    {

        if (!withoutRise)
            Destroy(transform.root.gameObject);
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.gameObject.tag == "PlayerSpear")
        {
            if (!withoutRise && !isAnimationRiseEnd)
                return;
                
            CharactorBase cb = colider.gameObject.transform.root.gameObject.GetComponent<CharactorBase>();
            Damage(cb);
        }
    }
}
