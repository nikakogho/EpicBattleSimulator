using UnityEngine;
using System.Collections;

public class PassThrough : Melee
{
    public float moveAfterHitTime = 0.75f;
    bool marching = false;
    Vector3 dir;
    float moveAfterHitCountdown = 0;

    protected override bool RbKin
    {
        get
        {
            return false;
        }
    }

    void Update()
    {
        if (moveAfterHitCountdown > 0)
        {
            target = null;
            moveAfterHitCountdown -= Time.deltaTime;

            if (moveAfterHitCountdown <= 0)
            {
                moveAfterHitCountdown = 0;

                marching = false;
                anim.SetBool("moving", false);
            }
        }
    }

    protected override void Move(bool shouldAttack = false)
    {
        if (!marching)
        {
            if (target != null)
            {
                base.Move(shouldAttack);

                anim.SetBool("moving", true);
            }
        }
        else
        {
            rb.MovePosition(rb.position + dir * Time.fixedDeltaTime);
        }
    }

    protected override IEnumerator Attack(Troop target = null)
    {
        March();
        Hit(this.target);

        yield return null;
    }

    void March()
    {
        marching = true;
        moveAfterHitCountdown = moveAfterHitTime;
        dir = target.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        dir *= data.LevelBlueprint(currentLevel).moveSpeed;
    }

    void OnTriggerEnter(Collider col)
    {
        Troop troop = col.GetComponent<Troop>();

        if (troop == null || troop == target) return;

        if (troop.side != side)
        {
            Hit(troop);
        }
    }
}
