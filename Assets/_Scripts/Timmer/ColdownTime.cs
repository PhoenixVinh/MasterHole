using System;
using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColdownTime : MonoBehaviour, IPrecent
{
    
    public static ColdownTime Instance;
    //[SerializeField]private Image _fillTimer;
    
    private TMP_Text _txtDisplayTime;
    
    
    public float ColdownTimeComplete = 300;
    private float _timeColdown = 0;
    
    
    
    private bool isStartColdown = false;


    private bool isPlaySound = false;


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
            //_fillTimer.fillAmount = Precent();
            if (_timeColdown <= 30 && !isPlaySound)
            {
                if (ManagerSound.Instance != null)
                {
                    ManagerSound.Instance.PlayEffectSound(EnumEffectSound.TimeEnd);
                    isPlaySound = true;
                }
            }
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
    public async void StartColdown()
    {

        while (HoleController.Instance.HoleMovement.GetDirectionMovement() != Vector2.zero)
        {
            await Task.Delay(100); 
        }
        this.isStartColdown = true;
    }




    public void SetData(float levelTimeToComplete)
    {
        this.ColdownTimeComplete = levelTimeToComplete;
        this._timeColdown = ColdownTimeComplete;
        StartColdown();
        isPlaySound = false;
        
    }

    public void AddTime(float timeAdd)
    {
        this._timeColdown += timeAdd;
        this.ColdownTimeComplete = _timeColdown;
       
    }
}