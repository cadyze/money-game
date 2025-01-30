using UnityEngine;
using UnityEngine.Events;

public class O_Button : MonoBehaviour
{

    public UnityEvent onActivateEvents;

    private void OnActivate()
    {
        onActivateEvents.Invoke();
    }

    public void AddToActivateEvents(UnityAction unityAction)
    {
        onActivateEvents.AddListener(unityAction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            GameObject go = collision.gameObject;
            if(go.CompareTag("Bullet") && go.GetComponent<BulletController>().fromPlayer)
            {
                OnActivate();   
            }
        }
    }
}
