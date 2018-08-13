using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : StateMachineBehaviour
{
    private EnemyScript enemyScript;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyScript == null)
        {
            enemyScript = animator.GetComponent<EnemyScript>();
        }

        enemyScript.ToggleAttack(true);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyScript.ToggleAttack(false);
    }
}
