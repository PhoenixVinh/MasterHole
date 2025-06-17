using _Scripts.Booster;
using _Scripts.Firebase;
using _Scripts.UI;

namespace _Scripts.Tutorial
{
    public class TutorialLevel09:TutorialLevel03
    {
        public override void FreeBooster()
        {
            
            ManagerBooster.Instance.ChangeAmountBooster(3,1);
          //  ManagerFirebase.Instance?.LogEarnResource(ResourceType.booster, Utills.GetBoosterNameByIndex(3), "1", Reson.reward);
            this.gameObject.SetActive(false);
            boosterUI.UseSpecialSkill();
           
            
            
        }
    }
    
}