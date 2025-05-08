
using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Event;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Tutorial
{
    public class TutorialLevel01 : BaseTutorial
    {

        [Header("Collider Bounds")]
        public GameObject wall;
        
        
        [Header("UI Elements")]
        public GameObject panels;
        public GameObject moveMessage;
        public GameObject collectMessage;
        public GameObject missionMessage;



        private bool isShowMoveMessage = false;




        private int amountItemCollect = 16;
        
        
        private void Start()
        {
            //this.holeController.HoleMovement.SetColliderArea(this.limitCollider);
            // ShowPanel
            isShowMoveMessage = false;
            amountItemCollect = 16;
            panels.SetActive(false);
            moveMessage.SetActive(false);
            collectMessage.SetActive(false);
            missionMessage.SetActive(false);
            StartCoroutine(ShowMoveMessage());
            StartCoroutine(ShowCollectMessage());
        }

        private IEnumerator ShowCollectMessage()
        {
            while (!isShowMoveMessage)
            {
                yield return null;
            }
            panels.SetActive(true);
            collectMessage.transform.SetParent(this.holeController.transform.Find("Canvas"));
            yield return new WaitForSeconds(1f);
            collectMessage.SetActive(true);
          
            //yield return new WaitForSeconds(7f);
            
            //Debug.Log("?????");
            //collectMessage.SetActive(false);
        }


        public IEnumerator  ShowMoveMessage()
        {
            yield return new WaitUntil(
                () => ManagerLevelGamePlay.Instance.SpawnLevel().Result
            );
            wall.SetActive(true);
            moveMessage.SetActive(true);
            //moveMessage.transform.SetParent(this.HoleController.transform);
            
            while (this.holeController.HoleMovement.GetDirectionMovement() == Vector2.zero)
            {
                yield return null;
            }

            isShowMoveMessage = true;
            moveMessage.transform.SetParent(holeController.transform.Find("Canvas"));
            
            yield return new WaitForSeconds(2f);
            moveMessage.transform.DOScale(Vector3.zero, 0.5f).OnComplete(
                () => Destroy(moveMessage)
                
                );
            //Debug.Log("?????");
            


            //Debug.Log(moveMessage.transform.position);
        }

        public void OnEnable()
        {
            ItemEvent.OnAddScore += MissionTutorial;
        }

        private async void MissionTutorial(int score)
        {
            amountItemCollect--;
            if (amountItemCollect == 0)
            {

                collectMessage.transform.DOScale(Vector3.zero, 0.3f).OnComplete(
                    () =>
                    {

                        Destroy(collectMessage);
                        panels.SetActive(false);
                        wall.SetActive(false);
                    }

                );
                
               
            
                missionMessage.SetActive(true);
                await Task.Delay(3000);
                missionMessage.transform.DOScale(Vector3.zero, 0.3f).OnComplete(
                    () => Destroy(missionMessage)
                    );

            }
        }
    }
}