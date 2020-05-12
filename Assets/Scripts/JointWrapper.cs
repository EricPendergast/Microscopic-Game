using UnityEngine;
using UnityEngine.Assertions;

// TODO: What should happen if someone calls Destroy() on a joint? Right now it
// does not call the OnJointBreak stuff.
public class JointWrapper : AwakeOnce {
    public SpringJoint2D joint;
    [SerializeField]
    private SimplePart attachedCellPart;
    private bool notifiedBreak = false;
    
    public override void DoAwake() {
        joint = gameObject.AddComponent<SpringJoint2D>();
        attachedCellPart = GetComponent<SimplePart>();
    }

    void Start() {
        Reconfigure();
    }

    private void SetConnected(SimplePart other) {
        Assert.IsNull(joint.connectedBody);
        joint.connectedBody = other.GetComponent<Rigidbody2D>();
        Reconfigure();
        other.OnConnectedTo(this);
    }

    public SimplePart GetConnected() {
        if (joint.connectedBody == null) {
            return null;
        } else {
            return joint.connectedBody.GetComponent<SimplePart>();
        }
    }

    public SimplePart GetSource() {
        return attachedCellPart;
    }

    public void Reconfigure() {
        if (joint != null) {
            GetSource().ConfigureJointConstants(this);
        }
    }

    //void Update() {
    //  update rendering
    //}
    
    private void NotifyJointBreak() {
        if (!notifiedBreak) {
            GetSource().OnOwnedJointBroke(this);
            GetConnected().OnUnownedJointBroke(this);
            notifiedBreak = true;
        }
    }

    void OnJointBreak2D(Joint2D broken) {
        if (broken != joint) {
            return;
        }

        NotifyJointBreak();

        joint = null;
        Destroy(this);
    }
    
    void OnDestroy() {
        NotifyJointBreak();
        Destroy(joint);
        joint = null;
    }

    public static JointWrapper MakeJoint(SimplePart sp1, SimplePart sp2) {
        if (sp1 == sp2) {
            return null;
        }
        if (sp1 != WhoControlsJoint(sp1, sp2)) {
            return null;
        }

        var joint = GetJoint(sp1, sp2);
        if (joint != null) {
            return null;
        }

        joint = sp1.gameObject.AddComponent<JointWrapper>();
        joint.SetConnected(sp2);
        
        return joint;
    }

    // Finds the joint from sp1 to sp2
    public static JointWrapper GetJoint(SimplePart sp1, SimplePart sp2) {
        foreach (var joint in sp1.GetComponents<JointWrapper>()) {
            if (joint.GetConnected() == sp2) {
                return joint;
            }
        }
        return null;
    }
    
    public static SimplePart WhoControlsJoint(SimplePart sp1, SimplePart sp2) {
        if (sp1 is Membrane m1 && sp2 is Membrane m2) {
            // sp1 is the default controller if niether has an actual joint
            if (GetJoint(sp2, sp1) == null) {
                return sp1;
            } else {
                return sp2;
            }
        }

        if (sp1.JointDesire(sp2) > sp2.JointDesire(sp1)) {
            return sp1;
        } else if (sp1.JointDesire(sp2) == sp2.JointDesire(sp1)) {
            // If same desire, choose arbitrariliy but consistently.
            return sp1.GetInstanceID() < sp2.GetInstanceID() ? sp1 : sp2;
        } else {
            return sp2;
        }
    }
}
