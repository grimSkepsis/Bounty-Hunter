using UnityEngine;
using System.Collections;
using BehaviourMachine;

public class Action_WalkTo : AI_ActionNode {


    public override Status Update() {
        const float minDistance = 0.1f;

        Vector3 toTarget = (self.transform.position - ai.targetLocation);

        toTarget.y = 0.0f;
        if(toTarget.magnitude < minDistance) {
            return Status.Success;
        }
        else {
            movementController.Move(toTarget.x > 0.0f? -1.0f : 1.0f);
        }

        return Status.Running;
    }

}