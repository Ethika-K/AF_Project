// 2025-08-23 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using Cinemachine;
using OscJack;

public class OSCToSplineDolly : MonoBehaviour
{
    [Header("OSC Settings")] // OSC 설정
    [SerializeField] private string oscAddress = "/splinePosition"; // OSC 주소
    [SerializeField] private int oscPort = 3000; // OSC 포트 (3000으로 변경)

    [Header("Cinemachine Settings")]
    [SerializeField] private CinemachineSplineDolly splineDolly; // Spline Dolly 컴포넌트

    [Header("Position Range")]
    [SerializeField] private float minPosition = 0f; // Spline Dolly의 최소 Position 값
    [SerializeField] private float maxPosition = 1f; // Spline Dolly의 최대 Position 값

    private OscServer oscServer;

    private void Start()
    {
        // OSC 서버 초기화
        oscServer = new OscServer(oscPort);
        oscServer.MessageDispatcher.AddCallback(oscAddress, OnOscMessageReceived);
    }

    private void OnOscMessageReceived(string address, OscDataHandle data)
    {
        if (address == oscAddress && data.GetElementCount() > 0)
        {
            float oscValue = data.GetElementAsFloat(0); // OSC 값 가져오기
            UpdateSplinePosition(oscValue);
        }
    }

    private void UpdateSplinePosition(float oscValue)
    {
        if (splineDolly == null)
        {
            Debug.LogWarning("Spline Dolly is not assigned!");
            return;
        }

        // OSC 값을 Spline Dolly Position 값으로 매핑
        float mappedPosition = Mathf.Clamp(oscValue, 0f, 1f) * (maxPosition - minPosition) + minPosition;
        splineDolly.m_SplineSettings.Position = mappedPosition;
    }

    private void OnDestroy()
    {
        // OSC 서버 종료
        oscServer?.Dispose();
    }
}