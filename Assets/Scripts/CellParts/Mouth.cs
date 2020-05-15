//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Mouth : SimplePart {
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out SimplePart part)) {
            if (part.transform.parent != transform.parent) {
                Destroy(part.gameObject);
            }
        }
    }

    public override float JointDesire(SimplePart other) {
        return 0;
    }
}
