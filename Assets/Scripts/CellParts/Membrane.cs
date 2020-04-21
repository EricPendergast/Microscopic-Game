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
        Nucleus nucleus = null;
        foreach (SimplePart sibling in GetAll()) {
            if (sibling is Nucleus n) {
                nucleus = n;
            }
        }

        RelativeJoint2D conn = gameObject.GetComponent<RelativeJoint2D>();
        if (conn == null) {
            conn = gameObject.AddComponent<RelativeJoint2D>();
        }
        conn.connectedBody = nucleus.GetComponent<Rigidbody2D>();
        conn.linearOffset = new Vector2(Mathf.Cos(order*6.28f), Mathf.Sin(order*6.28f))*MembraneBalance.i.awaySpringDist;
        conn.maxForce = 20;
        conn.autoConfigureOffset = false;
        //conn.distance = MembraneBalance.i.immediateSpringDist*difference;
        //conn.frequency = MembraneBalance.i.immediateSpringFreq*difference;
        //conn.maxForce = MembraneBalance
    }

    public override int JointDesire(SimplePart other) {
        return 1;
    }
}
