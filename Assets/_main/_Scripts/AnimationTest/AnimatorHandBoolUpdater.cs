using UnityEngine;

// DirectPositionController의 트리거 상태를 '지정된' Animator의 Bool 파라미터에
// 매 프레임 업데이트해주는 스크립트입니다.
[RequireComponent(typeof(DirectPositionController))] // 이제 Animator가 같은 오브젝트에 있을 필요가 없습니다.
public class AnimatorHandBoolUpdater : MonoBehaviour
{
    private DirectPositionController _controller;
    
    // Cached Animator parameter hashes and previous values for edge detection
    private int _rightHandUpHash;
    private int _rightHandDownHash;
    private int _leftHandUpHash;
    private int _leftHandDownHash;
    
    private bool _prevRightUp;
    private bool _prevRightDown;
    private bool _prevLeftUp;
    private bool _prevLeftDown;
    
    // 전환 중인지 확인하기 위한 변수들
    private float _lastTransitionTime;
    private const float TRANSITION_COOLDOWN = 0.1f; // 0.1초 쿨다운

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

        // 파라미터 해시 캐싱 (Animator가 연결된 경우)
        if (targetAnimator != null)
        {
            _rightHandUpHash = Animator.StringToHash(rightHandUpParamName);
            _rightHandDownHash = Animator.StringToHash(rightHandDownParamName);
            _leftHandUpHash = Animator.StringToHash(leftHandUpParamName);
            _leftHandDownHash = Animator.StringToHash(leftHandDownParamName);
        }
    }

    void Update()
    {
        // Target Animator나 Controller가 없으면 아무것도 하지 않습니다.
        if (targetAnimator == null)
        {
            Debug.LogWarning("AnimatorHandBoolUpdater: Target Animator가 연결되지 않았습니다!", this);
            return;
        }
        
        if (_controller == null)
        {
            Debug.LogWarning("AnimatorHandBoolUpdater: DirectPositionController를 찾을 수 없습니다!", this);
            return;
        }
        
        if (_controller.triggers.Count == 0)
        {
            Debug.LogWarning($"AnimatorHandBoolUpdater: 트리거가 설정되지 않았습니다. (현재 개수: {_controller.triggers.Count})", this);
            return;
        }
        
        // 인덱스 유효성 검사 (안전성 강화)
        if (!IsValidIndex(rightHandUpTriggerIndex) ||
            !IsValidIndex(rightHandDownTriggerIndex) ||
            !IsValidIndex(leftHandUpTriggerIndex) ||
            !IsValidIndex(leftHandDownTriggerIndex))
        {
            Debug.LogWarning($"AnimatorHandBoolUpdater: 잘못된 트리거 인덱스입니다. " +
                           $"RightUp:{rightHandUpTriggerIndex}, RightDown:{rightHandDownTriggerIndex}, " +
                           $"LeftUp:{leftHandUpTriggerIndex}, LeftDown:{leftHandDownTriggerIndex} " +
                           $"(총 트리거 개수: {_controller.triggers.Count})", this);
            return;
        }

        // 현재 프레임 입력값 읽기
        bool isRightUp = _controller.triggers[rightHandUpTriggerIndex].IsConditionMet;
        bool isRightDown = _controller.triggers[rightHandDownTriggerIndex].IsConditionMet;
        bool isLeftUp = _controller.triggers[leftHandUpTriggerIndex].IsConditionMet;
        bool isLeftDown = _controller.triggers[leftHandDownTriggerIndex].IsConditionMet;

        // 디버그: 트리거 상태 출력 (변화가 있을 때만)
        if (isRightUp != _prevRightUp || isRightDown != _prevRightDown || 
            isLeftUp != _prevLeftUp || isLeftDown != _prevLeftDown)
        {
            Debug.Log($"AnimatorHandBoolUpdater: 트리거 상태 변화 - " +
                     $"RightUp:{isRightUp}, RightDown:{isRightDown}, " +
                     $"LeftUp:{isLeftUp}, LeftDown:{isLeftDown}", this);
        }

        // 값이 변할 때만 SetBool 호출 (엣지-트리거 동작)
        SetBoolIfChanged(_rightHandUpHash, rightHandUpParamName, isRightUp, ref _prevRightUp);
        SetBoolIfChanged(_rightHandDownHash, rightHandDownParamName, isRightDown, ref _prevRightDown);
        SetBoolIfChanged(_leftHandUpHash, leftHandUpParamName, isLeftUp, ref _prevLeftUp);
        SetBoolIfChanged(_leftHandDownHash, leftHandDownParamName, isLeftDown, ref _prevLeftDown);
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < _controller.triggers.Count;
    }

    private void SetBoolIfChanged(int paramHash, string paramName, bool newValue, ref bool previousValue)
    {
        if (newValue == previousValue)
        {
            return; // 변화 없으면 아무것도 하지 않음 (재트리거 방지)
        }

        // Animator 파라미터 존재 여부 확인
        bool paramExists = false;
        foreach (AnimatorControllerParameter param in targetAnimator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Bool)
            {
                paramExists = true;
                break;
            }
        }

        if (!paramExists)
        {
            Debug.LogError($"AnimatorHandBoolUpdater: Animator에 '{paramName}' Bool 파라미터가 존재하지 않습니다!", this);
            return;
        }

        // Animator가 현재 전환 중인지 확인
        if (targetAnimator.IsInTransition(0))
        {
            Debug.Log($"AnimatorHandBoolUpdater: {paramName} = {newValue} 신호 무시 (전환 중)", this);
            return; // 전환 중이면 신호 무시
        }
        
        // 쿨다운 시간 확인 (최근 전환 후 일정 시간 대기)
        if (Time.time - _lastTransitionTime < TRANSITION_COOLDOWN)
        {
            Debug.Log($"AnimatorHandBoolUpdater: {paramName} = {newValue} 신호 무시 (쿨다운 중)", this);
            return; // 쿨다운 중이면 신호 무시
        }

        // Animator에 설정 (해시가 0일 수 있으므로 이름도 폴백으로 처리)
        if (paramHash != 0)
        {
            targetAnimator.SetBool(paramHash, newValue);
            Debug.Log($"AnimatorHandBoolUpdater: {paramName} = {newValue} (해시 사용)", this);
        }
        else
        {
            targetAnimator.SetBool(paramName, newValue);
            Debug.Log($"AnimatorHandBoolUpdater: {paramName} = {newValue} (이름 사용)", this);
        }

        // 전환 시간 기록
        _lastTransitionTime = Time.time;
        previousValue = newValue;
    }
    

}