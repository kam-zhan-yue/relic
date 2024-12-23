using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.Rendering;

public class Shadow : MonoBehaviour, ISwitchable
{
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private ShadowOutline _outlineNormalScript;

    public void Init(MeshFilter sourceFilter, MeshRenderer sourceRenderer, Collider sourceCollider)
    {
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshFilter.sharedMesh = sourceFilter.sharedMesh;
        
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.material = ServiceLocator.Instance.Get<IGameManager>().GetShadowMaterial();
        _meshRenderer.shadowCastingMode = ShadowCastingMode.Off;

        _collider = gameObject.AddComponent<MeshCollider>();

        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        _rigidbody.freezeRotation = true;

        _outlineNormalScript = gameObject.AddComponent<ShadowOutline>();
        
        gameObject.SetLayerRecursively(LayerMask.NameToLayer("Shadow"));
    }

    public void Snapshot(Transform other)
    {
        transform.position = other.position;
        transform.localRotation = other.rotation;
        transform.localScale = other.localScale;
    }
}