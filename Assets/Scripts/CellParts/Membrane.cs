using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Membrane : SimplePart {
    public Membrane prev = null;
    public JointWrapper nextJoint = null;

    public int minLoopSize = 5;

    MembraneDrawer drawer = null;

    public override void Start() {
        base.Start();
        if (nextJoint == null) {
            FindNext();
        }
    }

    public override void OnMouseOver() {
        base.OnMouseOver();
        if (Mouse.RightMouseDown()) {
            drawer = new MembraneDrawer(this);
            StartCoroutine(DraggingCoroutine());
        }
    }


    IEnumerator DraggingCoroutine() {
        while (Mouse.RightMouse() && drawer != null) {
            drawer.DoDraw();
            yield return null;
        }
    }

    void FindNext() {
        Assert.IsNull(nextJoint);

        Membrane closest = null;
        foreach (SimplePart sibling in nearby.nearby) {
            if (sibling != this && sibling is Membrane m) {
                if (closest == null || Distance(m) < Distance(closest)) {
                    if (m.prev == null && !m.GetNexts(minLoopSize).Contains(this)) {
                        closest = m;
                    }
                }
            }
        }

        if (closest != null) {
            ConnectTo(closest);
        }
    }

    List<Membrane> GetNexts(int numNext) {
        if (nextJoint == null || numNext == 0) {
            return new List<Membrane>();
        } else {
            Membrane n = nextJoint.GetConnected() as Membrane;
            List<Membrane> l = n.GetNexts(numNext - 1);
            l.Add(n);
            return l;
        }
    }

    public void ConnectTo(Membrane newNext) {
        Assert.IsNull(newNext.prev);
        Assert.IsNull(nextJoint);
        nextJoint = JointWrapper.MakeJoint(this, newNext);
        Assert.IsNotNull(nextJoint);
    }

    public void Disconnect() {
        Assert.IsNotNull(nextJoint);
        nextJoint.Destroy();
        Assert.IsNull(nextJoint);
    }

    public override void OnConnectedTo(JointWrapper joint) {
        if (joint.GetSource() is Membrane m) {
            Assert.IsNull(prev, "Unexpected joint connection");
            prev = m;
        }
    }

    public override void OnUnownedJointBroke(JointWrapper joint) {
        if (joint.GetSource() is Membrane m) {
            Assert.AreEqual(m, prev, "Unexpected joint connection broke");
            prev = null;
        }
    }

    public override void OnOwnedJointBroke(JointWrapper joint) {
        if (joint.GetConnected() is Membrane m) {
            Assert.AreEqual(joint, nextJoint);
            // This is not technically necessary, but it makes this field show
            // up as "null" rather than "missing" while in the editor
            nextJoint = null;
        }
    }

    public override void ConfigureJointConstants(JointWrapper wrap) {
        base.ConfigureJointConstants(wrap);
        var joint = wrap.GetOrMakeJoint<SpringJoint2D>();
        joint.frequency = MembraneBalance.i.immediateSpringFreq;
        joint.breakForce = MembraneBalance.i.immediateSpringBreakForce;
    }

    public override void UpdateSprings() {
        if (nextJoint != null) {
            nextJoint.Reconfigure();
        }
    }

    public override float JointDesire(SimplePart other) {
        return 3;
    }

    public override float GetNearbyRadius() {
        return GetRadius() + .1f;
    }

    public override void OnCellPartEnterNearby(SimplePart cp) {
        if (nextJoint == null) {
            FindNext();
        }
    }
}
