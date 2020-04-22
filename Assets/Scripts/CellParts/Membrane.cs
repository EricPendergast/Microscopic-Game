using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Membrane : SimplePart {
    private Membrane left;
    private Membrane right;
    public float order;
    

    void Awake() {
        order = Random.value;
    }

    public override void UpdateSprings() {
        // The set of all membranes
        var siblings = new List<Membrane>();
        foreach (SimplePart sibling in GetAll()) {
            if (sibling is Membrane m) {
                siblings.Add(m);
            }
        }

        int siz = 3;
        if (siblings.Count < siz*2 + 1) {
            return;
        }

        siblings.Sort(delegate(Membrane m1, Membrane m2) {
            return m1.order > m2.order ? 1 : -1;
        });

        int me = 0;
        for (me = 0; me < siblings.Count; me++) {
            if (siblings[me] == this) {
                break;
            }
        }

        // TODO
        foreach (var joint in GetComponents<RelativeJoint2D>()) {
            if (joint.connectedBody.gameObject.GetComponent<Membrane>() != null) {
                Destroy(joint);
            }
        }

        for (int j = -siz; j <= siz; j++) {
            if (j == 0) {
                continue;
            }
            Membrane nearSib = siblings[(me+j+siblings.Count)%siblings.Count];

            var joint = gameObject.AddComponent<RelativeJoint2D>();
            joint.connectedBody = nearSib.GetComponent<Rigidbody2D>();
            joint.linearOffset = new Vector2(1, 0) * MembraneBalance.i.immediateSpringDist * j;
            joint.autoConfigureOffset = false;
            joint.maxForce = MembraneBalance.i.immediateSpringFreq/Mathf.Abs(j);
            joint.maxTorque = 0;
            joint.enableCollision = true;
            //var conn = GetCellGroup().MakeJoint(this, nearSib);
        }
    }

    public override float JointDesire(SimplePart other) {
        return 1+order;
    }
}
