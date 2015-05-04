using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    // Buff debuff system: NOTE: this is totally an experiment, not sure if it's worth doing it this way
    public struct StatEffect {
        public string tag;      // tag identifies where this effect came from so it can be removed specifically
        public float modifier;  // how much to modify the stat by, may be addition or multiply depending on stat
        public Stat targetStat; // which stat to effect
    }

    /* List of all stat modifiers on the actor. this list will be polled for any relevant effects when needed */
    private List<StatEffect> activeEffects;

    public enum Stat {
        health, 
        speed, 
        armor, 
        stealth, 
        shield, 
        jumpHeight,
        airJump
    }

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

        activeEffects = new List<StatEffect>();
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


    public void ApplyStatEffect(Stat targetStat, float modifier, string sourceTag) {
        StatEffect effect = new StatEffect();
        effect.tag = sourceTag;
        effect.modifier = modifier;
        effect.targetStat = targetStat;

        activeEffects.Add(effect);
    }

    /* remove all effects whose source is the input tag */
    public void RemoveStatEffect(string sourceTag) {
        activeEffects.RemoveAll(effect => effect.tag.Equals(sourceTag));
    }

    public float GetTotalModifierForStat(Stat stat) {
        float modifier = 0.0f;

        foreach(StatEffect effect in activeEffects) {
            if(effect.targetStat == stat) {
                modifier += effect.modifier;
            }
        }

        return modifier;
    }

    /* treats modifier as a bonus multiplier.
     * Returns the total modifier for a stat, unless there is no modifiers, in which case it returns 1.0f 
     */
    public float GetMultiplierForStat(Stat stat) {
        float total = GetTotalModifierForStat(stat);
        return (total == 0.0f)? 1.0f : total;
    }
}
