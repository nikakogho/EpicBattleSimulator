using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerPosFix : MonoBehaviour {

    void Awake()
    {
        //transform.position = transform.parent.position;
        Animator anim = GetComponent<Animator>();

        anim.applyRootMotion = false;
    }
}
