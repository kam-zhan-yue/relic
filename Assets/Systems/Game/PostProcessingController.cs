using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume postProcessingVolume;
    // Start is called before the first frame update
    [SerializeField] private Statue[] statues;
    [SerializeField] private Player player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Set closest distance to FAR
        float closestDistance = 100.0f;
        for (int i=0;i<statues.Length;i++) {
            // Check if statue can move and if so, compare distances
            if (statues[i].getCanMove && statues[i].getActivated) {
                float distance = Vector3.Distance(statues[i].transform.position, player.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                }
            }
        }
        postProcessingVolume.profile.TryGetSettings(out Vignette vignette);
        // If the closest statue is less than 5 meters, lerp vignette between 0.4 (minimum) and 1 (max)
        if (closestDistance < 5.0f) {
            float vignetteValue = Mathf.Lerp(0.2f, 1, (5f - closestDistance)/5.0f);
            vignette.intensity.value = vignetteValue;
        } else {
            vignette.intensity.value = 0.2f;
        }
    }
}
