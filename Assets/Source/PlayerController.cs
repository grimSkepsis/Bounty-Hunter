using UnityEngine;
using System.Collections;

public class PlayerController : ActorController {

    private AbilityManager abilityManager;

    private Camera mainCamera;
    private PlayerCamera playerCamera;

    protected override void Start() {
        base.Start();

        // Note that it may not have one
        abilityManager = GetComponent<AbilityManager>();

        playerCamera = FindObjectOfType<PlayerCamera>();
        if(playerCamera == null) {
            Debug.LogError(name + " failed to find a PlayerCamera Component in the scene!");
        }
        else {
            mainCamera = playerCamera.GetComponent<Camera>();
        }
    }

    protected override void Update() {
        base.Update();

        // Update the aim location to be the mouse position
        if(mainCamera != null) {
            aimLocation = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            aimLocation.z = 0.0f;
        }

        // basic movement stuff
        float horzInput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space)) {
            owner.GetMovementController().Jump();
        }

        owner.GetMovementController().Move(horzInput);

        // TEMP ability testing stuff:
        if(Input.GetButtonDown("Fire1")) {
            abilityManager.ActivateAbility(AbilityManager.abilitySlot.Slot_Weapon);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            if(abilityManager != null) {
                abilityManager.SetAbilityslot<AGrapplingHook>(AbilityManager.abilitySlot.Slot_Weapon);
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
