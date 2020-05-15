using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cytoskeleton : SimplePart {
    public override void OnMouseOver() {
        base.OnMouseOver();
        if (Mouse.RightMouseDown()) {
            Debug.Log("Right button down");
            StartCoroutine(DragCoroutine());
        }
    }

    IEnumerator DragCoroutine() {
        while (Mouse.RightMouse()) {
            yield return null;
        }

        Debug.Log("Mouse released");

        if (Mouse.currentlyOver != null) {
            JointWrapper.MakeJoint(this, Mouse.currentlyOver);
        }
    }

    public override void OnCellPartEnterNearby(SimplePart part) {}
    public override float GetNearbyRadius() {
        return 0;
    }

    public override void ConfigureJointConstants(JointWrapper wrap) {
        var joint = wrap.GetOrMakeJoint<FixedJoint2D>();

        //joint.autoConfigureOffset = false;
        //joint.maxForce = 100;
        joint.frequency = 2;
        joint.autoConfigureConnectedAnchor = false;
        joint.breakTorque = 100;
        joint.breakForce = 100;
        joint.enableCollision = true;
    }

    public override float JointDesire(SimplePart sp) {
        return 1000;
    }

}
