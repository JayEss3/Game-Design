using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRoation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*GameObject discoBall = this.gameObject;
        Light discoLight = discoBall.AddComponent<Light>();
        discoLight.color = Color.blue;
        discoLight.intensity = 80;
        discoLight.range = 30;*/
        GameObject sideArch;
        Light outerLight;
        for (int i = 0; i < 3; i++)
        {
            sideArch = this.gameObject.transform.parent.gameObject.transform.GetChild(i).gameObject;
            outerLight = sideArch.AddComponent<Light>();
            outerLight.color = Color.green;
            outerLight.intensity = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 5f, 0);
    }
}
