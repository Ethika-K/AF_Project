// 2025-08-23 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEngine;
using OscJack;
using System.Collections.Generic;

public class OSCDataManager : MonoBehaviour
{
    public static OSCDataManager Instance { get; private set; }

    [Header("OSC Settings")]
    public int oscPort = 3000; // OSC 포트 번호

    private OscServer oscServer;

    // 파싱된 데이터를 저장할 딕셔너리
    private Dictionary<string, float> parsedData = new Dictionary<string, float>();

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // OSC 서버 초기화
        oscServer = new OscServer(oscPort);
        oscServer.MessageDispatcher.AddCallback("", OnReceiveOSCMessage); // 모든 주소를 수신
    }

    void OnReceiveOSCMessage(string address, OscDataHandle data)
    {
        try
        {
            // OSC 데이터가 존재하는지 확인
            if (data != null)
            {
                // OSC 데이터의 첫 번째 요소를 float로 가져옴
                float value = data.GetElementAsFloat(0); // 첫 번째 요소만 가져옴

                // 주소와 값을 파싱하여 저장
                if (!string.IsNullOrEmpty(address))
                {
                    parsedData[address] = value; // 기존 키가 있으면 업데이트, 없으면 추가
                    Debug.Log($"Parsed OSC Data: {address} = {value}");
                }
            }
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogWarning($"OSC message at address {address} does not contain enough elements.");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to parse OSC message: {ex.Message}");
        }
    }

    public float GetValue(string address)
    {
        // 특정 주소의 값을 반환 (없으면 0 반환)
        if (parsedData.TryGetValue(address, out float value))
        {
            return value;
        }
        return 0f;
    }

    void OnDestroy()
    {
        // OSC 서버 종료
        oscServer?.Dispose();
    }
}