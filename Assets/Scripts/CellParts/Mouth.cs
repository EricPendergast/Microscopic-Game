//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Mouth : Membrane {
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out SimplePart part)) {
            part.transform.parent = transform.parent;
        }
    }

    //public override float JointDesire(SimplePart other) {
    //    if (other is Membrane) {
    //        return base.JointDesire(other);
    //    }
    //    return -1;
    //}
}
