using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 로딩 UI 없이 씬 로딩만 담당하는 간단한 버전의 스크립트입니다.
public class SceneLoader : MonoBehaviour
{
    /// GameManager가 호출할 공용 함수입니다.
    /// <param name="sceneName">로드할 씬의 이름</param>
    public void LoadScene(string sceneName)
    {
        // 비동기 로딩을 사용하면 씬이 매우 가벼울 경우 한 프레임만에 로딩이 완료됩니다.
        // 이는 거의 동기 로딩과 유사한 경험을 주면서도, 혹시 모를 무거운 씬 전환 시에도
        // 게임이 멈추는 것을 최소화해주는 장점이 있습니다.
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// UI 업데이트 없이 비동기적으로 씬만 로드하는 코루틴입니다.
    /// <param name="sceneName">로드할 씬의 이름</param>
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // 비동기 씬 로딩을 시작하고, 완료될 때까지 기다립니다.
        yield return SceneManager.LoadSceneAsync(sceneName);
        
        // 씬 로딩이 완료되면 이 코루틴은 자동으로 종료됩니다.
    }
}