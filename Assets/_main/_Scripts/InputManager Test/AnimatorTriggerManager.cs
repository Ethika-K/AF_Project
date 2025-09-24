using UnityEngine;
using System.Collections.Generic; // List, HashSet을 사용하기 위해 필요합니다.

// DirectPositionController의 트리거 상태를 감지하여,
// 각 상태를 '단 한 번만' 실행시키는 Animator Trigger를 관리합니다.
[RequireComponent(typeof(DirectPositionController), typeof(Animator))]
public class AnimatorTriggerManager : MonoBehaviour
{
    private DirectPositionController _controller;
    private Animator _animator;

    // "이미 실행된 상태"들의 이름을 기억하기 위한 저장소입니다.
    // HashSet은 특정 항목이 포함되어 있는지 매우 빠르게 확인할 수 있어 이런 용도에 적합합니다.
    private HashSet<string> _triggeredStates = new HashSet<string>();

    void Awake()
    {
        _controller = GetComponent<DirectPositionController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 컨트롤러에 4개의 트리거가 모두 설정되었는지 확인합니다.
        if (_controller.triggers.Count < 4) return;

        // 1. DirectPositionController에서 4개 트리거의 현재 상태를 가져옵니다.
        bool isRightUp = _controller.triggers[0].IsConditionMet;
        bool isRightDown = _controller.triggers[1].IsConditionMet;
        bool isLeftUp = _controller.triggers[2].IsConditionMet;
        bool isLeftDown = _controller.triggers[3].IsConditionMet;

        // 2. 현재 상태 조합에 따라 "상태 이름"을 결정합니다.
        string currentStateName = GetStateName(isRightUp, isRightDown, isLeftUp, isLeftDown);

        // 3. 상태가 결정되었고("None"이 아님), 아직 한 번도 실행된 적이 없다면
        if (currentStateName != "None" && !_triggeredStates.Contains(currentStateName))
        {
            // Animator의 Trigger를 발동시킵니다.
            _animator.SetTrigger(currentStateName);
            
            // "이 상태는 이제 실행되었음" 이라고 기억 저장소에 추가합니다.
            _triggeredStates.Add(currentStateName);

            Debug.Log($"상태 <{currentStateName}> 가 한 번 실행되었습니다.");
        }
    }

    /// <summary>
    /// 4개의 bool 값을 조합하여 9개 상태 중 하나의 이름을 반환합니다.
    /// </summary>
    private string GetStateName(bool rUp, bool rDown, bool lUp, bool lDown)
    {
        string rightState = (rUp) ? "Up" : (rDown) ? "Down" : "Mid";
        string leftState = (lUp) ? "Up" : (lDown) ? "Down" : "Mid";
        return $"{rightState}&&{leftState}"; // 예: "Up&&Mid"
    }
    
    /// <summary>
    /// 모든 기억을 지우고, 애니메이터를 초기 상태로 되돌립니다.
    /// 이 함수는 외부(예: UI 버튼)에서 호출할 수 있습니다.
    /// </summary>
    public void ResetAllStates()
    {
        _triggeredStates.Clear();
        _animator.SetTrigger("Reset"); // Animator에 "Reset" Trigger가 필요합니다.
        Debug.Log("모든 상태 기억이 리셋되었습니다.");
    }
}