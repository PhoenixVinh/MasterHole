using System;
using System.Threading.Tasks;
using _Scripts.Event;
using _Scripts.Sound;
using _Scripts.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColdownTime : MonoBehaviour, IPrecent
{
    
    public static ColdownTime Instance;
    //[SerializeField]private Image _fillTimer;
    
    [SerializeField]private TMP_Text _txtDisplayTime;
    [SerializeField]private TMP_Text _txtLevel;
    [SerializeField]private Image imgDisplayTime;
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
       
        CalucalteTime();
        
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
                this._txtDisplayTime.color = Color.red;
                this._txtDisplayTime.transform.DOScale(Vector3.one*1.2f, 2f);
                ScaleText();
            }
        }
        else
        {
            // Process when playe lose
            WinLossEvent.OnLoss?.Invoke();
            ManagerSound.Instance?.StopEffectSound(EnumEffectSound.TimeEnd);
        }

        imgDisplayTime.fillAmount = Precent();

    }
    
    
    private void ScaleText()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(
            _txtDisplayTime.transform.DOScale(Vector3.one * 1.2f, 0.5f)
        );
        sequence.Append(
            _txtDisplayTime.transform.DOScale(Vector3.one * 0.8f, 0.5f)
        );
        sequence.SetLoops(30, LoopType.Yoyo);
        // Phóng to từ minScale đến maxScale trong duration/2 giây
        // _txtDisplayTime.transform.localScale = Vector3.one * 0.8f; // Đặt kích thước ban đầu
        // _txtDisplayTime.transform.DOScale(Vector3.one * 1.2f,  0.5f)
        //     .SetEase(Ease.InOutSine) // Tạo chuyển động mượt mà
        //     .OnComplete(() =>
        //     {
        //         
        //         _txtDisplayTime.transform.DOScale( Vector3.one * 0.8f, 0.5f)
        //             .SetEase(Ease.InOutSine)
        //             .OnComplete(ScaleText); // Gọi lại hàm để lặp
        //     });
    }

    public float Precent()
    {
        return _timeColdown / ColdownTimeComplete;
    }


    [ContextMenu("Start Coldown")]
    public async void StartColdown()
    {

        while (HoleController.Instance.HoleMovement.GetDirectionMovement().magnitude < 0.5f)
        {
            await Task.Delay(100); 
        }
        Debug.Log("Starting Coldown");

        this.isStartColdown = true;
    }




    public  void SetData(float levelTimeToComplete)
    {
        this.ColdownTimeComplete = levelTimeToComplete;
        this._timeColdown = ColdownTimeComplete;
        this.isStartColdown = false;
        isPlaySound = false;
        TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.CeilToInt(_timeColdown));
        this._txtDisplayTime.text =  string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        int level = PlayerPrefs.GetInt(StringPlayerPrefs.CURRENT_LEVEL, 1);
        _txtLevel.text = level.ToString();
        //DOTween.KillAll();
        this._txtDisplayTime.transform.localScale = Vector3.one;
        this._txtDisplayTime.color = new Color(1,0.85f,0,1);
        imgDisplayTime.fillAmount = Precent();
        StartColdown();
        
        
    }

    public void AddTime(float timeAdd)
    {
        this._timeColdown += timeAdd;
        this.ColdownTimeComplete = _timeColdown;
        DOTween.KillAll();
        this._txtDisplayTime.transform.localScale = Vector3.one;
        this._txtDisplayTime.color = new Color(1,0.85f,0,1);
        
       
    }
}