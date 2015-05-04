using UnityEngine;
using System.Collections;

public class ADoubleJump : Ability {

    public ADoubleJump() {
        name = "Jet Pack";
        bPassiveEffect = true;
    }

    public override void Activate(Actor owner) {
        owner.ApplyStatEffect(Actor.Stat.airJump, 1.0f, name);
    }


    public override AbilityStatus Update(Actor owner) {
        return AbilityStatus.Running;
    }


    public override void End(Actor owner) {
        owner.RemoveStatEffect(name);
    }
}