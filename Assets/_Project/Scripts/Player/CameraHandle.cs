using Unity.Cinemachine;
using UnityEngine;

public class CameraHandle : MonoBehaviour
{
    //[SerializeField] private Transform cameraPosition;
    private CinemachineVirtualCameraBase cinemachineCamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cinemachineCamera == null)
        {
            return;
        }
    }
    public void SetupCamera()
    {
        cinemachineCamera = CinemachineCore.GetVirtualCamera(0);
        if (cinemachineCamera != null)
        {
            //cinemachineCamera.ForceCameraPosition(cameraPosition.position,  Quaternion.LookRotation(transform.position, Vector3.up));
            cinemachineCamera.Follow = transform;
        }
        else
        {
            Debug.LogError("No active Cinemachine virtual camera found.");
        }
    }
}
