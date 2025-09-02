using UnityEngine;

public class DoorPosController : MonoBehaviour
{
    // Inspector 창에서 문이 열렸을 때와 닫혔을 때의 X 위치를 설정할 수 있습니다.
    [Tooltip("문이 1을 받았을 때 이동할 X 좌표")]
    public float positionX_Positive = 2.0f;

    [Tooltip("문이 -1을 받았을 때 이동할 X 좌표")]
    public float positionX_Negative = -2.0f;

    // 문의 처음 위치를 저장할 변수 (Y, Z 좌표는 바꾸지 않기 위함)
    private Vector3 initialPosition;

    void Awake()
    {
        // 스크립트가 시작될 때 문의 초기 위치를 저장합니다.
        initialPosition = transform.position;
    }

    /// <summary>
    /// OSC Event Receiver가 호출할 함수입니다.
    /// 반드시 public이어야 하며, int 파라미터가 하나 있어야 합니다.
    /// </summary>
    /// <param name="direction">OSC를 통해 수신된 정수 값 (1 또는 -1)</param>
    public void SetDoorPosition(int direction)
    {
        Vector3 targetPosition = initialPosition;

        // OSC로 받은 값이 1이면 positionX_Positive 위치로 설정
        if (direction == 1)
        {
            //Debug.Log("OSC: 1 수신. 문을 Positive 위치로 이동합니다.");
            targetPosition.x = positionX_Positive;
        }
        // OSC로 받은 값이 -1이면 positionX_Negative 위치로 설정
        else if (direction == -1)
        {
            //Debug.Log("OSC: -1 수신. 문을 Negative 위치로 이동합니다.");
            targetPosition.x = positionX_Negative;
        }

        // 계산된 최종 위치로 문을 이동시킵니다.
        transform.position = targetPosition;
    }
}