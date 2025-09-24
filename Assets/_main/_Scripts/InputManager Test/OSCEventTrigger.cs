using UnityEngine;
using UnityEngine.Events; // UnityEvent를 사용하기 위해 꼭 필요합니다!

// OSCInputManager의 데이터를 감시하여 특정 조건에 따라 이벤트를 발생시키는 스크립트입니다.
public class OSCEventTrigger : MonoBehaviour
{
    // SphereController에서 사용했던 DataSource enum을 그대로 가져옵니다.
    // 감시할 데이터 소스를 Inspector에서 선택하기 위함입니다.
    public enum DataSource
    {
        None,
        RightHandX, RightHandY, RightHandZ,
        LeftHandX, LeftHandY, LeftHandZ,
        HeadX, HeadY, HeadZ,    
        // OSCInputManager에 변수를 추가할 때마다 여기에도 추가해주면 됩니다.
    }

    // --- Inspector 설정 변수들 ---
    [Header("1. Designate OSC Data Source")]
    [Tooltip("Choose the OSC data source to monitor.")]
    public DataSource sourceToMonitor;

    [Header("2. Set Trigger Condition")]
    [Tooltip("Comparison type for triggering the event.")]
    public ComparisonType comparison = ComparisonType.GreaterThan;
    public enum ComparisonType { GreaterThan, LessThan }

    [Tooltip("Threshold value for the selected OSC data source.")]
    public float threshold = 1.0f;

    [Header("3. Event to Trigger")]
    [Tooltip("Call this event when the condition is met.")]
    public UnityEvent onThresholdReached;

    // --- 내부 상태 변수 ---
    // 이벤트가 매 프레임 반복해서 실행되는 것을 방지하기 위한 상태 플래그입니다.
    private bool isTriggered = false;

    void Update()
    {
        // 1. 선택된 데이터 소스의 현재 값을 OSCInputManager에서 가져옵니다.
        float currentValue = GetValueFromSource(sourceToMonitor);

        // 2. 현재 값이 조건을 만족하는지 확인합니다.
        bool conditionMet = false;
        if (comparison == ComparisonType.GreaterThan)
        {
            conditionMet = currentValue >= threshold;
        }
        else // LessThan
        {
            conditionMet = currentValue <= threshold;
        }

        // 3. 조건이 만족되었고, 아직 트리거가 발동되지 않은 상태라면 이벤트를 실행합니다.
        if (conditionMet && !isTriggered)
        {
            Debug.Log($"트리거 발동! ({sourceToMonitor} 값: {currentValue})");
            onThresholdReached.Invoke();
            isTriggered = true; // "이미 발동됨" 상태로 변경하여 중복 실행을 막습니다.
        }
        // 4. 조건이 만족되지 않았는데, 이전에 발동된 상태였다면 다시 "준비" 상태로 되돌립니다.
        else if (!conditionMet && isTriggered)
        {
            isTriggered = false; // 다음에 다시 조건을 만족하면 발동될 수 있도록 리셋합니다.
        }
    }



    // SphereController의 함수를 그대로 가져와서 사용합니다.
    private float GetValueFromSource(DataSource source)
    {
        switch (source)
        {
            case DataSource.None: return 0f;
            case DataSource.RightHandX: return OSCInputManager.Instance.RightHandX;
            case DataSource.RightHandY: return OSCInputManager.Instance.RightHandY;
            case DataSource.RightHandZ: return OSCInputManager.Instance.RightHandZ;
            case DataSource.LeftHandX: return OSCInputManager.Instance.LeftHandX;
            case DataSource.LeftHandY: return OSCInputManager.Instance.LeftHandY;
            case DataSource.LeftHandZ: return OSCInputManager.Instance.LeftHandZ;
            case DataSource.HeadX: return OSCInputManager.Instance.HeadX;
            case DataSource.HeadY: return OSCInputManager.Instance.HeadY;
            case DataSource.HeadZ: return OSCInputManager.Instance.HeadZ;
            default: return 0f;
        }
    }
}