using UnityEngine;

public class ARObjectsActivator : MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
