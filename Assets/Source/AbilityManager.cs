using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Component that acts as the manager for abilities on an actor.
 * It includes the 'inventory' functionality of slots and stuff.
 * 
 * An Ai using abilities will likely have it's own unique implementation
 */
[RequireComponent(typeof(Actor))]
public class AbilityManager : MonoBehaviour {

    public const int NUM_ABILITY_SLOTS = 5;
    public enum abilitySlot {
        Slot_Weapon, 
        Slot_Head, 
        Slot_Torso, 
        Slot_Feet, 
        Slot_Util 
    }

    private Actor owner;

    /* base set of available abilities */
    private Ability[] abilitySet;

    /* Currently updating abilties */
    private List<Ability> activeList;

    private void Start() {
        owner = GetComponent<Actor>();

        abilitySet = new Ability[NUM_ABILITY_SLOTS];
        activeList = new List<Ability>();
	}


	private void Update() {

        for(int i = 0; i < activeList.Count; i++) {
            Ability.AbilityStatus status = activeList[i].Update(owner);
            if(status == Ability.AbilityStatus.Finished) {
                activeList[i].End(owner);
                activeList[i].bActive = false;
            }
        }

        // remove inactive abilities from update list
        activeList.RemoveAll(ability => !ability.bActive);
	}


    
    public bool ActivateAbility(abilitySlot targetSlot) {
        // first ensure that there is no ability blocking new activations
        bool bWasBlocked = false;
        foreach(Ability a in activeList) {
            if(a.bRestrictsActivation) {
                bWasBlocked = true;
                break;
            }
        }

        if(bWasBlocked) {
            return false;
        }

        // If it wasn't blocked, go ahead and try to activate it and set it to update
        Ability activated = abilitySet[(int)targetSlot];
        if(activated == null) {
            // There is no ability in that slot
            return false;
        }

        // Make sure it isnt already active or in cooldown
        if(activated.bActive || Time.time - activated.lastActivated < activated.cooldown) {
            return false;
        }

        activated.bActive = true;
        activated.lastActivated = Time.time;
        activated.Activate(owner);

        activeList.Add(activated);

        Debug.Log("Ability: " + activated.name + " was activated on " + owner.name);
        return true;
    }


    public void SetAbilityslot<AbilityType>(abilitySlot targetSlot) where AbilityType : Ability, new() {
        // TODO: if an ability was replaced, drop a physical pickup for that ability
        // For now it will just get replaced...

        if(abilitySet[(int)targetSlot] != null) {
            // drop the current ability
        }

        RemoveAbility(targetSlot);
        abilitySet[(int)targetSlot] = new AbilityType();

        // Auto activate passive abilities
        if(abilitySet[(int)targetSlot].bPassiveEffect) {
            ActivateAbility(targetSlot);
        }
    }

    public void RemoveAbility(abilitySlot targetSlot) {
        if(abilitySet[(int)targetSlot] != null) {
            activeList.Remove(abilitySet[(int)targetSlot]);
            abilitySet[(int)targetSlot].End(owner);
            abilitySet[(int)targetSlot].bActive = false;

            Debug.Log("Ability: " + abilitySet[(int)targetSlot].name + " was removed from " + owner.name);
        }

        abilitySet[(int)targetSlot] = null;
    }

}