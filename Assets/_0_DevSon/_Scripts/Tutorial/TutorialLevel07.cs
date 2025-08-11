using _Scripts.Booster;
using _Scripts.Firebase;
using _Scripts.UI;

namespace _Scripts.Tutorial
{
    public class TutorialLevel07 : TutorialLevel03
    {
        public override void FreeBooster()
        {
            ManagerBooster.Instance.ChangeAmountBooster(2,1);
            ManagerFirebase.Instance?.LogEarnResource(ResourceType.booster, Utills.GetBoosterNameByIndex(2), "1", Reson.reward);
            this.gameObject.SetActive(false);
            boosterUI.UseSpecialSkill();
        }
    }
}