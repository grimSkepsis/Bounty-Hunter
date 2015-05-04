using UnityEngine;
using System.Collections;

public class PlayerController : ActorController {

    private AbilityManager abilityManager;

    protected override void Start() {
        base.Start();

        // Note that it may not have one
        abilityManager = GetComponent<AbilityManager>();
    }

    protected override void Update() {
        base.Update();

        // basic movement stuff
        float horzInput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space)) {
            owner.GetMovementController().Jump();
        }

        owner.GetMovementController().Move(horzInput);

        // TEMP ability testing stuff: just 1 to add ability, 2 to remove it
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            if(abilityManager != null) {
                abilityManager.SetAbilityslot<ASpeedBoots>(AbilityManager.abilitySlot.Slot_Feet);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            if(abilityManager != null) {
                abilityManager.SetAbilityslot<ARocketBoots>(AbilityManager.abilitySlot.Slot_Feet);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            if(abilityManager != null) {
                abilityManager.SetAbilityslot<ADoubleJump>(AbilityManager.abilitySlot.Slot_Util);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            if(abilityManager != null) {
                abilityManager.RemoveAbility(AbilityManager.abilitySlot.Slot_Feet);
                abilityManager.RemoveAbility(AbilityManager.abilitySlot.Slot_Util);
            }
        }
    }
}
