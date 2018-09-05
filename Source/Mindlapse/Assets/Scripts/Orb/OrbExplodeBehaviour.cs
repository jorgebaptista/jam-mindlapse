using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbExplodeBehaviour : StateMachineBehaviour
{
    private OrbScript orbScript;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (orbScript == null)
        {
            orbScript = animator.gameObject.GetComponent<OrbScript>();
        }

        orbScript.ToggleExplosionTrigger(true);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        orbScript.ToggleExplosionTrigger(false);

        orbScript.Dismiss();
    }
}
