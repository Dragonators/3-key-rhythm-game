using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickeffect : MonoBehaviour
{
    void Start()
    {
        Invoke("Del",0.19f);
    }
    void Del()
    {
        GameObject.Destroy(this.gameObject);
    }
}
