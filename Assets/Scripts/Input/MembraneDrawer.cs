using UnityEngine;

class MembraneDrawer {
    private Membrane current;

    public MembraneDrawer(Membrane startPoint) {
        current = startPoint;
    }

    public void DoDraw() {
        if (current == null) {
            return;
        }

        if (current.nextJoint != null) {
            if (Mouse.RightMouseDown()) {
                Membrane nextNext = (Membrane)current.nextJoint.GetConnected();
                current.Disconnect();
                Membrane next = CreateMembrane((nextNext.transform.position + current.transform.position)/2);
                current.ConnectTo(next);
                next.ConnectTo(nextNext);
            }
        } else {
            var difference = Mouse.WorldPosition() - (Vector2)current.transform.position;
            float springDist = current.GetNearbyRadius() + .25f;
            Debug.Log(difference);
            // if mouse is more than immediateSpringDist from current, make a new node
            if (difference.magnitude > springDist) {
                Membrane next = CreateMembrane((Vector2)current.transform.position + difference.normalized*springDist);

                current.ConnectTo(next);
                current = next;
            }
        }
    }

    private Membrane CreateMembrane(Vector2 position) {
        Membrane next = MonoBehaviour.Instantiate(Refs.inst.membrane, current.transform.parent).GetComponent<Membrane>();

        next.transform.position = position;

        return next;
    }
}
