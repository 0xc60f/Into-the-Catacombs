using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;
    
    void ShakeCamera(float secs, float amt){
        StartCoroutine(Shake(secs, amt));
    }

    IEnumerator Shake(float secs, float amt){
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amt;
             
        yield return new WaitForSeconds(secs);

        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
    }
}
