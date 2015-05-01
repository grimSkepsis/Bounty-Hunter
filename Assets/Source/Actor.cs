using UnityEngine;
using System.Collections;

/**
 * Main base class:
 * Handles taking damage, receiving attacks and routing controller decisions to the movement component
 */ 
public class Actor : MonoBehaviour {

    [SerializeField]
    public string actorName;

    [SerializeField]
    public float health;

    private float maxHealth;

    /*
     * TODO: 
     * Armor, or would it be in an ability class? Like allow active abilities to modify damage before its applied.
     * Other shielding and whatnot.
     */ 

    /* The brains of the system: all inputs originate from the controller */
    private ActorController controller;

    /* Component handles the physical movement and collision */
    private MovementController movementComponent;


    // Get and Set: note that the controllers can return null 
    public ActorController GetController() { return controller; }
    public MovementController GetMovementController() { return movementComponent; }


    void Start() {
        // Grab the other components
        controller = GetComponent<ActorController>();
        movementComponent = GetComponent<MovementController>();

        // start with full health
        maxHealth = health;
    }


    void Update() {

    }



    public void TakeDamage(float damageAmount) {
        // TODO: allow all sorts of stuff to modify the health, also possibly add params for hit location etc
        
        health -= damageAmount;
        if(health <= 0.0f) {
            health = 0.0f;
            // Died!!!
        }
    }
}
