using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPurchased : StateMachineBehaviour
{

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.localPosition = Vector3.zero;
        FeedbackAnimationManager.instance.RecycleFX(animator.gameObject);
    }
}
