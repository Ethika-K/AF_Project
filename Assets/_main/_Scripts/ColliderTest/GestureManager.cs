using UnityEngine;

// 모든 손의 Up/Down 상태를 관리하는 싱글톤 매니저입니다.
public class GestureManager : MonoBehaviour
{
    // --- 싱글톤 구현 ---
    public static GestureManager Instance { get; private set; }

    // --- 상태 변수 ---
    // 다른 스크립트들이 이 값들을 읽어서 사용합니다.
    public bool IsRightHandUp { get; private set; }
    public bool IsRightHandDown { get; private set; }
    public bool IsLeftHandUp { get; private set; }
    public bool IsLeftHandDown { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 이 매니저는 특정 씬에만 존재해도 되므로 DontDestroyOnLoad는 선택사항입니다.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- 상태 업데이트 함수 ---
    // HandTriggerZone 스크립트가 이 함수를 호출하여 상태 변경을 보고합니다.
    public void UpdateHandState(HandSide hand, VerticalState state, bool isEntering)
    {
        if (hand == HandSide.Right)
        {
            if (state == VerticalState.Up) IsRightHandUp = isEntering;
            else IsRightHandDown = isEntering;
        }
        else // HandSide.Left
        {
            if (state == VerticalState.Up) IsLeftHandUp = isEntering;
            else IsLeftHandDown = isEntering;
        }
    }
}

// 스크립트 간의 명확한 소통을 위한 enum 선언
public enum HandSide { Right, Left }
public enum VerticalState { Up, Down }