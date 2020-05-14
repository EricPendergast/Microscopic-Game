using UnityEngine;
using UnityEngine.Assertions;

// TODO: What should happen if someone calls Destroy() on a joint? Right now it
// does not call the OnJointBreak stuff.
public class JointWrapper : MonoBehaviour {
    [SerializeField]
    private Joint2D joint;
    [SerializeField]
    private SimplePart source;
    [SerializeField]
    private SimplePart connected;
    [SerializeField]
    private bool notifiedBreak = false;
    private bool isDestroyed = false;
    
    private void Init(SimplePart source, SimplePart connected) {
        this.source = source;
        this.connected = connected;
        Reconfigure();
    }

    public SimplePart GetConnected() {
        return connected;
    }

    public SimplePart GetSource() {
        return source;
    }

    public bool HasJoint() {
        return HasJoint<Joint2D>();
    }

    public bool HasJoint<T>() where T : Joint2D {
        return joint != null && joint is T;
    }

    public T GetOrMakeJoint<T>() where T : Joint2D {
        if (joint == null) {
            joint = gameObject.AddComponent<T>();
            joint.connectedBody = connected.body;
            connected.OnConnectedTo(this);
        }
        return (T)joint;
    }

    public void Reconfigure() {
        GetSource().ConfigureJointConstants(this);
        Assert.IsNotNull(joint);
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
        NotifyJointBreak();
        isDestroyed = true;
        Destroy(this);
    }
    
    public void Destroy() {
        NotifyJointBreak();
        isDestroyed = true;
        Destroy(this);
    }

    void OnApplicationQuit() {
        isDestroyed = true;
    }

    void OnDestroy() {
        Assert.IsTrue(isDestroyed, "Someone destroyed JointWrapper directly, without calling Destroy()");
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
        joint.Init(sp1, sp2);
        
        return joint;
    }

    // Finds the joint from sp1 to sp2
    public static JointWrapper GetJoint(SimplePart sp1, SimplePart sp2) {
        foreach (var joint in sp1.GetComponents<JointWrapper>()) {
            if (!joint.isDestroyed && joint.GetConnected() == sp2) {
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
