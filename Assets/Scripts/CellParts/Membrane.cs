using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Membrane : SimplePart {
    public Membrane prev = null;
    public Membrane next = null;
    public SpringJoint2D nextJoint = null;
    public int minLoopSize = 5;

    new void Start() {
        base.Start();
        if (next == null) {
            FindNext();
        }
    }

    void OnJointBreak(float force) {
        StartCoroutine(OnJointBreakCoroutine());
    }

    IEnumerator OnJointBreakCoroutine() {
        yield return null;

        if (nextJoint != null) {
            Debug.Log("Unexpected joint broke in Membrane");
            yield break;
        }
        // TODO: Do something like this
        next.prev = null;
        next = null;
        Debug.Log("OnJointBreakCoroutine set next to [" + next + "] " + this.GetInstanceID());

        FindNext();
    }

    void FindNext() {
        Debug.Log("Call to FindNext " + this.GetInstanceID());
        Assert.IsNull(nextJoint);
        Assert.IsNull(next);

        Membrane closest = null;
        foreach (SimplePart sibling in GetSiblings()) {
            if (sibling is Membrane m) {
                if (closest == null || Distance(m) < Distance(closest)) {
                    if (m.prev == null && !m.GetNexts(minLoopSize).Contains(this)) {
                        closest = m;
                    }
                }
            }
        }

        if (closest != null && Distance(closest) < MembraneBalance.i.immediateMaxDist) {
            ConnectTo(closest);
        }
    }

    private void ConnectTo(Membrane newNext) {
        next = newNext;
        Debug.Log("ConnectTo set next to [" + next + "] " + this.GetInstanceID());
        Assert.IsNull(next.prev);
        next.prev = this;

        nextJoint = gameObject.AddComponent<SpringJoint2D>();
        Debug.Log("ConnectTo set nextJoint to [" + nextJoint + "] " + this.GetInstanceID());
        nextJoint.connectedBody = next.GetComponent<Rigidbody2D>();
        nextJoint.distance = MembraneBalance.i.immediateSpringDist;
        nextJoint.frequency = MembraneBalance.i.immediateSpringFreq;
        nextJoint.autoConfigureDistance = false;
    }

    List<Membrane> GetNexts(int numNext) {
        if (nextJoint == null || numNext == 0) {
            return new List<Membrane>();
        } else {
            Membrane n = nextJoint.connectedBody.GetComponent<Membrane>();
            List<Membrane> l = n.GetNexts(numNext - 1);
            l.Add(n);
            return l;
        }
    }

    public override void UpdateSprings() {
        if (next == null) {
            FindNext();
        }
    }

    public override float JointDesire(SimplePart other) {
        return -1;
    }
}
