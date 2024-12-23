using System;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    private TutorialPopup _tutorialPopup;
    protected bool completed = false;
    protected bool active = false;
    public Action onComplete;

    protected virtual void Awake()
    {
        _tutorialPopup = GetComponentInChildren<TutorialPopup>();
    }

    public void Activate()
    {
        active = true;
        _tutorialPopup.ShowPopup();
    }

    protected void Complete()
    {
        if (completed || !active)
            return;
        completed = true;
        _tutorialPopup.Complete();
        onComplete?.Invoke();
    }

    private void OnDestroy()
    {
        onComplete = null;
    }
}
