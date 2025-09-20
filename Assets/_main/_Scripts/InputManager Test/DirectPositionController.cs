using UnityEngine;

// 여러 개의 '정의된' 트리거 조건을 관리하는 클래스입니다.
[System.Serializable] // 이 클래스를 Inspector에 표시하기 위해 필요합니다.
public class DefinedTrigger
{
    [Tooltip("이 트리거를 활성화할지 여부입니다.")]
    public bool isEnabled = true;
    
    [Tooltip("감시할 OSC 데이터를 선택하세요.")]
    public DirectPositionController.DataSource sourceToMonitor;

    public enum ComparisonType { GreaterThan, LessThan }
    [Tooltip("값을 비교할 방식을 선택하세요.")]
    public ComparisonType comparison = ComparisonType.GreaterThan;

    [Tooltip("이벤트를 발생시킬 기준값입니다.")]
    public float threshold = 1.0f;
    
    // 이 트리거의 현재 상태를 저장하는 변수입니다. (외부에서는 직접 수정 불가)
    public bool IsConditionMet { get; private set; }

    // 이전 프레임의 상태를 기억하기 위한 내부 변수입니다.
    private bool _wasMetLastFrame = false;

    // 매 프레임 호출될 트리거 검사 함수입니다.
    public void CheckCondition()
    {
        if (!isEnabled)
        {
            IsConditionMet = false;
            return;
        }

        // OSCInputManager에서 현재 값을 가져옵니다.
        float currentValue = DirectPositionController.GetValueFromDataSource(sourceToMonitor);
        
        // 조건을 검사합니다.
        if (comparison == ComparisonType.GreaterThan)
        {
            IsConditionMet = currentValue >= threshold;
        }
        else // LessThan
        {
            IsConditionMet = currentValue <= threshold;
        }

        // TODO: 만약 "조건 만족" 이벤트가 필요하다면 여기에 추가할 수 있습니다.
        // if (IsConditionMet && !_wasMetLastFrame) { /* 이벤트 호출 */ }
        
        _wasMetLastFrame = IsConditionMet;
    }
}


// OSC 데이터로 위치를 제어하고, 5개의 정의된 트리거 상태를 관리하는 컨트롤러입니다.
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
    public DataSource zSource = DataSource.RightHandZ;
    

    // ▼▼▼▼▼ [핵심 변경점 1: 5개의 정의된 트리거 선언] ▼▼▼▼▼
    [Header("2. Defined Triggers")]
    public DefinedTrigger rightHandUp;
    public DefinedTrigger leftHandUp;
    public DefinedTrigger handsTogether; // 예시: 두 손이 가까이 있는지
    public DefinedTrigger rightHandPunch; // 예시: 오른손을 앞으로 뻗었는지
    public DefinedTrigger customTrigger5; // 예시: 5번째 커스텀 트리거

    // 다른 스크립트에서 쉽게 접근할 수 있도록 public bool 변수를 제공합니다.
    public bool IsRightHandUp => rightHandUp.IsConditionMet;
    public bool IsLeftHandUp => leftHandUp.IsConditionMet;
    public bool AreHandsTogether => handsTogether.IsConditionMet;
    public bool IsRightHandPunch => rightHandPunch.IsConditionMet;
    public bool IsCustomTrigger5Active => customTrigger5.IsConditionMet;
    // ▲▲▲▲▲ [여기까지 변경] ▲▲▲▲▲

    public Animator animator; // 애니메이터 컴포넌트 참조
    public void Step01(float x)
    {
        animator.SetTrigger("MoveRight");
    }
    void Update()
    {
        // --- 위치 업데이트 로직 (기존과 동일) ---
        Vector3 newPosition = transform.position;
        if (xSource != DataSource.None) newPosition.x = GetValueFromDataSource(xSource);
        if (ySource != DataSource.None) newPosition.y = GetValueFromDataSource(ySource);
        if (zSource != DataSource.None) newPosition.z = GetValueFromDataSource(zSource);
        transform.position = newPosition;
        
        // ▼▼▼▼▼ [핵심 변경점 2: 모든 트리거 상태 업데이트] ▼▼▼▼▼
        // 정의된 5개의 트리거가 매 프레임 자신의 상태를 확인하도록 합니다.
        rightHandUp.CheckCondition();
        leftHandUp.CheckCondition();
        handsTogether.CheckCondition();
        rightHandPunch.CheckCondition();
        customTrigger5.CheckCondition();
        // ▲▲▲▲▲ [여기까지 변경] ▲▲▲▲▲
    }
    
    // ▼▼▼▼▼ [핵심 변경점 3: static 함수로 변경] ▼▼▼▼▼
    // GetValueFromSource 함수를 public static으로 변경하여 DefinedTrigger 클래스에서도 접근 가능하게 합니다.
    public static float GetValueFromDataSource(DataSource source)
    {
        // OSCInputManager가 없는 경우를 대비한 방어 코드
        if (OSCInputManager.Instance == null) return 0f;
        
        switch (source)
        {
            case DataSource.None: return 0f;
            case DataSource.RightHandX: return OSCInputManager.Instance.RightHandX;
            case DataSource.RightHandY: return OSCInputManager.Instance.RightHandY;
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
    // ▲▲▲▲▲ [여기까지 변경] ▲▲▲▲▲
}