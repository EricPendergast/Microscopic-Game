using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nucleus : SimplePart {
    public float accel = 1;
    public List<SpringJoint2D> stickyJoints;
    public bool isSticky = true;

    public override void Awake() {
        base.Awake();
        if (stickyJoints == null) {
            stickyJoints = new List<SpringJoint2D>();
        }
    }

    public override void OnCellPartEnterNearby(SimplePart cp) {
        JointWrapper.MakeJoint(this, cp);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (isSticky) {
            foreach (var joint in stickyJoints) {
                if (joint.connectedBody == collision.rigidbody) {
                    return;
                }
            }
            SpringJoint2D sticky = gameObject.AddComponent<SpringJoint2D>();
            sticky.connectedBody = collision.rigidbody;
            sticky.distance = .5f;
            sticky.frequency = 2;
            sticky.enableCollision = true;
            stickyJoints.Add(sticky);
        }
    }
    
    //public override void ConfigureJointConstants(JointWrapper wrap) {
    //    var joint = wrap.GetOrMakeJoint<DistanceJoint2D>();
    //    
    //    joint.enableCollision = true;
    //    joint.autoConfigureConnectedAnchor = false;
    //    joint.autoConfigureDistance = false;
    //    joint.maxDistanceOnly = true;
    //    joint.distance = Distance(wrap.GetConnected());
    //    joint.breakForce = 20;
    //}
    public override void ConfigureJointConstants(JointWrapper wrap) {
        var joint = wrap.GetOrMakeJoint<DistanceJoint2D>();
        
        joint.enableCollision = true;
        joint.autoConfigureConnectedAnchor = false;
        joint.autoConfigureDistance = false;
        joint.maxDistanceOnly = true;
        joint.distance = Distance(wrap.GetConnected());
        joint.breakForce = 20;
    }

    public override void Update() {
        if (!isSticky && stickyJoints.Count > 0) {
            foreach (var joint in stickyJoints) {
                Destroy(joint);
            }
            stickyJoints.Clear();
        }
        base.Update();

        //foreach (SimplePart part in gameObject.GetComponentInChildren<NearbyDetector>().nearby) {
        //    Vector2 dir = (part.transform.position - transform.position);
        //    if (dir.magnitude < 1) {
        //        JointWrapper.MakeJoint(this, part);
        //    } else {
        //        dir.Normalize();
        //        part.body.AddForce(-dir*force);
        //        body.AddForce(dir*force);
        //    }
        //}
        ////TODO :apply a force to every nearby cell
    }

    public override float GetNearbyRadius() {
        return GetRadius() + 1;
    }

    public override float JointDesire(SimplePart sp) { 
        return float.PositiveInfinity;
    }
}
