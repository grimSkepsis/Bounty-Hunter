using UnityEngine;
using System.Collections;

public class ASpeedBoots : Ability {

    public ASpeedBoots() {
        name = "Speed Boots";
        bPassiveEffect = true;
    }

    public override void Activate(Actor owner) {
        owner.ApplyStatEffect(Actor.Stat.speed, 2.0f, name);
    }

    
    public override AbilityStatus Update(Actor owner) {
        return AbilityStatus.Running;
    }


    public override void End(Actor owner) {
        owner.RemoveStatEffect(name);
    }
}