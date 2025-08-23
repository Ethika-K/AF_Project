using System;
using UnityEditor;
using UnityEngine;

public class HeadXYZUpdater : MonoBehaviour
{
    public GameObject head; // head 오브젝트
    public string oscAddressX = "/p1/head:tx"; // X 좌표에 해당하는 OSC 주소
    public string oscAddressY = "/p1/head:ty"; // Y 좌표에 해당하는 OSC 주소
    public string oscAddressZ = "/p1/head:tz"; // Z 좌표에 해당하는 OSC 주소

    void Update()
    {
        if (head != null && OSCDataManager.Instance != null)
        {
            // OSC 데이터를 가져와서 Position 업데이트
            float x = OSCDataManager.Instance.GetValue(oscAddressX);
            float y = OSCDataManager.Instance.GetValue(oscAddressY);
            float z = OSCDataManager.Instance.GetValue(oscAddressZ);

            // 새로운 Position 계산
            Vector3 newPosition = new Vector3(x, y, z-5.0f);
            // head의 Position 업데이트
            head.transform.position = newPosition;}
    }
}