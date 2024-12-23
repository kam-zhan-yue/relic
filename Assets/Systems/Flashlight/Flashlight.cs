using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    [SerializeField] private Light torch;

    [SerializeField] public List<Material> etherealMaterials = new List<Material>();
    public Light Torch => torch;

    private bool _active;
    public bool Active
    {
        get => _active;
        set
        {
            _active = value;
            if(_active)
                Activate();
            else
                Deactivate();
        }
    }

    private List<IFlashable> _visible = new List<IFlashable>();
    private static readonly int SpotlightColor = Shader.PropertyToID("_SpotlightColor");
    private static readonly int SpotlightDirection = Shader.PropertyToID("_SpotlightDirection");
    private static readonly int SpotlightPosition = Shader.PropertyToID("_SpotlightPosition");
    private static readonly int SpotlightRange = Shader.PropertyToID("_SpotlightRange");
    private static readonly int SpotlightIntensity = Shader.PropertyToID("_SpotlightIntensity");
    private static readonly int SpotlightAngle = Shader.PropertyToID("_SpotlightAngle");


    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        Active = false;
    }
    
    private void Update()
    {
        for (int i = 0; i < etherealMaterials.Count; ++i)
        {
            if (torch.type == LightType.Spot && Active)
            {
                // Pass spotlight position
                etherealMaterials[i].SetVector(SpotlightPosition, torch.transform.position);
                
                // Pass spotlight direction
                etherealMaterials[i].SetVector(SpotlightDirection, torch.transform.forward);

                // Pass spotlight color
                etherealMaterials[i].SetColor(SpotlightColor, torch.color);

                // Pass attenuation parameters (if necessary)
                etherealMaterials[i].SetFloat(SpotlightRange, torch.range);
                etherealMaterials[i].SetFloat(SpotlightIntensity, torch.intensity);
                etherealMaterials[i].SetFloat(SpotlightAngle, torch.spotAngle);
            }
            else
            {
                etherealMaterials[i].SetFloat(SpotlightRange, 0);
            }
        }
    }

    private void Activate()
    {
        _meshRenderer.enabled = true;
        _meshCollider.enabled = true;
        torch.enabled = true;
    }

    private void Deactivate()
    {
        if (_meshRenderer)
            _meshRenderer.enabled = false;
        if (_meshCollider)
            _meshCollider.enabled = false;
        for (int i = 0; i < _visible.Count; ++i)
        {
            _visible[i].Hide();
        }

        _visible.Clear();
        torch.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IFlashable flashable)) return;
        flashable.Show();
        _visible.Add(flashable);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IFlashable flashable)) return;
        flashable.Hide();
        _visible.Remove(flashable);
    }
}
