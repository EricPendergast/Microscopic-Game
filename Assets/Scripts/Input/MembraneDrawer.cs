using UnityEngine;

class MembraneDrawer {
    private Membrane current;

    public MembraneDrawer(Membrane startPoint) {
        current = startPoint;
    }

    public void DoDraw() {
        if (current == null || current.next != null) {
            return;
        }
        var difference = Mouse.WorldPosition() - (Vector2)current.transform.position;
        Debug.Log(difference);
        // if mouse is more than immediateSpringDist from current, make a new node
        if (difference.magnitude > MembraneBalance.i.immediateSpringDist) {
            Membrane next = MonoBehaviour.Instantiate(Refs.inst.membrane, current.GetCellGroup().transform).GetComponent<Membrane>();

            next.transform.position = (Vector2)current.transform.position + difference.normalized*MembraneBalance.i.immediateSpringDist;
            
            current.ConnectTo(next);
            current = next;
        }
    }
}
