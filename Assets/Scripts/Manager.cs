using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    #region Part [VAR]
    [Space(10f)]
    [Header("Values")]
    [Space(10f)]
    [SerializeField] private float TimeForGame = 35f;
    [SerializeField] private bool StartRandomizeNeeds = true;
    [SerializeField] private List<NeedItems> Needs;
    [SerializeField] private List<ItemIcon> Icons;

    [Space(10f)]
    [Header("Objects")]
    [Space(10f)]
    [SerializeField] private Moving Camera;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private CustomSlider TimerSlider;
    [SerializeField] private TextMeshProUGUI NeedText;
    [SerializeField] private Image NeedImage;
    [SerializeField] private Animator Anim;
    [SerializeField] private BioIK.BioIK IK;
    [SerializeField] private Transform MenuPanel;
    [SerializeField] private Transform WinPanel;
    [SerializeField] private Transform LosePanel;
    [SerializeField] private Transform GameplayPanel;
    [SerializeField] private Transform ShowPanelPos;
    [SerializeField] private Transform HidePanelPos;
    [System.Serializable]
    [SerializeField] private class timerObj
    {
        public Transform timer_1;
        public Transform timer_2;
        public Transform timer_3;
        public Transform timer_go;
    }
    [SerializeField] private timerObj timer_Obj;
    [SerializeField] private Transform ConveyerObj;
    [SerializeField] private Transform ConvShow;
    [SerializeField] private Transform ConvHide;

    [Space(10f)]
    [Header("Start Signal Send")]
    [Space(10f)]
    [SerializeField] private PlayerClick SSS1;
    [SerializeField] private ConveyerSpawner SSS2;
    [SerializeField] private CallHovering SSS3;

    private bool START = false;
    public bool GetStart()
    {
        return START;
    }
    public List<NeedItems> GetNeedItems()
    {
        return Needs;
    }
    #endregion

    #region Part [START]
    void Start()
    {
        Application.targetFrameRate = 70;
        TimerText.text = Mathf.RoundToInt(TimeForGame).ToString();
        if (StartRandomizeNeeds || Needs.Count == 0)
            RandomNeed();
        //For only type) Demo code. heh :D Its a Pashalka ahahaha ^-^
        //Write me, if need my code experience: fedan.andriy@gmail.com
    }

    public void LoadGamePlay()
    {
        StartCoroutine(ShowGame());
    }

    IEnumerator ShowGame()
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * 3f;
            MenuPanel.position = Vector3.Lerp(ShowPanelPos.position, HidePanelPos.position, time);
            yield return new WaitForEndOfFrame();
        }
        MenuPanel.gameObject.SetActive(false);
        bool once = true;
        while (once) // for wait 0.3f before show first timer objects
        {
            once = false;
            yield return new WaitForSeconds(0.3f);
        }
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        int timer = 3;
        while (timer >= 0f)
        {
            if (timer == 3)
                StartCoroutine(ShowTimeObj(timer_Obj.timer_3));
            if (timer == 2)
                StartCoroutine(ShowTimeObj(timer_Obj.timer_2));
            if (timer == 1)
                StartCoroutine(ShowTimeObj(timer_Obj.timer_1));
            if (timer == 0)
                StartCoroutine(ShowTimeObj(timer_Obj.timer_go, true));
            timer--;
            yield return new WaitForSecondsRealtime(1);
        }
        SetGame(true);
        SetNeedPanel();
    }

    IEnumerator ShowTimeObj(Transform obj, bool last = false)
    {
        float timer = 1f;
        obj.gameObject.SetActive(true);
        while (timer > 0f)
        {
            obj.localScale = Vector3.Lerp(obj.localScale, Vector3.one, timer * 10f * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0.1f;
        if (last)
        {
            TextMeshProUGUI objText = obj.GetComponent<TextMeshProUGUI>();
            while (timer > 0f)
            {
                objText.color = new Color(1, 1, 1, timer / 0.1f);
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            Image objImg = obj.GetComponent<Image>();
            while (timer > 0f)
            {
                objImg.color = new Color(1, 1, 1, timer / 0.1f);
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        obj.gameObject.SetActive(false);
    }

    private void RandomNeed()
    {
        Needs = new List<NeedItems>();
        Needs.Add(new NeedItems(RandomType(), Random.Range(1, 6)));
    }

    private Item.Type RandomType()
    {
        return (Item.Type)Random.Range(0, System.Enum.GetValues(typeof(Item.Type)).Length); ;
    }

    private void SetNeedPanel()
    {
        for (int i = 0; i < Icons.Count; i++)
        {
            if (Icons[i].Type == Needs[0].Type)
            {
                NeedImage.sprite = Icons[i].Icon;
                NeedText.text = Needs[0].Count + " " + Needs[0].Type.ToString();
            }
        }
    }
    #endregion

    #region Part [GAME]
    public void SetGame(bool value)
    {
        START = value;
        
        //START value Set in Scripts
        StartSender(value);
        
        //TIMER
        if (START)
        {
            StartCoroutine(TimerGame());
        }
        else
        {
            StopAllCoroutines();
        }

    }

    public void StartSender(bool value)
    {
        SSS1.SetStart(value);
        SSS2.SetStart(value);
        SSS3.SetStart(value);
    }


    IEnumerator TimerGame()
    {
        float allTime = TimeForGame;
        while (TimeForGame > 0f)
        {
            TimeForGame -= Time.deltaTime;
            TimerText.text = Mathf.RoundToInt(TimeForGame).ToString();
            TimerSlider.SetValue(TimeForGame / allTime);
            yield return new WaitForEndOfFrame();
        }
        TimeOut();
    }
    #endregion

    #region Part [END]
    public void Win()
    {
        SetGame(false);
        Debug.Log("WIN!");
        Camera.Move();
        StartCoroutine(MovePanel(GameplayPanel, ShowPanelPos, HidePanelPos));
        StartCoroutine(MovePanel(WinPanel, HidePanelPos, ShowPanelPos));
        IK.enabled = false;
        Anim.SetBool("Win", true);
        FindObjectOfType<Basket>().EndGame();
        StartCoroutine(MovePanel(ConveyerObj, ConvHide, ConvShow));
    }
    public void FullBasket()
    {
        SetGame(false);
        Debug.Log("LOSE! <= Basket Full!");
        StartCoroutine(MovePanel(LosePanel, HidePanelPos, ShowPanelPos));
    }
    public void TimeOut()
    {
        SetGame(false);
        Debug.Log("LOSE! <= Time Out!");
        StartCoroutine(MovePanel(LosePanel, HidePanelPos, ShowPanelPos));
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator MovePanel(Transform obj, Transform from, Transform to)
    {
        float time = 0f;
        obj.gameObject.SetActive(true);
        while (time < 1f)
        {
            time += Time.deltaTime * 3f;
            obj.position = Vector3.Lerp(from.position, to.position, time);
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}

// Custom class for icons on [Needs] panel
[System.Serializable]
public class ItemIcon
{
    public Item.Type Type;
    public Sprite Icon;
}
