using SuperMaxim.Messaging;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private bool _triggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.TryGetComponent(out Player _))
        {
            _triggered = true;
            Messenger.Default.Publish(new NextLevelPayload());
        }
    }
}
