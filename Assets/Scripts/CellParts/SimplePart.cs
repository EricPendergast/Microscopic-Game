using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePart : MonoBehaviour {
    public Rigidbody2D body;
    public int updateSpringsCoroutineCount = 0;
    public NearbyDetector nearby;

    public virtual void Awake() {
        if (nearby == null) {
            nearby = NearbyDetector.Create(this);
        }
        if (body == null) {
            body = GetComponent<Rigidbody2D>();
        }
    }

    public virtual void Start() {
        {
            var p = transform.position;
            p.z = -Random.value;
            transform.position = p;
        }
        OnTransformParentChanged();
        StartCoroutine(UpdateSpringsCoroutine());
    }
    
    public virtual void OnMouseUp() {
        if (Mouse.LeftMouseUp()) {
            Destroy(GetComponent<TargetJoint2D>());
        }
    }

    public virtual void OnMouseOver() {
        Mouse.OnMouseOverCellPart(this);
    }

    public virtual void OnMouseDrag() {
        if (Mouse.LeftMouse()) {
            //Mouse.OnMouseDragCellPart(this);
            var mouseSpring = GetComponent<TargetJoint2D>();
            if (mouseSpring == null) {
                mouseSpring = gameObject.AddComponent<TargetJoint2D>();
            }
            mouseSpring.target = Mouse.WorldPosition();
        }
    }

    public void OnTransformParentChanged() {
        //UpdateSprings();
    }

    IEnumerator UpdateSpringsCoroutine(){
        while (true) {
            //UpdateSprings();
            updateSpringsCoroutineCount++;
            yield return new WaitForSeconds(.5f + Random.value * CellPartBalance.i.springUpdateTime);
        }
    }

    public virtual void Update() {
        body.velocity = body.velocity*(CellPartBalance.i.friction);
    }

    public List<SimplePart> GetNearby(float distance) {
        CellGroup cg = transform.parent.GetComponent<CellGroup>();
        if (cg == null) {
            return new List<SimplePart>();
        } else {
            return cg.GetNearby(this, distance);
        }
    }

    public float Distance(SimplePart sp1) {
        return (transform.position - sp1.transform.position).magnitude;
    }


    public virtual void OnConnectedTo(JointWrapper joint) {
        // TODO: Keep track of all connected cells
    }

    public virtual void ConfigureJointConstants(JointWrapper wrap) {
        var joint = wrap.GetOrMakeJoint<SpringJoint2D>();

        joint.distance = wrap.GetSource().GetNearbyRadius() + wrap.GetConnected().GetRadius() - .2f;
        joint.frequency = CellPartBalance.i.springFreq;
        joint.breakForce = CellPartBalance.i.springBreakForce;
        joint.autoConfigureDistance = false;
        joint.enableCollision = true;
    }

    // Called on the joint owner when a joint breaks
    public virtual void OnOwnedJointBroke(JointWrapper joint) {}

    // Called on the other end of the joint when a joint breaks
    public virtual void OnUnownedJointBroke(JointWrapper joint) {}


    public virtual void UpdateSprings() {
        foreach (var joint in GetComponents<JointWrapper>()) {
            joint.Reconfigure();
        }
    }

    // Returns an integer representing how much this cell part wants to control
    // the joint from it to the other cell part
    public virtual float JointDesire(SimplePart other) {
        return -1;
    }

    public virtual float GetRadius() {
        CircleCollider2D c = GetComponent<CircleCollider2D>();
        return c.radius*c.transform.lossyScale.x;
    }

    public virtual float GetNearbyRadius() {
        return GetRadius() + .1f;
    }

    public virtual void OnCellPartEnterNearby(SimplePart cp) {
        if (Distance(cp) < CellPartBalance.i.springMaxDist && cp.transform.parent == transform.parent) {
            JointWrapper.MakeJoint(this, cp);
        }
    }

    public virtual void OnCellPartExitNearby(SimplePart cp) {}

    protected virtual void OnDestroy() {
        foreach (JointWrapper joint in gameObject.GetComponents<JointWrapper>()) {
            joint.Destroy();
        }
    }
}
