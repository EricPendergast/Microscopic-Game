using UnityEngine;

public abstract class AwakeOnce : MonoBehaviour {
    [SerializeField]
    private bool initialized = false;

    void Awake() {
        if (!initialized) {
            DoAwake();
        }
        initialized = true;
    }

    public abstract void DoAwake();
}
