// 2025-08-23 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using Unity.Cinemachine; // Cinemachine 네임스페이스
using OscJack;

public class OSCToSplineDolly : MonoBehaviour
{
    [Header("OSC Settings")] // OSC 설정
    [SerializeField] private string oscAddress = "/splinePosition"; // OSC 주소
    [SerializeField] private int oscPort = 3000; // OSC 포트

    [Header("Cinemachine Settings")]
    [Tooltip("제어할 CinemachineSplineDolly 컴포넌트를 여기에 할당하세요.")]
    [SerializeField] private CinemachineSplineDolly splineDolly; // Spline Dolly 컴포넌트

    [Header("Position Range")]
    [Tooltip("OSC 값 0에 매핑될 Spline Dolly의 최소 위치입니다.")]
    [SerializeField] private float minPosition = 0f; // Spline Dolly의 최소 Position 값
    [Tooltip("OSC 값 1에 매핑될 Spline Dolly의 최대 위치입니다.")]
    [SerializeField] private float maxPosition = 1f; // Spline Dolly의 최대 Position 값

    private OscServer _oscServer;

    void OnEnable()
    {
        // OSC 서버 초기화 및 주소에 콜백 함수 등록
        try
        {
            _oscServer = new OscServer(oscPort);
            _oscServer.MessageDispatcher.AddCallback(oscAddress, OnOscMessageReceived);
        }
        catch (Exception e)
        {
            Debug.LogError($"OSC 서버를 {oscPort} 포트에서 시작하는 데 실패했습니다: {e.Message}");
        }
    }

    void OnDisable()
    {
        // OSC 서버 종료
        _oscServer?.Dispose();
        _oscServer = null;
    }

    private void OnOscMessageReceived(string address, OscDataHandle data)
    {
        // 주소가 일치하고 데이터가 있는지 확인
        if (address == oscAddress && data.GetElementCount() > 0)
        {
            // OSC 메시지에서 float 값을 가져와서 Spline 위치 업데이트
            float oscValue = data.GetElementAsFloat(0);
            UpdateSplinePosition(oscValue);
        }
    }

    private void UpdateSplinePosition(float oscValue)
    {
        if (splineDolly == null)
        {
            // Spline Dolly가 할당되지 않은 경우 경고 메시지 출력
            Debug.LogWarning("제어할 Spline Dolly 컴포넌트가 할당되지 않았습니다!");
            return;
        }

        // OSC 값(0.0 ~ 1.0 범위로 가정)을 지정된 최소/최대 위치 값으로 매핑
        // Lerp 함수를 사용하여 두 값 사이를 보간합니다.
        float mappedPosition = Mathf.Lerp(minPosition, maxPosition, oscValue);
        
        // Spline Dolly의 위치(m_Position)를 직접 업데이트
        splineDolly.CameraPosition = mappedPosition;
    }
}