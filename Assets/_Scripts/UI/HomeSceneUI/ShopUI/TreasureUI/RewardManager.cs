using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts.UI.HomeSceneUI.ShopUI.TreasureUI
{
    
    
    public class DataReward
    {
        public int id;
        public string amound;
    }
    
    
    
    public class RewardManager : MonoBehaviour
    {
        public List<Transform> positions;
        public List<Sprite> images;

        public GameObject item;

        public Transform positionStart;
        public ChestAnim chestAnim;

        public List<GameObject> items;


        public void SetData(List<DataReward> rewards)
        {
            this.gameObject.SetActive(true);
            StartCoroutine(SetDataCourutine(rewards));

        }
        
        public IEnumerator SetDataCourutine(List<DataReward> rewards)
        {
            chestAnim.PlayChestAnim();
            yield return new WaitForSecondsRealtime(1f);
            int count = 0;
            foreach (var data in rewards)
            {
                GameObject itemA = Instantiate(item, transform);
                items.Add(itemA);
                var itemAnim = itemA.GetComponent<ItemAnim>();
                itemAnim.SetData(images[data.id], data.amound, positionStart.position, positions[count].position);

                yield return new WaitForSecondsRealtime(0.5f);
                count++;
            }
            yield return new WaitForSecondsRealtime(0.6f);
            foreach (var item in items)
            {
                Destroy(item);
            }
            this.gameObject.SetActive(false);
        }





    }
}