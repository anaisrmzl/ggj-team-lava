using UnityEngine;

public class Edge : MonoBehaviour
{
    #region FIELDS

    public Vector3 TileCenter { get => transform.parent.position; }
    public Vector3 Direction { get => transform.forward; }

    #endregion

    #region BEHAVIORS

    public bool AreThereObjects()
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

    #endregion
}
