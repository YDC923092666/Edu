using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lesson1_Item2 : MonoBehaviour {

    public GameObject go;
    public static Lesson1_Item2 _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void OnMouseDown()
    {
        if (Lesson1_Gamemanager._instance.countArray[0] > 8) return;
        //播放音效
        Lesson1_Gamemanager._instance.PlaySound(Lesson1_Gamemanager._instance.audioClips[0]);
        Instantiate(go, transform.position, transform.rotation, Lesson1_Gamemanager._instance.goHolder);
    }
}
