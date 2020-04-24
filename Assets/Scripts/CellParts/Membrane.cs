using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Membrane : SimplePart {
    public Membrane prev = null;
    public JointWrapper nextJoint = null;

    public int minLoopSize = 5;

    MembraneDrawer drawer = null;

    new void Start() {
        base.Start();
        if (nextJoint == null) {
            FindNext();
        }
    }

    new void OnMouseOver() {
        base.OnMouseOver();
        if (Input.GetMouseButtonDown(1)) {
            drawer = new MembraneDrawer(this);
            StartCoroutine(DraggingCoroutine());
        }
    }

    new void OnMouseUp() {
        base.OnMouseUp();
        if (Input.GetMouseButtonUp(1)) {
            drawer = null;
        }
    }

    public override void OnConnectedTo(JointWrapper joint) {
        if (joint.GetSource() is Membrane m) {
            prev = m;
            joint.onBreak.AddListener(() => {
                prev = null;
            });
        }
    }

    IEnumerator DraggingCoroutine() {
        while (Input.GetMouseButton(1) && drawer != null) {
            drawer.DoDraw();
            yield return null;
        }
    }

    void FindNext() {
        Assert.IsNull(nextJoint);

        Membrane closest = null;
        foreach (SimplePart sibling in GetNearby(MembraneBalance.i.immediateMaxDist)) {
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

    public void ConnectTo(Membrane newNext) {
        Assert.IsNull(newNext.prev);

        nextJoint = GetCellGroup().MakeJoint(this, newNext);
        Assert.IsNotNull(nextJoint);

        MembraneBalance.ConfigureJointConstants(nextJoint.joint);
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

    public override void UpdateSprings() {
        if (nextJoint == null) {
            FindNext();
        } else {
            MembraneBalance.ConfigureJointConstants(nextJoint.joint);
        }
    }

    public override float JointDesire(SimplePart other) {
        return 2;
    }
}
