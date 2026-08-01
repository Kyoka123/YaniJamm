using UnityEngine;

public class NoEraser : MonoBehaviour
{
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (transform.parent != null)
        {
            Vector3 parentScale = transform.parent.localScale;
            transform.localScale = new Vector3(
                initialScale.x / parentScale.x,
                initialScale.y / parentScale.y,
                initialScale.z / parentScale.z
            );
        }
    }
}
