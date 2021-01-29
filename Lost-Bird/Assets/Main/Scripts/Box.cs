using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Box opposite = null;
    [SerializeField] private BoxSide boxSide;
    [SerializeField] private Vector3 origin;

    private float minForce = 0.8f;

    public bool IsColliding { get; private set; }
    public BoxSide BoxSide { get => boxSide; }
    public Vector3 PushOrigin { get => transform.parent.position + origin; }

    private void OnTriggerEnter(Collider other)
    {
        IsColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsColliding = false;
    }

    public bool CanPush(Vector2 force)
    {
        switch (boxSide)
        {
            case BoxSide.Up:
                if (force.y > -minForce)
                    return false;
                break;
            case BoxSide.Down:
                if (force.y < minForce)
                    return false;
                break;
            case BoxSide.Left:
                if (force.x < minForce)
                    return false;
                break;
            case BoxSide.Right:
                if (force.x > -minForce)
                    return false;
                break;
        }

        return !opposite.IsColliding;
    }
}
