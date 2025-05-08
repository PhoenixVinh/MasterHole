using System;
using System.Threading.Tasks;
using _Scripts.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColdownTime : MonoBehaviour, IPrecent
{
    
    public static ColdownTime Instance;
    [SerializeField]private Image _fillTimer;
    
    private TMP_Text _txtDisplayTime;
    
    
    public float ColdownTimeComplete = 300;
    private float _timeColdown = 0;
    
    
    
    private bool isStartColdown = false;



    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _timeColdown = ColdownTimeComplete;
        _txtDisplayTime = transform.Find("LabelTime").GetComponent<TMP_Text>();
       

    }


    public void OnEnable()
    {
        TimeEvent.OnFreezeTime += FreezeTime;
    }

    public void OnDisable()
    {
        TimeEvent.OnFreezeTime -= FreezeTime;
    }

    private async void FreezeTime(float time)
    {
        isStartColdown = false;
        await Task.Delay((int)time * 1000);
        isStartColdown = true;
    }

    private void FixedUpdate()
    {
        if(!isStartColdown) return;
        else
        {
            CalucalteTime();
        }
    }

    private void CalucalteTime()
    {
        if (_timeColdown >= 0)
        {
            _timeColdown -= Time.deltaTime;
        
            TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.CeilToInt(_timeColdown));
            this._txtDisplayTime.text =  string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            _fillTimer.fillAmount = Precent();
        }
        else
        {
            // Process when playe lose
            WinLossEvent.OnLoss?.Invoke();
        }
        
    }

    public float Precent()
    {
        return _timeColdown / ColdownTimeComplete;
    }


    [ContextMenu("Start Coldown")]
    public void StartColdown()
    {
        this.isStartColdown = true;
    }




    public void SetData(float levelTimeToComplete)
    {
        this.ColdownTimeComplete = levelTimeToComplete;
        this._timeColdown = ColdownTimeComplete;
        StartColdown();
    }

    public void AddTime(float timeAdd)
    {
        this._timeColdown += timeAdd;
        this.ColdownTimeComplete = _timeColdown;
       
    }
}