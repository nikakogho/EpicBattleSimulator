using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Troop))]
public class TroopEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Troop troop = (Troop)target;

        if(troop.data != null && troop.data.type != TroopData.TroopType.Epic)
        {
            troop.currentLevel = EditorGUILayout.IntField("Level", troop.currentLevel);
        }

        if (troop.data != null && GUILayout.Button("Turn Into Troop"))
        {
            CapsuleCollider col = troop.GetComponent<CapsuleCollider>();
            if (col == null) col = troop.gameObject.AddComponent<CapsuleCollider>();

            col.height = 2.5f;
            col.radius = 0.5f;
            col.center = Vector3.up * 1.2f;

            Rigidbody rb = troop.GetComponent<Rigidbody>();
            if (rb == null) rb = troop.gameObject.AddComponent<Rigidbody>();

            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.freezeRotation = true;

            troop.gameObject.layer = LayerMask.NameToLayer("Troop");
            troop.gameObject.name = troop.data.name;

            if (troop.currentLevel > 1) troop.gameObject.name += " level " + troop.currentLevel;
        }

        if(GUILayout.Button("Edit Troop Data"))
        {
            EditorWindow.GetWindow<TroopDataWindow>("Troop Data Editor").data = troop.data;
        }
    }
}

[CustomEditor(typeof(Melee))]
public class MeleeEditor : TroopEditor
{
    
}

[CustomEditor(typeof(PassThrough))]
public class PassThroughEditor : MeleeEditor
{

}

[CustomEditor(typeof(Shooter))]
public class ShooterEditor : TroopEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Shooter shooter = (Shooter)target;

        if (shooter.troopAnimType == TroopAnimType.German)
        {
            shooter.weaponName = EditorGUILayout.TextField("Weapon Name", shooter.weaponName);
            shooter.capitalS = EditorGUILayout.Toggle("Capital S", shooter.capitalS);
        }

        if (shooter.diesAfterTime)
        {
            shooter.timeForDeath = EditorGUILayout.FloatField("Time For Death", shooter.timeForDeath);
        }
    }
}

[CustomEditor(typeof(Kamikaze))]
public class KamikazeEditor : TroopEditor
{

}

[CustomEditor(typeof(Healer))]
public class HealerEditor : TroopEditor
{

}

[CustomEditor(typeof(Hypnotist))]
public class HypnotistEditor : TroopEditor
{

}