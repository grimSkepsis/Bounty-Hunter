using UnityEngine;
using System.Collections;

public class ARocketBoots : Ability {

    public ARocketBoots() {
        name = "Rocket Boots";
        bPassiveEffect = true;
    }

    public override void Activate(Actor owner) {
        owner.ApplyStatEffect(Actor.Stat.jumpHeight, 2.0f, name);
    }


    public override AbilityStatus Update(Actor owner) {
        return AbilityStatus.Running;
    }


    public override void End(Actor owner) {
        owner.RemoveStatEffect(name);
    }
}