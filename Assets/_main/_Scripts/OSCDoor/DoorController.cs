using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    Animator doorAnimator;

    public bool IsOpen = false;

    private string paramOpen = "Open";

    public void OpenDoor()
    {
        IsOpen = true;
        doorAnimator.SetBool(paramOpen, IsOpen);
    }

    public void CloseDoor()
    {
        IsOpen = false;
        doorAnimator.SetBool(paramOpen, IsOpen);
    }
    
    public void ToggleDoor()
    {
        IsOpen = !IsOpen;
        doorAnimator.SetBool(paramOpen, IsOpen);
    }
    
}
