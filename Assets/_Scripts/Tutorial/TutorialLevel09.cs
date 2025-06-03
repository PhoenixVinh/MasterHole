using _Scripts.Booster;

namespace _Scripts.Tutorial
{
    public class TutorialLevel09:TutorialLevel03
    {
        public override void FreeBooster()
        {
            ManagerBooster.Instance.ChangeAmountBooster(2,1);
            this.gameObject.SetActive(false);
            boosterUI.UseSpecialSkill();
        }
    }
    
}