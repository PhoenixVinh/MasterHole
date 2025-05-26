using _Scripts.Booster;

namespace _Scripts.Tutorial
{
    public class TutorialLevel05 : TutorialLevel03
    {
        public override void FreeBooster()
        {
            ManagerBooster.Instance.ChangeAmountBooster(1,1);
            this.gameObject.SetActive(false);
            boosterUI.UseSpecialSkill();
        }
    }
}