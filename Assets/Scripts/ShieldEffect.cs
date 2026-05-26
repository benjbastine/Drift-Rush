using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Hide by disabling renderer not the gameobject
        if (sr != null) sr.enabled = false;
    }

    public void Show()
    {
        if (sr != null)
        {
            sr.enabled = true;
            Debug.Log($"ShieldEffect shown on {transform.parent.name}");
        }
    }

    public void Hide()
    {
        if (sr != null)
        {
            sr.enabled = false;
            Debug.Log($"ShieldEffect hidden on {transform.parent.name}");
        }
    }
}