using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Actor))]
public abstract class ActorController : MonoBehaviour {

    /* The actor that this controller is controlling */
    protected Actor owner;

    protected virtual void Start() {
        owner = GetComponent<Actor>();
        if(owner == null) {
            Debug.LogError(name+" : ActorController is not attached to an Actor; Thats dumb!");
        }
    }

    protected virtual void Update() {

    }
}
