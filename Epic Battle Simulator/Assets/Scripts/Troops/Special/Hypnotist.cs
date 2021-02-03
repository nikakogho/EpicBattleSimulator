using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hypnotist : Troop
{
    public float hypnotiseRange = 0;

    static Color hypnotisedByBlueColor = new Color(128, 0, 128, 1);
    static Color hypnotisedByRedColor = Color.yellow;

    List<Troop> victims = new List<Troop>();
    
    protected override IEnumerator Attack(Troop target = null)
    {
        yield return new WaitForSeconds(attackAfterTime);

        if(target != null)
        {
            anim.SetTrigger("Hypnotise");

            if (hypnotiseRange > 0)
            {
                foreach (var col in Physics.OverlapSphere(transform.position, hypnotiseRange, troopMask))
                {
                    var troop = col.GetComponent<Troop>();

                    if(troop.side != side)
                    {
                        Hypnotise(troop);
                    }
                }
            }
            else Hypnotise(target);
        }
    }

    void Hypnotise(Troop target)
    {
        target.side = side;

        target.army.troops.Remove(target);
        target.army = army;
        army.troops.Add(target);

        target.troopSign.GetComponentInChildren<SpriteRenderer>().color = side == PlaySide.Blue ? hypnotisedByBlueColor : hypnotisedByRedColor;

        victims.Add(target);

        TakeDamage(Damage);
    }

    void UnHypnotise(Troop victim)
    {
        victim.side = side == PlaySide.Blue ? PlaySide.Red : PlaySide.Blue;

        army.troops.Remove(victim);

        var enemyArmy = side == PlaySide.Blue ? Army.red : Army.blue;
        victim.army = enemyArmy;
        enemyArmy.troops.Add(victim);

        victim.troopSign.GetComponentInChildren<SpriteRenderer>().color = side == PlaySide.Blue ? Color.red : Color.blue;
    }

    protected override void Die()
    {
        foreach(var victim in victims)
        {
            if (victim != null) UnHypnotise(victim);
        }

        base.Die();
    }

    protected override void GizmoDraw()
    {
        if (data == null) return;

        Gizmos.color = side == PlaySide.Blue ? hypnotisedByBlueColor : hypnotisedByRedColor;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}
