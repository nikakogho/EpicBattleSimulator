using System.Collections;
using UnityEngine;

public class Healer : Troop
{
    public float healRange;

    protected override IEnumerator Attack(Troop target = null)
    {
        if (target != null)
        {
            anim.SetTrigger("Heal");
            Heal(target);

            if (healRange > 0)
            {
                foreach (var col in Physics.OverlapSphere(target.transform.position, healRange, troopMask))
                {
                    var troop = col.GetComponent<Troop>();

                    if (troop != this && troop.Health < troop.Blueprint.health && troop.side == side)
                    {
                        Heal(troop);
                    }
                }
            }

            yield return new WaitForSeconds(attackAfterTime);
        }
        else
        {
            //fight?
        }
    }

    void Heal(Troop target)
    {
        target.GetHealed(Damage);
    }

    protected override void UpdateTarget()
    {
        Troop nearest = null;
        float minDist = float.MaxValue;

        foreach (Collider col in Physics.OverlapSphere(transform.position, 10000, troopMask))
        {
            Vector3 dir = col.transform.position - transform.position;

            float dist = dir.magnitude;

            if (dist < minDist)
            {
                Troop troop = col.GetComponent<Troop>();

                if (troop != this && troop.side == side && troop.Health < troop.Blueprint.health)
                {
                    nearest = troop;
                    minDist = dist;
                }
            }
        }

        target = nearest;
    }

    protected override void GizmoDraw()
    {
        if (data == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}
