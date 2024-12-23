using Kuroneko.UtilityDelivery;
using UnityEngine;

public abstract class EtherealBehaviour : MonoBehaviour, IFlashable
{
    [SerializeField] private bool parent;
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;
    protected Collider meshCollider;
    protected Shadow shadow;
    protected Transparent transparent;
    protected bool showing = false;

    protected virtual void Awake()
    {
        gameObject.name = $"{gameObject.name}{gameObject.GetInstanceID()}";
    }

    protected virtual void Start()
    {
        meshFilter = parent ? GetComponentInChildren<MeshFilter>() : GetComponent<MeshFilter>();
        meshRenderer = parent ? GetComponentInChildren<MeshRenderer>() : GetComponent<MeshRenderer>();
        meshCollider = parent ? GetComponentInChildren<Collider>() : GetComponent<Collider>();
        // meshRenderer.enabled = false;
        InitShadow();
    }

    public virtual void Show()
    {
        if (!transparent)
        {
            InitTransparent();
        }
        else
        {
            transparent.gameObject.SetActiveFast(true);
        }
        showing = true;
        // meshRenderer.enabled = true;
        HideShadow();
    }

    public virtual void Hide()
    {
        showing = false;
        // meshRenderer.enabled = false;
        ShowShadow();
    }

    protected virtual void Update()
    {
        if(transparent)
            transparent.Snapshot(transform);
        // Update the shadow's position to itself when showing
        // if (showing)
        // {
        //     shadow.transform.position = meshRenderer.transform.position;
        // }
    }

    private void InitShadow()
    {
        GameObject shadowObject = new GameObject($"Shadow{name} {gameObject.GetInstanceID()}");
        shadow = shadowObject.AddComponent<Shadow>();
        shadow.Init(meshFilter, meshRenderer, meshCollider);
        shadow.Snapshot(transform);
        shadowObject.SetActiveFast(false);
    }

    private void InitTransparent()
    {
        GameObject transparentObject = new GameObject($"Transparent{name} {gameObject.GetInstanceID()}");
        transparent  = transparentObject.AddComponent<Transparent>();
        transparent.Init(meshFilter, meshRenderer, meshCollider);
        transparent.copyMaterialSymbol(GetComponent<Renderer>().sharedMaterial);
        transparent.Snapshot(transform);
    }

    protected virtual void ShowShadow()
    {
        shadow.gameObject.SetActiveFast(true);
        shadow.Snapshot(transform);
    }

    protected void HideShadow()
    {
        shadow.gameObject.SetActiveFast(false);
    }

    public virtual void DeleteShadow()
    {
        shadow.gameObject.SetActiveFast(false);
        shadow.Snapshot(transform);

        if (transparent)
        {
            transparent.gameObject.SetActiveFast(false);
        }
    }
}
