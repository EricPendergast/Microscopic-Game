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

        int siz = 3;
        if (siblings.Count < siz*2 + 1) {
            return;
        }

        siblings.Sort(delegate(Membrane m1, Membrane m2) {
            return m1.order > m2.order ? 1 : -1;
        });

        //Membrane left = null;
        //Membrane right = null;
        
        int me = 0;
        for (me = 0; me < siblings.Count; me++) {
            if (siblings[me] == this) {
                break;
            }
        }

        // TODO: Optimize this
        foreach (Membrane sibling in siblings) {
            GetCellGroup().DestroyJoint(this, sibling);
        }

        for (int j = -siz; j <= siz; j++) {
            Membrane nearSib = siblings[(me+j+siblings.Count)%siblings.Count];
            if (j == 0) {
                continue;
            }
            int difference = j < 0 ? -j : j;

            var conn = GetCellGroup().MakeJoint(this, nearSib);
            conn.distance = MembraneBalance.i.immediateSpringDist*difference;
            conn.frequency = MembraneBalance.i.immediateSpringFreq*difference;
        }

        //Assert.IsNotNull(left);
        //Assert.IsNotNull(right);
    }

    public override int JointDesire(SimplePart other) {
        return 1;
    }
}
