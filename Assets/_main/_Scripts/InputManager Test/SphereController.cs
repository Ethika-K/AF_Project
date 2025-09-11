using UnityEngine;

// Sphere 오브젝트를 제어하는 스크립트입니다.
public class SphereController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 1. 관리사무소(OSCInputManager)에 가서 최신 데이터를 물어봅니다.
        // 싱글톤이므로 .Instance로 쉽게 접근할 수 있습니다.
        float targetX = OSCInputManager.Instance.TestX;
        float targetY = OSCInputManager.Instance.TestY;
        float targetZ = OSCInputManager.Instance.TestZ;

        // 2. 받아온 데이터로 새로운 위치 값을 만듭니다.
        Vector3 newPosition = new Vector3(targetX, targetY, targetZ);

        // 3. 이 스크립트가 붙어있는 게임 오브젝트(Sphere)의 위치를 newPosition으로 변경합니다.
        transform.position = newPosition;
    }
}