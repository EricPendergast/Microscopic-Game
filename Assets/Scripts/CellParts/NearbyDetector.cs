using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NearbyDetector : MonoBehaviour {
    public SimplePart owner;
    public List<SimplePart> nearby;

    public void Awake() {
        nearby = new List<SimplePart>();
    }

    public void Start() {
        Assert.IsNotNull(owner, "Owner should never be null; was this component initialized using NearbyDetector.Make()?");
        gameObject.GetComponent<CircleCollider2D>().radius = owner.GetNearbyRadius();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.attachedRigidbody.TryGetComponent<SimplePart>(out SimplePart sp)) {
            owner.OnCellPartEnterNearby(sp);
            nearby.Add(sp);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.attachedRigidbody.TryGetComponent<SimplePart>(out SimplePart sp)) {
            owner.OnCellPartExitNearby(sp);
            nearby.Remove(sp);
        }
    }

    public static void Create(SimplePart part) {
        GameObject go = new GameObject();
        go.transform.SetParent(part.transform);
        go.transform.position = part.transform.position;
        var circ = go.AddComponent<CircleCollider2D>();
        circ.isTrigger = true;
        go.AddComponent<NearbyDetector>().owner = part;
    }
}
