using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item3 : MonoBehaviour {

    public GameObject go;
    public static Item3 _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void OnMouseDown()
    {
        if (Gamemanager._instance.countArray[2] > 8) return;
        //播放音效
        Gamemanager._instance.PlaySound(Gamemanager._instance.audioClips[0]);
        Instantiate(go, transform.position, transform.rotation, Gamemanager._instance.goHolder);
    }
}
