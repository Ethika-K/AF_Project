using UnityEngine;
using OscJack;

public class OscReceiveTest : MonoBehaviour
{
    // 수신 포트와 확인할 OSC 주소
    public int port = 3000;
    public string address = "127.0.0.1";

    OscServer server;

    void Start()
    {
        server = new OscServer(port);
        server.MessageDispatcher.AddCallback(
            address,
            (string addr, OscDataHandle data) =>
            {
                // 수신된 데이터 내용 로그
                Debug.Log($"OSC: {addr} {data.GetElementAsFloat(0)}");
            }
        );
    }

    void OnDestroy()
    {
        server?.Dispose();
    }
}
