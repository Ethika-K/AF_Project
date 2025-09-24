using UnityEngine;

public class OSCInputManager : MonoBehaviour
{
    // --- 싱글톤 구현부 (이전과 동일) ---
    public static OSCInputManager Instance { get; private set; }

    // --- 1. Raw Data 저장부 (float 형태로 변경) ---
    // Vector3 대신 각 축을 별개의 float 변수로 선언합니다.
    public float HeadX { get; private set; }
    public float HeadY { get; private set; }
    public float HeadZ { get; private set; }

    public float RightHandX { get; private set; }
    public float RightHandY { get; private set; }
    public float RightHandZ { get; private set; }

    public float LeftHandX { get; private set; }
    public float LeftHandY { get; private set; }
    public float LeftHandZ { get; private set; }

    
    // --- 2. 정수(int) 데이터를 위한 저장부 ---
    // 제스처 ID나 버튼 클릭 여부 등 정수형 데이터를 저장할 변수입니다.
    public int RightHandGesture { get; private set; }
    public int LeftHandGesture { get; private set; }


    // --- 싱글톤 초기화 (이전과 동일) ---
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // --- 3. OSC Event Receiver 연동 함수들 ---
    
    // 오른손 (Right Hand) 위치 업데이트 함수들 (float 입력)
    public void UpdateRightHandX(float value) { RightHandX = value; }
    public void UpdateRightHandY(float value) { RightHandY = value; }
    public void UpdateRightHandZ(float value) { RightHandZ = value; }

    // 왼손 (Left Hand) 위치 업데이트 함수들 (float 입력)
    public void UpdateLeftHandX(float value) { LeftHandX = value; }
    public void UpdateLeftHandY(float value) { LeftHandY = value; }
    public void UpdateLeftHandZ(float value) { LeftHandZ = value; }

    // 머리 (Head) 위치 업데이트 함수들 (float 입력)
    public void UpdateHeadX(float value) { HeadX = value; }
    public void UpdateHeadY(float value) { HeadY = value; }
    public void UpdateHeadZ(float value) { HeadZ = value; }

    // --- 4. 정수(int) 신호 처리를 위한 함수들 ---

    // 방법 A: 정수 입력을 실수(float) 변수에 저장 (자동 형 변환)
    // OSC 신호가 정수(예: 1)로 오지만 위치값으로 사용하고 싶을 때 유용합니다.
    public void UpdateRightHandX_FromInt(int value) { RightHandX = value; }
    public void UpdateRightHandY_FromInt(int value) { RightHandY = value; }
    public void UpdateRightHandZ_FromInt(int value) { RightHandZ = value; }

    // 방법 B: 정수 입력을 정수(int) 변수에 그대로 저장
    // 제스처 ID (예: 1=주먹, 2=가위, 3=보) 와 같이 상태값을 저장할 때 사용합니다.
    public void UpdateRightHandGesture(int value) { RightHandGesture = value; }
    public void UpdateLeftHandGesture(int value) { LeftHandGesture = value; }
}