using UnityEngine;
using UnityEngine.Events; // UnityEvent를 사용하기 위해 꼭 필요합니다!
using System.Collections.Generic; // List를 사용하기 위해 꼭 필요합니다!

// 하나의 트리거에 대한 모든 설정과 상태를 담는 '설계도' 클래스입니다.
[System.Serializable]
public class DefinedTrigger
{
    [Tooltip("이 트리거의 역할을 알아보기 쉽게 이름을 지어주세요.")]
    public string triggerName = "New Trigger";

    [Tooltip("이 트리거를 활성화할지 여부입니다.")]
    public bool isEnabled = true;
    
    [Tooltip("감시할 OSC 데이터를 선택하세요.")]
    public DirectPositionController.DataSource sourceToMonitor;

    public enum ComparisonType { GreaterThan, LessThan }
    [Tooltip("값을 비교할 방식을 선택하세요.")]
    public ComparisonType comparison = ComparisonType.GreaterThan;

    [Tooltip("이벤트를 발생시킬 기준값입니다.")]
    public float threshold = 1.0f;
    
    // 이 트리거의 현재 상태 (읽기 전용)
    public bool IsConditionMet { get; private set; }

    // 이전 프레임의 상태를 기억하기 위한 내부 변수
    private bool _wasMetLastFrame = false;

    // Inspector에서 연결할 UnityEvent
    [Space(10)]
    public UnityEvent onTriggerEnter; // 조건이 만족되는 순간
    public UnityEvent onTriggerExit;  // 조건이 풀리는 순간

    // 매 프레임 호출될 트리거 검사 함수
    public void CheckCondition()
    {
        if (!isEnabled)
        {
            IsConditionMet = false;
            return;
        }

        float currentValue = DirectPositionController.GetValueFromDataSource(sourceToMonitor);

        if (comparison == ComparisonType.GreaterThan)
            IsConditionMet = currentValue >= threshold;
        else
            IsConditionMet = currentValue <= threshold;

        // 상태 변화 감지 및 이벤트 호출
        if (IsConditionMet && !_wasMetLastFrame)
        {
        onTriggerEnter.Invoke();
        Debug.Log($"트리거 발동! ({sourceToMonitor} 값: {currentValue})");
        }
        else if (!IsConditionMet && _wasMetLastFrame)
            onTriggerExit.Invoke();
        
        _wasMetLastFrame = IsConditionMet;
    }
}

// OSC 데이터로 위치를 제어하고, 여러 개의 정의된 트리거를 관리하는 최종 컨트롤러입니다.
public class DirectPositionController : MonoBehaviour
{
    // --- 데이터 소스 목록 (모든 스크립트가 공유) ---
    public enum DataSource
    {
        None,
        RightHandX, RightHandY, RightHandZ,
        LeftHandX, LeftHandY, LeftHandZ,
        TestZR, TestZL,
        TestYR, TestYL
    }

    // --- 위치 제어 설정 ---
    [Header("1. Position Control")]
    public DataSource xSource = DataSource.None;
    public DataSource ySource = DataSource.None;
    public DataSource zSource = DataSource.None;
    
    // ▼▼▼▼▼ [핵심 변경점: 트리거 목록] ▼▼▼▼▼
    [Header("2. Defined Triggers")]
    [Tooltip("원하는 만큼 트리거를 추가하고 설정할 수 있습니다.")]
    public List<DefinedTrigger> triggers = new List<DefinedTrigger>();
    // ▲▲▲▲▲ [여기까지 변경] ▲▲▲▲▲


    void Update()
    {
        // --- 위치 업데이트 로직 ---
        Vector3 newPosition = transform.position;
        if (xSource != DataSource.None) newPosition.x = GetValueFromDataSource(xSource);
        if (ySource != DataSource.None) newPosition.y = GetValueFromDataSource(ySource);
        if (zSource != DataSource.None) newPosition.z = GetValueFromDataSource(zSource);
        transform.position = newPosition;
        
        // ▼▼▼▼▼ [핵심 변경점: 모든 트리거 상태 업데이트] ▼▼▼▼▼
        // 목록에 있는 모든 트리거가 매 프레임 자신의 상태를 확인하도록 합니다.
        foreach (var trigger in triggers)
        {
            trigger.CheckCondition();
        }
        // ▲▲▲▲▲ [여기까지 변경] ▲▲▲▲▲
    }
    
    // GetValueFromSource 함수를 public static으로 변경하여 DefinedTrigger 클래스에서도 접근 가능하게 합니다.
    public static float GetValueFromDataSource(DataSource source)
    {
        if (OSCInputManager.Instance == null) return 0f;
        
        switch (source)
        {
            case DataSource.None: return 0f;
            case DataSource.RightHandX: return OSCInputManager.Instance.RightHandX;
            case DataSource.RightHandY: return OSCInputManager.Instance.RightHandY;
            // ... (나머지 case들도 동일) ...
            case DataSource.RightHandZ: return OSCInputManager.Instance.RightHandZ;
            case DataSource.LeftHandX: return OSCInputManager.Instance.LeftHandX;
            case DataSource.LeftHandY: return OSCInputManager.Instance.LeftHandY;
            case DataSource.LeftHandZ: return OSCInputManager.Instance.LeftHandZ;
            case DataSource.TestYR: return OSCInputManager.Instance.TestYR;
            case DataSource.TestYL: return OSCInputManager.Instance.TestYL;
            case DataSource.TestZR: return OSCInputManager.Instance.TestZR;
            case DataSource.TestZL: return OSCInputManager.Instance.TestZL;
            default: return 0f;
        }
    }
}