using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nucleus : SimplePart {
    public float force = 1;

    public override void OnCellPartEnterNearby(SimplePart cp) {
        JointWrapper.MakeJoint(this, cp);
    }
    
    //public override void ConfigureJointConstants(JointWrapper joint) {
    //    base.ConfigureJointConstants(joint);
    //    joint.joint.distance = 0;
    //}

    new void Update() {
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
        return 1;
    }
}
