using UnityEngine;

/// <summary>
/// OSC 신호를 받아 게임 오브젝트의 위치(Position)를 업데이트합니다.
/// 각 축(X, Y, Z)에 해당하는 public 함수를 OSC Event Receiver에 연결해야 합니다.
/// </summary>
public class OscPositionController : MonoBehaviour
{
    [Tooltip("OSC 신호로 위치를 제어할 게임 오브젝트를 여기에 할당하세요.")]
    public GameObject targetObject; // 인스펙터에서 제어할 오브젝트 (Sphere)를 선택

    // 실시간으로 OSC 값을 저장할 변수
    private Vector3 currentPosition;

    void Awake()
    {
        // 초기 위치값 저장
        currentPosition = targetObject.transform.position;

        // 디버깅 로그 추가
        Debug.Log($"[OscPositionController] Initialized with targetObject: {targetObject.name}");
    }

    void Update()
    {
        // 매 프레임마다 currentPosition에 저장된 값으로 실제 위치를 업데이트
        if (targetObject != null)
        {
            targetObject.transform.position = currentPosition;
        }
    }

    /// <summary>
    /// OSC 주소 /tx 와 연결할 함수입니다. X 위치 값을 업데이트합니다.
    /// OSC Event Receiver가 호출하며, 반드시 public이어야 합니다.
    /// </summary>
    /// <param name="value">OSC를 통해 수신된 float 값</param>
    public void SetPositionX(float value)
    {
        currentPosition.x = value;
        //Debug.Log($"[OscPositionController] X position updated to: {value}");
    }

    /// <summary>
    /// OSC 주소 /ty 와 연결할 함수입니다. Y 위치 값을 업데이트합니다.
    /// </summary>
    /// <param name="value">OSC를 통해 수신된 float 값</param>
    public void SetPositionY(float value)
    {
        currentPosition.y = value;
        //Debug.Log($"[OscPositionController] Y position updated to: {value}");
    }

    /// <summary>
    /// OSC 주소 /tz 와 연결할 함수입니다. Z 위치 값을 업데이트합니다.
    /// </summary>
    /// <param name="value">OSC를 통해 수신된 float 값</param>
    public void SetPositionZ(float value)
    {
        currentPosition.z = value;
        //Debug.Log($"[OscPositionController] Z position updated to: {value}");
    }
}