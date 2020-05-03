using UnityEngine;

class MembraneDrawer {
    private Membrane current;

    public MembraneDrawer(Membrane startPoint) {
        current = startPoint;
    }

    public void DoDraw() {
        if (current == null || current.nextJoint != null) {
            return;
        }
        var difference = Mouse.WorldPosition() - (Vector2)current.transform.position;
        float springDist = current.GetNearbyRadius() + .5f;
        Debug.Log(difference);
        // if mouse is more than immediateSpringDist from current, make a new node
        if (difference.magnitude > springDist) {
            Membrane next = MonoBehaviour.Instantiate(Refs.inst.membrane, current.transform.parent).GetComponent<Membrane>();

            next.transform.position = (Vector2)current.transform.position + difference.normalized*springDist;
            
            current.ConnectTo(next);
            current = next;
        }
    }
}
