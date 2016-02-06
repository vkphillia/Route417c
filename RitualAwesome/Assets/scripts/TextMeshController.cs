using UnityEngine;
using System.Collections;

public class TextMeshController : MonoBehaviour
{
    public string SortingLayerName = "FG";
    public int SortingOrder = 5;

    void Start ()
    {
        GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }

}
