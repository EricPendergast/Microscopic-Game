//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Interactions between cell parts should be facillitated by their cell group
public class CellGroup : MonoBehaviour {
    public bool addFlagella = false;
    public bool addLump = false;
    public bool addMembrane = false;

    public bool isPlayer = false;

    public void Start() {
    }

    public void Update() {
        var com = CalculateCenterOfMass();
        if (addLump) {
            addLump = false;
            GameObject.Instantiate(Refs.inst.lump, transform).transform.position = com;
        }
        if (addFlagella) {
            addFlagella = false;
            GameObject.Instantiate(Refs.inst.flagella, transform).transform.position = com;
        }
        if (addMembrane) {
            addMembrane = false;
            GameObject.Instantiate(Refs.inst.membrane, transform).transform.position = com;
        }
    }

    public Vector3 CalculateCenterOfMass() {
        //TODO: Optimization, cache this value at the beginning of each tick
        Vector3 com = new Vector3();
        int count = 0;

        foreach (SimplePart sp in GetCellParts()) {
            com += sp.transform.position;
            count++;
        }

        return com/count;
    }

    // TODO: See if I can make this generic, to get all nearby cells of a
    // certain type.
    public List<SimplePart> GetNearby(SimplePart part, float radius) {
        List<SimplePart> nearby = new List<SimplePart>();

        foreach (Collider2D rb in Physics2D.OverlapCircleAll(part.transform.position, radius)) {
            if (rb.gameObject.TryGetComponent<SimplePart>(out SimplePart sp)) {
                // TODO: There are multiple valid ways to do this (should there be an "== this"?)
                if (sp.GetCellGroup() == part.GetCellGroup()) {
                    nearby.Add(sp);
                }
            }
        }

        return nearby;
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

    public JointWrapper MakeJoint(SimplePart sp1, SimplePart sp2) {
        if (sp1 == sp2) {
            return null;
        }
        var joint = GetJoint(sp1, sp2);
        if (joint != null) {
            return joint;
        } else if (sp1 != WhoControlsJoint(sp1, sp2)) {
            return null;
        }

        joint = sp1.gameObject.AddComponent<JointWrapper>();
        joint.SetConnected(sp2);
        
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

    // Finds the joint from sp1 to sp2
    public JointWrapper GetJoint(SimplePart sp1, SimplePart sp2) {
        foreach (var joint in sp1.GetComponents<JointWrapper>()) {
            if (joint.GetConnected() == sp2) {
                return joint;
            }
        }
        return null;
    }

    public SimplePart WhoControlsJoint(SimplePart sp1, SimplePart sp2) {
        if (sp1.JointDesire(sp2) >= sp2.JointDesire(sp1)) {
            return sp1;
        } else {
            return sp2;
        }
    }
}
