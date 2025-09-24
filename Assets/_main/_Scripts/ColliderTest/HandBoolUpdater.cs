using UnityEngine;

public class HandBoolUpdater : MonoBehaviour
{
    private Animator _animator;
    
    [Header("Animator Parameter Names")]
    public string rightHandUpParamName = "RightHandUp";
    public string rightHandDownParamName = "RightHandDown";
    public string leftHandUpParamName = "LeftHandUp";
    public string leftHandDownParamName = "LeftHandDown";

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // GestureManager가 없으면 아무것도 하지 않음
        if (GestureManager.Instance == null || _animator == null) return;

        // 1. GestureManager에서 4개 상태의 현재 값(bool)을 가져옵니다.
        bool isRightUp = GestureManager.Instance.IsRightHandUp;
        bool isRightDown = GestureManager.Instance.IsRightHandDown;
        bool isLeftUp = GestureManager.Instance.IsLeftHandUp;
        bool isLeftDown = GestureManager.Instance.IsLeftHandDown;

        // 2. Animator의 Bool 파라미터 4개를 현재 상태로 업데이트합니다.
        _animator.SetBool(rightHandUpParamName, isRightUp);
        _animator.SetBool(rightHandDownParamName, isRightDown);
        _animator.SetBool(leftHandUpParamName, isLeftUp);
        _animator.SetBool(leftHandDownParamName, isLeftDown);
    }
}