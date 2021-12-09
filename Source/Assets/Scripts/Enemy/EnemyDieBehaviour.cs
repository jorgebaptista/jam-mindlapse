using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieBehaviour : StateMachineBehaviour
{
    private EnemyScript enemyScript;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyScript == null)
        {
            enemyScript = animator.GetComponent<EnemyScript>();
        }

        enemyScript.Dismiss();
    }
}
