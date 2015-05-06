using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Actor))]
public abstract class ActorController : MonoBehaviour {

    /* The actor that this controller is controlling */
    protected Actor owner;

    /* All actors can set a aiming location, that is where they are targetting */
    protected Vector3 aimLocation;

    public Vector3 GetAimLocation() { return aimLocation; }

    protected virtual void Start() {
        owner = GetComponent<Actor>();
        if(owner == null) {
            Debug.LogError(name+" : ActorController is not attached to an Actor; Thats dumb!");
        }
    }

    protected virtual void Update() {

    }
}
