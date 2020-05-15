using System.Collections.Generic;
using UnityEngine;

public class Nucleus : SimplePart {
    public float accel = 1;
    public List<JointWrapper> stickyJoints;
    public bool isSticky = true;

    public override void Awake() {
        base.Awake();
        if (stickyJoints == null) {
            stickyJoints = new List<JointWrapper>();
        }
    }

    public override void OnCellPartEnterNearby(SimplePart cp) {
        JointWrapper.MakeJoint(this, cp);
    }
    public override void ConfigureJointConstants(JointWrapper wrap) {
        SpringJoint2D sticky = wrap.GetOrMakeJoint<SpringJoint2D>();
        sticky.distance = .5f;
        sticky.frequency = 2;
        sticky.enableCollision = true;
        stickyJoints.Add(wrap);
    }

    public override void Update() {
        if (!isSticky && stickyJoints.Count > 0) {
            foreach (var joint in stickyJoints) {
                joint.Destroy();
            }
            stickyJoints.Clear();
        }
        base.Update();
    }

    public override float GetNearbyRadius() {
        return GetRadius() + .1f;
    }

    public override float JointDesire(SimplePart sp) { 
        return float.PositiveInfinity;
    }
}
