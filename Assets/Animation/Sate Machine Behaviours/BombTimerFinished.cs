using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTimerFinished : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Bomb attachedBomb = animator.gameObject.GetComponentInParent<Bomb>();
        attachedBomb.Explode();
    }
}
