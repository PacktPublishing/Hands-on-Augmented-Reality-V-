using UnityEngine;
using UnityEngine.UI;

public class ChangeState : MonoBehaviour
{
    [SerializeField]
    public GameObject objectToChangeState;
    private ModuleNavigator moduleNavigator;

    void Start()
    {
        moduleNavigator = FindObjectOfType<ModuleNavigator>();
    }

    public void CState()
    {
        objectToChangeState.SetActive(!objectToChangeState.activeSelf);
        moduleNavigator.isUIOn = !moduleNavigator.isUIOn;
        GetComponent<Button>().image.color = (moduleNavigator.isUIOn ? Color.red : Color.green);
    }


}
