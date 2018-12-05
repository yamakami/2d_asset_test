using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : CharactorBase
{
    public Vector2 startPosition;
    public int originalHitPoint;
    public float jumpPower;
    [SerializeField] ContactFilter2D filter2d;

    MgrGame mgrGame;
    MgrScene mgrScene;

    bool isGround = true;
    bool isJump   = false;
    float jumpStart;

    bool isAttack = false;
    float attackEndTime;
    readonly float attackOverTime = 0.5f;

    protected override void Start()
    {
        base.Start();

        mgrGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MgrGame>();
        mgrScene = GameObject.FindGameObjectWithTag("GameManager").transform.Find("SceneManager").GetComponent<MgrScene>();


        hitpoint = originalHitPoint;
        attack   = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (MgrGame.inputBlock)
            return;

        if (isDamaged)
            return;

        float x = Input.GetAxis("Horizontal");
        bool jump = Input.GetKey(KeyCode.LeftShift);

        Move(x, jump);

        if (isAttack && attackEndTime < Time.time)
            AttackEnd();

        if (Input.GetKeyDown(KeyCode.Space) && !isAttack)
            Attack();
    }

    void Attack()
    {
        isAttack = true;
        attackEndTime = Time.time + attackOverTime;
        animator.SetBool("Attack", true);
    }

    void AttackEnd()
    {
        isAttack = false;
        attackEndTime = 0;
        animator.SetBool("Attack", false);
    }

    void Move(float move, bool jump)
    {
        if (Mathf.Abs(move) > 0)
        {
            direction = Mathf.Sign(move);
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.x, direction == 1 ? 0 : 180, rot.z);
        }

        rg2.velocity = new Vector2(move * moveSpeed, rg2.velocity.y);
        animator.SetFloat("Move", Mathf.Abs(move));

        if(isJump)
        {
            float timeDiff = Time.time - jumpStart;
            if (0.05f > timeDiff) return;
        }

        isGround = rg2.IsTouching(filter2d);
        if (isGround && !isJump && jump)
        {

            jumpStart = Time.time;
            animator.SetBool("Jump", true);
            isJump = true;

            rg2.AddForce(Vector2.up * jumpPower);
        }
        else if(isGround && isJump)
        {
            animator.SetBool("Jump", false);
            isJump = false;
        }
    }

    protected override void Damage(CharactorBase cb)
    {
        base.Damage(cb);

        float damageMove = direction * -1f;
        rg2.velocity = new Vector2(1 * damageMove, rg2.velocity.y);
    }

    protected override void Dead()
    {
        base.Dead();
    }

    protected override void DeadEnd()
    {
        MgrCanvas.lifeAmount--;
        if (0 <= MgrCanvas.lifeAmount)
        {
            //PlayerReset();
            mgrGame.GameStatus = MgrGame.GAMESTATUS.PLAYER_RESET;
            return;
        }
        mgrGame.GameStatus = MgrGame.GAMESTATUS.OVER;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            CharactorBase cb = collision.gameObject.GetComponent<CharactorBase>();
            Damage(cb);
        }
    }

    public void PlayerReset()
    {
        gameObject.SetActive(false);

        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        int layer = LayerMask.NameToLayer("Player");
        gameObject.layer = layer;

        hitpoint = originalHitPoint;
        isDamaged = false;

        gameObject.SetActive(true);
        animator.Play("Idle");
    }

    public float GetPlayerDirection()
    {
        return direction;
    }

    public int GetHitPoint()
    {
        return hitpoint;
    }
}
