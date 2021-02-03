using UnityEngine;
using System.Collections.Generic;

public static class TroopFunctions
{
    public static void Attack(int canHit, LayerMask troopMask, float range, float damage, Vector3 checkPoint, Troop target, PlaySide ourSide)
    {
        if (canHit == 1)
        {
            if(target != null)
            target.TakeDamage(damage);
            return;
        }

        Collider[] cols = Physics.OverlapSphere(checkPoint, range, troopMask);

        List<TroopCol> troopCols = new List<TroopCol>();

        foreach (Collider col in cols)
        {
            Troop troop = col.GetComponent<Troop>();

            if(troop.side != ourSide)
            troopCols.Add(new TroopCol(troop, col, checkPoint));
        }

        troopCols.Sort();

        for(int i = 0; i < canHit && i < troopCols.Count; i++)
        {
            troopCols[i].troop.TakeDamage(damage);
        }
    }
}

public class TroopCol : System.IComparable<TroopCol>
{
    public TroopCol(Troop troop, Collider col, Vector3 checkPoint)
    {
        this.troop = troop;
        this.col = col;

        dist = Vector3.Distance(checkPoint, troop.transform.position);
    }

    public Troop troop;
    public Collider col;
    private float dist;

    public int CompareTo(TroopCol other)
    {
        if (dist < other.dist) return 1;
        if (dist > other.dist) return -1;

        return 0;
    }
}