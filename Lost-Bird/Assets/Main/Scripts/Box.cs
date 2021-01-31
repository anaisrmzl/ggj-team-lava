using UnityEngine;

public class Box : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private Box opposite = null;
    [SerializeField] private BoxSide boxSide;
    [SerializeField] private BoxMain boxMain;
    [SerializeField] private Vector3 origin;

    private float minForce = 0.5f;

    public BoxSide BoxSide { get => boxSide; }
    public Vector3 PushOrigin { get => transform.parent.position + origin; }

    #endregion

    #region BEHAVIORS

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

        return !opposite.IsThereObjects();
    }

    private void Update()
    {
        Debug.DrawRay(transform.parent.transform.position, transform.TransformDirection(Vector3.forward) * 1, Color.green);
    }

    public bool IsThereObjects()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.parent.transform.position, transform.TransformDirection(Vector3.forward), 1.0f);
        if (hits.Length <= 0)
            return false;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == transform.parent && hits.Length == 1)
                return false;
        }

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Ground" && boxSide == BoxSide.Left)
            boxMain.Colliding = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != "Ground" && boxSide == BoxSide.Left && !boxMain.Colliding)
            boxMain.Colliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Ground" && boxSide == BoxSide.Left)
            boxMain.Colliding = false;
    }

    #endregion
}
