using UnityEngine;
using System.Collections;

public abstract class Ability {

    public enum AbilityStatus {
        Running, Finished
    }



    /* If this falg is set, subsiquent abilties cannot be activated when this one is active */
    public bool bRestrictsActivation = false;

    /* period in which this ability cannot be actived again. if 0, then there is no cooldown */
    public float cooldown;

    /* If this ability is passive, then it is activated on add, and only ends when removed from set */
    public bool bPassiveEffect;

    /* The game name of the ability */
    public string name;

    /* Is the ability currently active */
    public bool bActive = false;

    /* Time stamp of last activation */
    public float lastActivated;



    /* Called when this ability is used, causes it to start updating */
    public abstract void Activate(Actor owner);

    /* Called every frame until it returns Finished */
    public abstract AbilityStatus Update(Actor owner);

    /* Called whent he ability stops updating */
    public abstract void End(Actor owner);
}