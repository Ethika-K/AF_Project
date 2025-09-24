using UnityEngine;

// 손의 진입/이탈을 감지하여 GestureManager에 보고하는 스크립트입니다.
public class HandTriggerZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [Tooltip("이 영역이 어떤 손을 감지할지 설정합니다.")]
    public HandSide targetHand;

    [Tooltip("이 영역이 Up 상태를 의미하는지, Down 상태를 의미하는지 설정합니다.")]
    public VerticalState zoneState;

    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (GestureManager.Instance == null)
        {
            Debug.LogWarning("HandTriggerZone: GestureManager.Instance가 없습니다. 씬에 GestureManager를 배치했는지 확인하세요.", this);
            return;
        }
        // 들어온 오브젝트의 태그가 우리가 감시하려는 손의 태그와 일치하는지 확인
        // 예: targetHand가 Right이면 "RightHand" 태그를 찾음
        if (other.CompareTag(targetHand.ToString() + "Hand"))
        {
            // GestureManager에 "오른손이 Up 영역에 진입했다"고 보고
            GestureManager.Instance.UpdateHandState(targetHand, zoneState, true);
            Debug.Log($"{targetHand} Hand Entered {zoneState} Zone");
        }
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    private void OnTriggerExit(Collider other)
    {
        if (GestureManager.Instance == null)
        {
            Debug.LogWarning("HandTriggerZone: GestureManager.Instance가 없습니다. 씬에 GestureManager를 배치했는지 확인하세요.", this);
            return;
        }
        // 나간 오브젝트의 태그가 우리가 감시하려는 손의 태그와 일치하는지 확인
        if (other.CompareTag(targetHand.ToString() + "Hand"))
        {
            // GestureManager에 "오른손이 Up 영역에서 이탈했다"고 보고
            GestureManager.Instance.UpdateHandState(targetHand, zoneState, false);
            Debug.Log($"{targetHand} Hand Exited {zoneState} Zone");
        }
    }
}