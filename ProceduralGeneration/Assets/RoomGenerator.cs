using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject instance = Instantiate(Resources.Load<GameObject>("Room"));
        GameObject instance2 = Instantiate(Resources.Load<GameObject>("Door"));

        instance = Instantiate(Resources.Load<GameObject>("Room"));
        instance.transform.position = new Vector3(1,0,0);
        instance2 = Instantiate(Resources.Load<GameObject>("Door"));
        instance2.transform.position = new Vector3(0.5f,0,0);

        instance = Instantiate(Resources.Load<GameObject>("Room"));
        instance.transform.position = new Vector3(1,1,0);
        instance2 = Instantiate(Resources.Load<GameObject>("Door"));
        instance2.transform.position = new Vector3(0.5f, 1, 0);

        instance = Instantiate(Resources.Load<GameObject>("Room"));
        instance2 = Instantiate(Resources.Load<GameObject>("Door"));
        instance.transform.position = new Vector3(1,1,0);
        instance2.transform.position = new Vector3(1,1,0);

        instance = Instantiate(Resources.Load<GameObject>("Room"));
        instance.transform.position = new Vector3(1,2,0);
        instance2 = Instantiate(Resources.Load<GameObject>("Door"));
        instance2.transform.position = new Vector3(1,2,0);

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
