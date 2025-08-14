using UnityEngine;
using OscJack;

public class OscReceiverTest : MonoBehaviour
{
    private OscServer _server;

    void Start()
    {
        _server = new OscServer(3000); // 3000번 포트 리스닝 시작
        _server.MessageDispatcher.AddCallback(
            "/test", // TouchDesigner에서 보내는 OSC 주소와 일치시킬 것
            (string address, OscDataHandle data) =>
            {
                Debug.Log("OSC message received: " + address);
                // 데이터 처리 코드
            }
        );
    }

    void OnDestroy()
    {
        _server?.Dispose();
    }
}