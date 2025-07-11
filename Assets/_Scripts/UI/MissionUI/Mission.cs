using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.ObjectPooling;
using _Scripts.Sound;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.MissionUI
{
    public class Mission : MonoBehaviour
    {

        [SerializeField] private GameObject IconMission;
        private int indexMission;
        
        
        public int amountItem;
        public string itemType;
        
        [SerializeField]private TMP_Text _text;
        [SerializeField]private Image image;

        
        [SerializeField] private ParticleSystem particle;
        public AnimationCurve heightCurve;
       
        public bool IsDone() => amountItem == 0;
        public int GetAmountItem() => amountItem;

        
        private string IdDotween = "MissionDT";


        // Check if the mission has been completed
        [SerializeField] private GameObject checkMark;

        private HashSet<GameObject> itemSpawnMissions;

        public void SetData(MissionData missionData, int index)
        {
            checkMark.SetActive(false);
              this._text.gameObject.SetActive(true);
            itemSpawnMissions = new HashSet<GameObject>();
            particle.Stop();
            this.amountItem = missionData.AmountItems;
            this.itemType = missionData.idItem;
            _text.text = this.amountItem.ToString();
            image.sprite = missionData.image;    
            this.indexMission = index;
        }
        public Sprite GetImage() => image.sprite;
        public void MinusItem()
        {
            AddItem();
        }

        private async void AddItem()
        {


            GameObject EffectMission = MissionPooling.Instance.spawnImage();

            itemSpawnMissions.Add(EffectMission);

            Vector3 screenPosition = UnityEngine.Camera.main.WorldToScreenPoint(HoleController.Instance.transform.position);
            EffectMission.GetComponent<RectTransform>().anchoredPosition = screenPosition;

            EffectMission.GetComponent<Image>().sprite = image.sprite;
            EffectMission.SetActive(true);


            // Using Dotween To move Object
            if (ManagerSound.Instance != null)
            {
                ManagerSound.Instance.PlayEffectSound(EnumEffectSound.ItemMission);
            }


            //EffectMission.transform.localScale = new Vector3(1, 1, 1) * 1.2f;


            amountItem--;
            amountItem = amountItem >= 0 ? amountItem : 0;
            StartCoroutine(MoveWithCurve(0.8f, screenPosition, this.transform.position, EffectMission));

            EffectMission.transform.localScale = Vector3.zero;
            DOTween.Sequence()
                .SetUpdate(true)
                .Append(EffectMission.transform.DOScale(Vector3.one * 1f, 0.6f))
                .Append(EffectMission.transform.DOScale(Vector3.one * 0.7f, 0.6f));


            if (amountItem == 0)
            {


                // EffectMission.SetActive(false);
               
                await Task.Delay(800);
                Debug.Log("Mission Completed: " + itemType);
                checkMark.SetActive(true);
                this._text.gameObject.SetActive(false);


                //DOTween.Kill(transform);
                //DOTween.KillAll();
                StartCoroutine(RotateCoroutine());

              

                await Task.Delay(1500);
                StartCoroutine(DelayedAction());
                    
                    // Smooth, consistent speed
                   
                    
                 
                //gameObject.SetActive(false);
                //Destroy(gameObject);
            }


            
        }

        private IEnumerator RotateCoroutine()
        {
            float duration = 1.5f;
            float elapsed = 0f;

            Vector3 initialRotation = transform.rotation.eulerAngles;
            Vector3 targetRotation = initialRotation + new Vector3(0, 180 * 7, 0);

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                transform.rotation = Quaternion.Euler(Vector3.Lerp(initialRotation, targetRotation, t));
                elapsed += Time.unscaledDeltaTime; // Use unscaled time for consistent rotation
                yield return null; // Wait for the next frame
            }

            transform.rotation = Quaternion.Euler(targetRotation);
        }

        public IEnumerator DelayedAction()
        {

            yield return new WaitForSecondsRealtime(0.5f);
            DOTween.Kill(transform);// Delay 0.5 giây trên unscaled time
            transform.DOScale(Vector3.one * 0.3f, 0.5f).SetUpdate(true)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    //Destroy(gameObject);
                });
        }
            
       
        private IEnumerator MoveWithCurve(float moveDuration, Vector3 startPoint, Vector3 endPoint, GameObject effectMission)
        {



            float currentTime = 0f;

            while (currentTime < moveDuration)
            {
                currentTime += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(currentTime / moveDuration); // Tỷ lệ thời gian (0 -> 1)

                // Tính toán vị trí theo AnimationCurve
                Vector3 newPosition = CalculateCurvePosition(t, startPoint, transform.position);
                effectMission.transform.position = newPosition;

                yield return null; // Chờ frame tiếp theo

            }

            // Đảm bảo vị trí cuối cùng chính xác
            effectMission.transform.position = endPoint;
            effectMission.SetActive(false);


            DOTween.Sequence()
                .SetUpdate(true)
                .Append(transform.DOScale(Vector3.one * 1.4f, 0.2f))
                .Append(transform.DOScale(Vector3.one, 0.1f))
                .Join(IconMission.transform.DOShakeScale(0.3f, 0.3f))

                .OnComplete(
                    () =>
                    {
                        IconMission.transform.localScale = Vector3.one;

                    });
            if (_text != null)
            {
                _text.text = amountItem.ToString();
            }

            if (particle != null)
            {
                particle.Play();
            }

        }

        private Vector3 CalculateCurvePosition(float t, Vector3 startPoint, Vector3 endPoint)
        {


            var heightC = -100;
            if (indexMission == 0)
            {
                heightC = -60;
            }
            else
            {
                heightC = -100;
            }
            
            
            float y = Mathf.Lerp(startPoint.y, endPoint.y, t);
            
            float curveValue = heightCurve.Evaluate(t); // Giá trị từ 0 đến 1
            float x = Mathf.Lerp(startPoint.x, endPoint.x, t) + curveValue * heightC;

            x = x > 100 ? x : 100;
           
            return new Vector3(x, y, 0);
        }
        private IEnumerator PlayEffectCoroutine(GameObject EffectMission)
        {
        

            // Lưu vị trí và tỷ lệ ban đầu
            Vector3 startPosition = EffectMission.transform.position;
            Vector3 startScale = EffectMission.transform.localScale;
            Vector3 targetScale = new Vector3(0.7f, 0.7f, 0.7f);
            float moveDuration = 0.8f;
            float startTime = Time.realtimeSinceStartup;
            float elapsedTime = 0f;

            // Phần 1: Di chuyển và thu nhỏ EffectMission
            while (elapsedTime < moveDuration)
            {
                if (EffectMission == null || this.transform == null)
                {
                    yield break; // Thoát nếu đối tượng bị hủy
                }

                elapsedTime = Time.realtimeSinceStartup - startTime;
                float t = Mathf.Clamp01(elapsedTime / moveDuration); 

                // Di chuyển đến vị trí mới nhất của this.transform.position
                EffectMission.transform.position = Vector3.Lerp(startPosition, this.transform.position, t);
                // Thu nhỏ tỷ lệ
                EffectMission.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                yield return null; // Chờ frame tiếp theo
            }

            // Đảm bảo vị trí và tỷ lệ cuối cùng chính xác
            if (EffectMission != null)
            {
                EffectMission.transform.position = this.transform.position;
                EffectMission.transform.localScale = targetScale;
                EffectMission.SetActive(false); // Tắt EffectMission
            }

            // Phần 2: Phóng to và thu nhỏ transform
            if (this.transform != null)
            {
                // Phóng to lên 1.2 trong 0.2 giây
                yield return StartCoroutine(ScaleTransformCoroutine(Vector3.one * 1.2f, 0.2f));
                // Thu về 1.0 trong 0.1 giây
                yield return StartCoroutine(ScaleTransformCoroutine(Vector3.one, 0.1f));
            }

            // Cập nhật text và phát particle
            if (_text != null)
            {
                _text.text = amountItem.ToString();
            }
            if (particle != null)
            {
                particle.Play();
            }
        }

        // Coroutine để thay đổi tỷ lệ của transform
        private IEnumerator ScaleTransformCoroutine(Vector3 targetScale, float duration)
        {
            if (this.transform == null)
            {
                yield break;
            }

            Vector3 startScale = this.transform.localScale;
            float startTime = Time.realtimeSinceStartup; // Thời gian thực ban đầu
            
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                if (this.transform == null)
                {
                    yield break;
                }

                elapsedTime = Time.realtimeSinceStartup - startTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                this.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

                yield return null;
            }

            // Đảm bảo tỷ lệ cuối cùng chính xác
            if (this.transform != null)
            {
                this.transform.localScale = targetScale;
            }
        }
            

        private void OnDestroy()
        {
            
            
            
            DOTween.Kill(IdDotween);
            DOTween.KillAll();
            StopAllCoroutines();
            foreach (GameObject item in itemSpawnMissions)
            {
                if (item != null && item.activeSelf)
                {
                    item.SetActive(false);
                }
            }

        }

        public void SetIndexMission(int index)
        {
            indexMission = index;
        }
    }
}