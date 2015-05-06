using UnityEngine;
using System.Collections;

public class AGrapplingHook : Ability {

    public const float grappleSpeed = 7.0f;
    public const float maxGrappleRange = 10.0f;

    public Vector3 grappleTarget;
    private bool bFailedToGrapple;

    public AGrapplingHook() {
        name = "Grappling Hook";
        bPassiveEffect = false;
        cooldown = 1.0f;
    }

    public override void Activate(Actor owner) {
        owner.GetMovementController().EnableGravity(false);
        grappleTarget = owner.GetController().GetAimLocation();

        // Register a callback so that when the owner collides with something it can cancle the grapple
        owner.GetMovementController().RegisterCollisionListener(HandleCollision);

        // Check that the grapple hook is on something
        Collider2D[] hits = Physics2D.OverlapPointAll(grappleTarget);
        bFailedToGrapple = (hits.Length == 0);

        if((owner.transform.position - grappleTarget).magnitude > maxGrappleRange) {
            bFailedToGrapple = true;
        }
    }


    public override AbilityStatus Update(Actor owner) {
        Vector3 toTarget = grappleTarget - owner.transform.position;

        // fly towards target until it collides with something or it is otherwise canceled (usually through collision)
        if(!bFailedToGrapple) {
            owner.GetMovementController().SetVelocity(toTarget.normalized * grappleSpeed);
            return AbilityStatus.Running;
        }
        else {
            owner.GetMovementController().SetVelocity(Vector3.zero);
            return AbilityStatus.Finished;
        }
    }


    public override void End(Actor owner) {
        owner.GetMovementController().EnableGravity(true);

        // dont forget to unregister the callback
        owner.GetMovementController().UnRegisterCollisionListener(HandleCollision);
    }

    public void HandleCollision(Vector3 point, Vector3 target) {
        // I guess we could also do a hold of some sort
        bFailedToGrapple = true;
    }
}