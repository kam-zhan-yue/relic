using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.Rendering;

public class Transparent : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private Rigidbody _rigidbody;

    private Flashlight flashlight;

    public void Init(MeshFilter sourceFilter, MeshRenderer sourceRenderer, Collider sourceCollider)
    {
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.sharedMesh = sourceFilter.sharedMesh;
        
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.material = ServiceLocator.Instance.Get<IGameManager>().GetTransparentMaterial();
        _meshRenderer.shadowCastingMode = ShadowCastingMode.Off;

        _collider = gameObject.AddComponent<MeshCollider>();

        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        _rigidbody.freezeRotation = true;
        
        gameObject.SetLayerRecursively(LayerMask.NameToLayer("Transparent"));

        flashlight = FindObjectsOfType<Flashlight>()[0];
    }

    public void Snapshot(Transform other)
    {
        transform.position = other.position;
        transform.localRotation = other.rotation;
        transform.localScale = other.localScale;
    }

    public void copyMaterialSymbol(Material material) {
        Material self_material = GetComponent<Renderer>().material;
        self_material.SetColor("_SymbolColour", material.GetColor("_SymbolColour"));
        self_material.SetTexture("_SymbolTex", material.GetTexture("_MainTex"));
        flashlight.etherealMaterials.Add(self_material);
    }
}
