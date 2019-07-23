using UnityEngine;
using TMPro;

public class Module : MonoBehaviour
{
    public string nameOfPrefab;
    private ModuleNavigator moduleNavigator;

    void Start()
    {
        nameOfPrefab = GetComponent<MeshFilter>().mesh.name.Replace(" Instance", "");
        transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = nameOfPrefab;
        moduleNavigator = FindObjectOfType<ModuleNavigator>();
    }

    public void Click()
    {
        moduleNavigator.receivedName = nameOfPrefab;
        moduleNavigator.Spawn();
    }

}
