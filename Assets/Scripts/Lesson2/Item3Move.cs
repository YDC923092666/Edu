using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3Move : MonoBehaviour {

    public float speed = 60f;
    private Vector3 end;
    private int count;

    void Start () {
        Gamemanager._instance.countArray[2]++;
        count = Gamemanager._instance.countArray[2];
        float y = 3.5f - 0.7f * count;
        end = new Vector3(transform.position.x, y, transform.position.z);
        //GetComponent<SpriteRenderer>().sortingOrder = count;
    }
	
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
    }
}
