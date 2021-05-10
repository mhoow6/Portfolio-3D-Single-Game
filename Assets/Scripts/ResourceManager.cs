using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    protected string village;
    protected string villagePath;
    private string forest;
    protected string forestPath;

    private void Awake()
    {
        village = "Village.csv";
        villagePath = Application.dataPath + "/Table/" + village;

        forest = "Forest.csv";
        forestPath = Application.dataPath + "/Table/" + forest;
    }
}
