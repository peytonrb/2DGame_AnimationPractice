using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // LateUpdate is called after all Update functions have been called
    // inherent to Unity
    void LateUpdate()
    {
        // accessing transform component in Unity of object target, maintains position y and z and updates x position
        this.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, this.transform.position.z);
    }
}
