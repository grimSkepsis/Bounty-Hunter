using UnityEngine;
using System.Collections;

public class PlayerController : ActorController {

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();

        // temporary test movement stuff
        float horzInput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space)) {
            owner.GetMovementController().Jump();
        }

        owner.GetMovementController().Move(horzInput);
    }
}
