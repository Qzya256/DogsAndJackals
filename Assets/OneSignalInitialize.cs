using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneSignalSDK;

public class OneSignalInitialize : MonoBehaviour
{
    void Start()
    {
        OneSignal.Default.Initialize("db99e946-7931-4b9f-8dcc-5ced8e0cabfa");
    }
}
