using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    private enum State
    {
        Idle,
        Dolly,
        Main
    }
    [SerializeField] private CinemachineDollyCart dollyCart; 
    [SerializeField] private CinemachineVirtualCamera dollyCamera; 
    [SerializeField] private LevelCamera levelCamera;

    private State _state = State.Idle;

    private void Awake()
    {
        dollyCart.enabled = false;
    }
    
    public void StartDolly()
    {
        dollyCart.enabled = true;
        _state = State.Dolly;
    }

    public void StopShaking()
    {
        CinemachineBasicMultiChannelPerlin perlin = dollyCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, 0f, 1f);
    }
    
    private void Update()
    {
        // Check if the dolly cart has reached the end of the path
        if (_state == State.Dolly && dollyCart.m_Position >= dollyCart.m_Path.PathLength)
        {
            SwitchToNextCamera();
        }
    }
 
   private  void SwitchToNextCamera()
    {
        // Reduce the priority of the dolly camera
        dollyCamera.Priority = 5;
 
        // Increase the priority of the next camera
        levelCamera.Focus();
        
        // Switch to main state
        _state = State.Main;
    }
}
