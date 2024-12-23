using UnityEngine;
using Cinemachine;
 
 public class DollyTrackEndSwitch : MonoBehaviour
 {
     public CinemachineDollyCart dollyCart; 
     public CinemachineVirtualCamera dollyCamera; 
     public CinemachineVirtualCamera nextCamera;
 
     void Update()
     {
         // Check if the dolly cart has reached the end of the path
         if (dollyCart.m_Position >= dollyCart.m_Path.PathLength)
         {
             SwitchToNextCamera();
         }
     }
 
     void SwitchToNextCamera()
     {
         // Reduce the priority of the dolly camera
         dollyCamera.Priority = 5;
 
         // Increase the priority of the next camera
         nextCamera.Priority = 10;
 
         // disable the script after the switch
         this.enabled = false;
     }
 }

