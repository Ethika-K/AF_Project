using UnityEngine;

// DirectPositionController의 트리거 상태를 '지정된' Animator의 Bool 파라미터에
// 매 프레임 업데이트해주는 스크립트입니다.
[RequireComponent(typeof(DirectPositionController))] // 이제 Animator가 같은 오브젝트에 있을 필요가 없습니다.
public class AnimatorHandBoolUpdater : MonoBehaviour
{
    private DirectPositionController _controller;
    
    // ▼▼▼▼▼ [핵심 변경점 1: public Animator 변수] ▼▼▼▼▼
    [Header("1. Target Animator")]
    [Tooltip("이 스크립트로 제어할 Animator 컴포넌트를 여기에 연결하세요.")]
    public Animator targetAnimator;
    // ▲▲▲▲▲ [여기까지 변경] ▲▲▲▲▲


    [Header("2. Trigger-Parameter Mapping")]
    [Tooltip("DirectPositionController의 triggers 목록에서 몇 번째 인덱스를 쓸지 지정합니다.")]
    public int rightHandUpTriggerIndex = 0;
    public int rightHandDownTriggerIndex = 1;
    public int leftHandUpTriggerIndex = 2;
    public int leftHandDownTriggerIndex = 3;

    [Header("3. Animator Parameter Names")]
    [Tooltip("Animator에 생성한 Bool 파라미터의 이름을 정확히 입력하세요.")]
    public string rightHandUpParamName = "RightHandUp";
    public string rightHandDownParamName = "RightHandDown";
    public string leftHandUpParamName = "LeftHandUp";
    public string leftHandDownParamName = "LeftHandDown";

    void Awake()
    {
        _controller = GetComponent<DirectPositionController>();

        // ▼▼▼▼▼ [핵심 변경점 2: 연결 확인] ▼▼▼▼▼
        // 만약 Inspector에서 Animator를 연결하지 않았다면 경고 메시지를 표시합니다.
        if (targetAnimator == null)
        {
            Debug.LogError("Target Animator가 연결되지 않았습니다!", this.gameObject);
        }
        // ▲▲▲▲▲ [여기까지 변경] ▲▲▲▲▲
    }

    void Update()
    {
        // Target Animator나 Controller가 없으면 아무것도 하지 않습니다.
        if (targetAnimator == null || _controller == null || _controller.triggers.Count < 4)
        {
            return;
        }

        // --- 로직은 기존과 동일하지만, _animator 대신 targetAnimator를 사용합니다. ---
        bool isRightUp = _controller.triggers[rightHandUpTriggerIndex].IsConditionMet;
        bool isRightDown = _controller.triggers[rightHandDownTriggerIndex].IsConditionMet;
        bool isLeftUp = _controller.triggers[leftHandUpTriggerIndex].IsConditionMet;
        bool isLeftDown = _controller.triggers[leftHandDownTriggerIndex].IsConditionMet;

        targetAnimator.SetBool(rightHandUpParamName, isRightUp);
        targetAnimator.SetBool(rightHandDownParamName, isRightDown);
        targetAnimator.SetBool(leftHandUpParamName, isLeftUp);
        targetAnimator.SetBool(leftHandDownParamName, isLeftDown);
    }
}