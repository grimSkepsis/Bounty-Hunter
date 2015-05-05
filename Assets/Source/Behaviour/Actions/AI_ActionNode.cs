using UnityEngine;
using System.Collections;
using BehaviourMachine;

public abstract class AI_ActionNode : ActionNode {

    protected AIController ai;
    protected MovementController movementController;

    public override void Awake() {
        ai = self.GetComponent<AIController>();
        movementController = self.GetComponent<MovementController>();
    }


}