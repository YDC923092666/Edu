using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class Gamemanager : MonoBehaviour {

    //维护一个数组，表示每个元素当前生成了多少个
    public int[] countArray = new int[3];
    public static Gamemanager _instance;
    private int count = 0;//第几关
    private int[] currentArray;
    private GameObject submitButton;
    public GameObject rightEffect;
    public GameObject winEffect;
    public Transform goHolder;
    public RectTransform message;
    public RectTransform keyboard;
    public GameObject itemPanel;
    public RectTransform winMessage;

    private Tweener tw;
    private bool isGame1 = true;
    public AudioSource audioSource;
    public Text[] texts;
    public List<int> inputNumberList;
    public AudioClip[] audioClips;

    //563    365     305      530       729     472    600      404
    private List<int[]> arrayLists = new List<int[]>() {
        new int[] { 5, 6, 3 },
        new int[] { 3, 6, 5 },
        new int[] { 3, 0, 5 },
        new int[] { 5, 3, 0 },
        new int[] { 7, 2, 9 },
        new int[] { 4, 7, 2 },
        new int[] { 6, 0, 0 },
        new int[] { 4, 0, 4 },
        new int[] { 9, 2, 3 },
    };


    private void Awake()
    {
        _instance = this;
    }

    void Start () {
        Input.backButtonLeavesApp = true;
        inputNumberList = new List<int>();
        submitButton = GameObject.Find("Canvas/Button");
        ShowNumber(0);
    }

    //点击确认按钮
    public void OnSubmitButtonClick()
    {
        currentArray = arrayLists[count];
        if (Enumerable.SequenceEqual(currentArray, countArray))//判断2个数组是否相等
        {
            //正确
            Right();
        }
        else
        {
            //错误
            Error1();
        }
    }

    //显示数字，点击图形
    private void ShowNumber(int count)
    {
        if (isGame1)
        {
            itemPanel.SetActive(true);
        }
        currentArray = arrayLists[count];
        for (int i = 0; i < currentArray.Length; i++)
        {
            Text text = texts[i];
            text.text = currentArray[i].ToString();
        }
    }


    //显示图形，填写数字
    private void ShowPicture(int count)
    {
        currentArray = arrayLists[count];
        //生成第一种物体
        for (int i = 0; i < currentArray[0]; i++)
        {
            Item1._instance.OnMouseDown();
        }

        //生成第二种物体
        for (int i = 0; i < currentArray[1]; i++)
        {
            Item2._instance.OnMouseDown();
        }

        //生成第三种物体
        for (int i = 0; i < currentArray[2]; i++)
        {
            Item3._instance.OnMouseDown();
        }

        //隐藏原有选择图形面板
        if (itemPanel.activeSelf)
        {
            itemPanel.SetActive(false);
        }
        
        //显示键盘
        keyboard.DOLocalMoveY(-419, 0.5f);
    }

    private void Right()
    {
        if (isGame1)
        {
            itemPanel.SetActive(false);
        }
        PlaySound(audioClips[1]);
        //播放成功特效
        Instantiate(rightEffect);
        //关卡数+1
        count++;
        StartCoroutine(NextCount());
    }

    IEnumerator NextCount()
    {
        yield return new WaitForSeconds(1f);
        //刷新页面，显示下一组数值
        Clear();

        //游戏通关
        if (count == 9)
        {
            Win();
            yield break;
        }

        //看是哪一种游戏方式
        if (count < 5)
        {
            isGame1 = true;
            ShowNumber(count);
        }
        else
        {
            isGame1 = false;
            submitButton.SetActive(false);
            ShowPicture(count);
        }
    }
    //清空页面已有内容
    public void Clear()
    {
        //清空GoHolder里的图形
        foreach (Transform go in goHolder)
        {
            Destroy(go.gameObject);
        }
        //清空texts里的数字
        foreach (Text text in texts)
        {
            text.text = null;
        }
        
        //清空countArray数组里的内容,隐藏确定按钮
        countArray = new int[3];

        //清空inputNumberList列表里的内容
        inputNumberList.Clear();
    }
    
    private void Error1()
    {
        PlaySound(audioClips[3]);
        //清空GoHolder里的图形
        foreach (Transform go in goHolder)
        {
            Destroy(go.gameObject);
        }
        //清空countArray数组里的内容,隐藏确定按钮
        countArray = new int[3];

        //隐藏items面板
        itemPanel.SetActive(false);
        //显示错误信息
        tw = message.DOLocalMoveX(0, 0.5f);
        tw.SetAutoKill(false);
    }

    //“好”按钮点击后，错误提示信息返回原来位置
    public void OnOKButtonClick()
    {
        PlaySound(audioClips[4]);
        tw.PlayBackwards();
        if (isGame1)
        {
            //显示items面板
            itemPanel.SetActive(true);
        }
    }

    //点击小键盘UI上的0-9数字后
    public void OnNumberButtonClick(int number)
    {
        PlaySound(audioClips[4]);
        if (inputNumberList.Count > 2) return;
        inputNumberList.Add(number);
        int index = inputNumberList.Count;
        texts[index - 1].text = (inputNumberList[index - 1]).ToString();
    }

    //点击小键盘UI上的删除键后
    public void OnKeyboardDeleteButtonClick()
    {
        PlaySound(audioClips[4]);
        if (inputNumberList.Count < 1) return;
        texts[inputNumberList.Count - 1].text = null;
        inputNumberList.RemoveAt(inputNumberList.Count - 1);
    }

    //点击小键盘UI上的确认键后
    public void OnKeyboardSubmitButtonClick()
    {
        int[] inputNumberArray = inputNumberList.ToArray();
        currentArray = arrayLists[count];
        if (Enumerable.SequenceEqual(currentArray, inputNumberArray))//判断2个数组是否相等
        {
            //正确
            Right();
        }
        else
        {
            //错误
            Error2();
        }
    }

    //Game2方式错误
    private void Error2()
    {
        PlaySound(audioClips[3]);
        //清空Text里的数字
        foreach (Text text in texts)
        {
            text.text = null;
        }
        //清空inputNumberList列表里的内容
        inputNumberList.Clear();
        //显示错误信息
        tw = message.DOLocalMoveX(0, 0.5f);
        tw.SetAutoKill(false);
    }

    //音效
    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Win()
    {
        PlaySound(audioClips[2]);
        //播放成功特效
        Instantiate(winEffect);
        //显示胜利信息
        tw = winMessage.DOLocalMoveX(0, 0.5f);
        tw.SetAutoKill(false);
        keyboard.gameObject.SetActive(false);
    }

    public void OnGameOverButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
