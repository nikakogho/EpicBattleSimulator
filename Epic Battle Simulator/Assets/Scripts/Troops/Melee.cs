using UnityEngine;
using System.Collections;

public class Melee : Troop
{
    protected override IEnumerator Attack(Troop target = null)
    {
        if (target == null) target = this.target;
        anim.SetTrigger("hit");

        yield return new WaitForSeconds(attackAfterTime);

        Hit(target);
    }

    protected void Hit(Troop target)
    {
        if (target != null)
        {
            TroopFunctions.Attack(data.canHit, troopMask, data.range, Damage, transform.position, target, side);
        }
    }
}
