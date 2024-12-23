using System;
using UnityEngine;

public class EtherealPosition : MonoBehaviour, IFlashable
{
    public bool showing = false;
    private bool canShow = true;
    private ParticleSystem.EmissionModule emission;
    private EtherealBlock parentBlock;
    private bool viewed = false;

    void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        emission = ps.emission;
    }

    void Update()
    {
        emission.enabled = !viewed || parentBlock.transform.position != transform.position;
    }

    public void SetParentBlock(EtherealBlock block)
    {
        parentBlock = block;
    }

    private void OnTriggerEnter(Collider other)
    {
        var isFlashlight = other.gameObject.TryGetComponent(out Flashlight _);
        
        if (!other.gameObject.TryGetComponent(out Switch _) && !isFlashlight)
        {
            canShow = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canShow = true;
    }

    public void Show()
    {
        if (canShow)
        {
            showing = true;
            viewed = true;
        }
    }

    public void Hide()
    {
        showing = false;
    }
}