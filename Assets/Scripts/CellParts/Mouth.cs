//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Mouth : SimplePart {
    public JointWrapper anchor;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out SimplePart part)) {
            part.transform.parent = transform.parent;
        }
    }

    public override void OnCellPartEnterNearby(SimplePart cp) {
        if (anchor == null && cp is Membrane) {
            anchor = JointWrapper.MakeJoint(this, cp);
        }
    }

    public override float JointDesire(SimplePart other) {
        return 3;
    }

    public override void ConfigureJointConstants(JointWrapper wrap) {
        base.ConfigureJointConstants(wrap);
        var joint = wrap.GetOrMakeJoint<SpringJoint2D>();
        joint.distance = 0;
        joint.enableCollision = false;
    }
}
