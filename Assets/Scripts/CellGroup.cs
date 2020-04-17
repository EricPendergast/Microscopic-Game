//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CellGroup : MonoBehaviour {
    public bool addFlagella = false;
    public bool addLump = false;

    public bool isPlayer = false;

    public void Start() {
    }

    public void Update() {
        if (addLump) {
            addLump = false;
            GameObject.Instantiate(Refs.inst.lump, transform);
        }
        if (addFlagella) {
            addFlagella = false;
            GameObject.Instantiate(Refs.inst.flagella, transform);
        }
    }

    public List<SimplePart> GetCellParts() {
        List<SimplePart> all = new List<SimplePart>();
        
        foreach (Transform child in transform) {
            var part = child.GetComponent<SimplePart>();
            if (part != null) {
                all.Add(part);
            }
        }

        return all;
    }

    public SpringJoint2D MakeJoint(SimplePart sp1, SimplePart sp2) {
        Assert.AreNotEqual(sp1, sp2, "Joints must be between two different cell parts");
        var joint = GetJoint(sp1, sp2);
        if (joint != null) {
            return joint;
        }

        joint = sp1.gameObject.AddComponent<SpringJoint2D>();
        joint.connectedBody = sp2.gameObject.GetComponent<Rigidbody2D>();
        
        return joint;
    }

    // Returns true if a joint was destroyed, false if already no joint.
    public void DestroyJoint(SimplePart sp1, SimplePart sp2) {
        var joint = GetJoint(sp1, sp2);
        if (joint == null) {
            //return false;
        } else {
            Destroy(joint);
            //return true;
        }
    }

    // Finds the joint shared with sp1 and sp2
    public SpringJoint2D GetJoint(SimplePart sp1, SimplePart sp2) {
        SpringJoint2D joint12 = GetJointHelper(sp1, sp2);
        SpringJoint2D joint21 = GetJointHelper(sp2, sp1);
        Assert.IsTrue(joint12 == null || joint21 == null, "Spring should only go one direction");
        if (joint12 != null) {
            return joint12;
        } else {
            return joint21;
        }
    }

    // Finds the joint going from sp1 to sp2
    private SpringJoint2D GetJointHelper(SimplePart sp1, SimplePart sp2) {
        foreach (SpringJoint2D joint in sp1.GetComponents<SpringJoint2D>()) {
            if (joint.connectedBody.gameObject.GetComponent<SimplePart>() == sp2) {
                return joint;
            }
        }
        return null;
    }
}
