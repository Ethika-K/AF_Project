using UnityEngine;

// 게임의 전체적인 상태와 '이름 기반' 씬 전환을 관리하는 총괄 매니저입니다.
public class GameManager : MonoBehaviour
{
    // 싱글톤 구현 (이전과 동일)
    public static GameManager Instance { get; private set; }

    [Header("Scene Loader Reference")]
    [Tooltip("Connect the SceneLoader component here.")]
    public SceneLoader sceneLoader; // 행동대장 SceneLoader

    // 현재 씬의 이름을 기억하기 위한 변수 (선택 사항)
    public string CurrentSceneName { get; private set; }

    private void Awake()
    {
        // 싱글톤 설정 (이전과 동일)
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

    // ▼▼▼▼▼ 바로 이 함수가 핵심입니다! ▼▼▼▼▼
    /// <summary>
    /// 문자열로 받은 씬 이름을 로드하도록 SceneLoader에게 명령합니다.
    /// 이 함수를 Inspector의 이벤트에 연결하여 사용합니다.
    /// </summary>
    /// <param name="sceneName">로드할 씬의 이름</param>
    public void LoadSceneByName(string sceneName)
    {
        // 씬 이름이 비어있지 않은지 확인
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("로드할 씬 이름이 비어있습니다!");
            return;
        }

        Debug.Log($"{sceneName} 씬을 로드하라는 명령을 받았습니다.");

        // SceneLoader에게 "이 씬을 로드해!" 라고 명령을 내림
        sceneLoader.LoadScene(sceneName);

        // 현재 씬 이름을 기록
        CurrentSceneName = sceneName;
    }
    // ▲▲▲▲▲ 이 함수 하나면 모든 것이 해결됩니다! ▲▲▲▲▲
}