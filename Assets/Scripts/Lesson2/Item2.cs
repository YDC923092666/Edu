using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item2 : MonoBehaviour {

    public GameObject go;
    public static Item2 _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void OnMouseDown()
    {
        if (Gamemanager._instance.countArray[1] > 8) return;
        //播放音效
        Gamemanager._instance.PlaySound(Gamemanager._instance.audioClips[0]);
        Instantiate(go, transform.position, transform.rotation, Gamemanager._instance.goHolder);
    }
}
