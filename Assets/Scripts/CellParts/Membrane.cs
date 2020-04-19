using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Membrane : SimplePart {
    private Membrane left;
    private Membrane right;
    private float order;
    

    void Awake() {
        order = Random.value;
    }

    public override void UpdateSprings() {
        //Assert.AreEqual(left == null, right == null);
        //if (left != null) {
            //return;
        //}
        //base.UpdateSprings();
        // The set of all membranes
        var siblings = new List<Membrane>();
        foreach (SimplePart sibling in GetAll()) {
            if (sibling is Membrane m) {
                siblings.Add(m);
            }
        }

        if (siblings.Count < 5) {
            return;
        }

        siblings.Sort(delegate(Membrane m1, Membrane m2) {
            return m1.order > m2.order ? 1 : -1;
        });

        //Membrane left = null;
        //Membrane right = null;

        var near = new List<Membrane>();

        for (int i = 0; i < siblings.Count; i++) {
            if (siblings[i] == this) {
                for (int j = -2; j <= 2; j++) {
                    near.Add(siblings[(i+j+siblings.Count)%siblings.Count]);
                }
                break;
            }
        }

        //Assert.IsNotNull(left);
        //Assert.IsNotNull(right);

        foreach (Membrane sibling in siblings) {
            if (sibling == near[1] || sibling == near[3]) {
                var conn = GetCellGroup().MakeJoint(this, sibling);
                conn.distance = MembraneBalance.i.immediateSpringDist;
                conn.frequency = MembraneBalance.i.immediateSpringFreq;
            } else if (sibling != this) {
                //if (sibling == near[0] || sibling == near[4]) {
                var conn = GetCellGroup().MakeJoint(this, sibling);
                conn.distance = MembraneBalance.i.awaySpringDist;
                conn.frequency = MembraneBalance.i.awaySpringFreq;
            }// else {
                //GetCellGroup().DestroyJoint(this, sibling);
            ///}
        }
    }

    public override int JointDesire(SimplePart other) {
        return 1;
    }
}
