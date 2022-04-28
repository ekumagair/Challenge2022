using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCreateObjectOnStart : StateMachineBehaviour
{
    public GameObject instanciar;
    public Transform referencia;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        referencia = GameObject.FindGameObjectWithTag("Player").transform;
        Instantiate(instanciar, referencia.transform.position + (referencia.transform.forward * 2), referencia.transform.rotation);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Invector.vCharacterController.vThirdPersonMotor>().currentStamina *= 0.6f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(instanciar, referencia.transform.position + (referencia.transform.forward * 2), referencia.transform.rotation);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
