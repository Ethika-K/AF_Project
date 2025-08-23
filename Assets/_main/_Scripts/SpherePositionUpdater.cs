// 2025-08-23 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;

public class SpherePositionUpdater : MonoBehaviour
{
    public GameObject sphere; // Sphere 오브젝트
    public string oscAddressX = "/p1/hand_r:tx"; // X 좌표에 해당하는 OSC 주소
    public string oscAddressY = "/p1/hand_r:ty"; // Y 좌표에 해당하는 OSC 주소
    public string oscAddressZ = "/p1/hand_r:tz"; // Z 좌표에 해당하는 OSC 주소

    void Update()
    {
        if (sphere != null && OSCDataManager.Instance != null)
        {
            // OSC 데이터를 가져와서 Position 업데이트
            float x = OSCDataManager.Instance.GetValue(oscAddressX);
            float y = OSCDataManager.Instance.GetValue(oscAddressY);
            float z = OSCDataManager.Instance.GetValue(oscAddressZ);

            // 새로운 Position 계산
            Vector3 newPosition = new Vector3(x, y, (z-10.0f)*4.0f);
            // Sphere의 Position 업데이트
            sphere.transform.position = newPosition;}
    }
}
