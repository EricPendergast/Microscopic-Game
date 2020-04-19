using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Membrane : SimplePart {
    private List<Membrane> immediateConnections = new List<Membrane>();
    

    public override void UpdateSprings() {
        base.UpdateSprings();
        var siblingsByDistance = GetSiblings();
        siblingsByDistance.Sort(delegate(SimplePart sp1, SimplePart sp2) {
            return Distance(sp1) > Distance(sp2) ? 1 : -1;
        });

        immediateConnections.Clear();

        foreach (SimplePart sibling in siblingsByDistance) {
            if (sibling is Membrane m) {
                SpringJoint2D conn = GetCellGroup().MakeJoint(this, m);
                if (immediateConnections.Count < 2) {
                    // Prevents triangles
                    if (immediateConnections.Count == 1 &&
                        m.immediateConnections.Contains(immediateConnections[0])) {
                        continue;
                    }
                    conn.distance = MembraneBalance.i.immediateSpringDist;
                    conn.frequency = MembraneBalance.i.immediateSpringFreq;
                    immediateConnections.Add(m);
                } else if (Distance(sibling) < MembraneBalance.i.awayMaxDist) {
                    conn.distance = MembraneBalance.i.awaySpringDist;
                    conn.frequency = MembraneBalance.i.awaySpringFreq;
                }
            } else {
                //GetCellGroup().DestroyJoint(this, sibling);
            }
        }
        //foreach (SimplePart sibling in siblingsByDistance) {
        //    if (Distance(sibling) < CellPartBalance.i.springMaxDist) {
        //        SpringJoint2D conn = GetCellGroup().MakeJoint(this, sibling);
        //        //conn.distance += 1f*(1 - Random.value*2);
        //        //conn.distance = Mathf.Clamp(conn.distance, .5f, 3);
        //        conn.distance = CellPartBalance.i.springDist;
        //        conn.frequency = CellPartBalance.i.springFreq;
        //    } else {
        //        GetCellGroup().DestroyJoint(this, sibling);
        //    }
        //}
    }

    public override int JointDesire(SimplePart other) {
        return 1;
    }
}
