using UnityEngine;
using System.Collections;
using BehaviourMachine;

public class Action_GetPatrolLocation : AI_ActionNode {

    public override Status Update() {
        const float patrolRange = 3.0f;

        Vector3 offset = (Random.Range(0.0f, 1.0f) > 0.5f)? Vector3.right : -Vector3.right;
        ai.targetLocation = self.transform.position + offset * patrolRange;

        return Status.Success;
    }
}