using UnityEngine;
using System.Collections;

public class Kamikaze : Troop
{
    public float explosionRange;

    protected override IEnumerator Attack(Troop target = null)
    {
        yield return new WaitForSeconds(attackAfterTime);
        Die();
    }

    protected override void GizmoDraw()
    {
        base.GizmoDraw();

        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    protected override void Die()
    {
        TroopFunctions.Attack(data.canHit, troopMask, explosionRange, Damage, transform.position, null, side);

        base.Die();
    }
}
