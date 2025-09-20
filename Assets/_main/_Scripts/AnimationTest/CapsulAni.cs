using UnityEngine;

// 'Idle' 상태일 때만 활성화되어 'IsRightHandUp'과 'IsLeftHandUp' 조건을 감시하는 스크립트입니다.
public class CapsuleAni : StateMachineBehaviour
{
    // DirectPositionController를 담을 변수
    private DirectPositionController controller;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 'Idle' 상태에 진입하는 순간, 애니메이터가 붙어있는 게임 오브젝트에서
        // DirectPositionController 컴포넌트를 찾아서 변수에 저장해둡니다.
        // OnStateUpdate에서 매번 찾는 것보다 훨씬 효율적입니다.
        if (controller == null)
        {
            controller = animator.GetComponent<DirectPositionController>();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 컨트롤러를 성공적으로 찾았다면
        if (controller != null)
        {
            // 'Idle' 상태인 매 프레임마다 양손의 상태를 순서대로 확인합니다.

            // 1. 오른손이 올라갔는지 먼저 확인합니다.
            if (controller.IsRightHandUp)
            {
                // 'MoveRight' 트리거를 발동시켜 'CapsuleAni_MoveRight' 상태로 전환합니다.
                animator.SetTrigger("MoveRight");
            }
            // 2. 오른손이 올라가지 않았을 경우, 왼손이 올라갔는지 확인합니다.
            else if (controller.IsLeftHandUp)
            {
                // 'MoveLeft' 트리거를 발동시켜 'CapsuleAni_MoveLeft' 상태로 전환합니다.
                animator.SetTrigger("MoveLeft");
            }
        }
    }
}