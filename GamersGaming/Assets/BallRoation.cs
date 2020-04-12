using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRoation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Light discoLight = lightGameObject.AddComponent<Light>();
        discoLight.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, 0.05);
    }
}
