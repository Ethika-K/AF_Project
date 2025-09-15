using UnityEngine;

// 어떤 OSC 데이터든 연결하거나, 연결하지 않을 축을 선택할 수 있는 범용 컨트롤러 스크립트입니다.
public class SphereController : MonoBehaviour
{
    // 1. 선택 가능한 데이터 목록 만들기 (enum)
    // Inspector 창에서 이 목록이 드롭다운 메뉴로 표시됩니다.
    public enum DataSource
    {
        None, // '사용 안 함' 옵션 추가
        RightHandX, RightHandY, RightHandZ,
        LeftHandX, LeftHandY, LeftHandZ,
        TestZR, TestZL,
        TestYR, TestYL
    }

    // 2. Inspector에서 각 축에 연결할 데이터 소스를 선택할 변수들
    [Header("OSC Data Connection")]
    [Tooltip("X축 위치에 연결할 OSC 데이터를 선택하세요. 'None'을 선택하면 현재 X값을 유지합니다.")]
    public DataSource xSource = DataSource.None; // 기본값을 None으로 변경

    [Tooltip("Y축 위치에 연결할 OSC 데이터를 선택하세요. 'None'을 선택하면 현재 Y값을 유지합니다.")]
    public DataSource ySource = DataSource.None; // 기본값을 None으로 변경

    [Tooltip("Z축 위치에 연결할 OSC 데이터를 선택하세요. 'None'을 선택하면 현재 Z값을 유지합니다.")]
    public DataSource zSource = DataSource.RightHandZ; // 예시로 Z만 기본값 설정


    void Update()
    {
        // 3. 현재 오브젝트의 위치를 기본값으로 먼저 가져옵니다.
        Vector3 newPosition = transform.position;

        // 4. 각 축의 데이터 소스가 'None'이 아닐 경우에만 OSCInputManager에서 값을 가져와 덮어씁니다.
        if (xSource != DataSource.None)
        {
            newPosition.x = GetValueFromSource(xSource);
        }

        if (ySource != DataSource.None)
        {
            newPosition.y = GetValueFromSource(ySource);
        }

        if (zSource != DataSource.None)
        {
            newPosition.z = GetValueFromSource(zSource);
        }
        
        // 5. 최종적으로 계산된 위치로 오브젝트를 이동시킵니다.
        transform.position = newPosition;
    }

    // enum 값에 따라 OSCInputManager에서 해당하는 float 값을 반환하는 함수
    private float GetValueFromSource(DataSource source)
    {
        switch (source)
        {
            // None에 대한 처리는 Update 함수에서 이미 했으므로 여기서는 신경 쓰지 않아도 됩니다.
            // case DataSource.None: return ??? // 이 부분은 필요 없어졌습니다.
            
            case DataSource.RightHandX: return OSCInputManager.Instance.RightHandX;
            case DataSource.RightHandY: return OSCInputManager.Instance.RightHandY;
            case DataSource.RightHandZ: return OSCInputManager.Instance.RightHandZ;

            case DataSource.LeftHandX: return OSCInputManager.Instance.LeftHandX;
            case DataSource.LeftHandY: return OSCInputManager.Instance.LeftHandY;
            case DataSource.LeftHandZ: return OSCInputManager.Instance.LeftHandZ;

            case DataSource.TestYR: return OSCInputManager.Instance.TestYR;
            case DataSource.TestYL: return OSCInputManager.Instance.TestYL;

            case DataSource.TestZR: return OSCInputManager.Instance.TestZR;
            case DataSource.TestZL: return OSCInputManager.Instance.TestZL;
                
            default: return 0f;
        }
    }
}