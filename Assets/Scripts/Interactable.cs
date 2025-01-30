using UnityEngine;

public class Interactable : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log($"{gameObject.name} interacted with!");
    }
}
