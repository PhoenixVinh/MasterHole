
public class HoleEvent
{
    public delegate void LevelUpHole();
    public static LevelUpHole OnLevelUp;
    
    
    
    public delegate void StartIncreaseSpecialSkill(float timeSkill);
    public static StartIncreaseSpecialSkill OnStartIncreaseSpecialSkill;




    public delegate void HoleSkinEvent();

    public static HoleSkinEvent OnSkinSelected;



    public delegate void TurnOffSkillUI();
    public static TurnOffSkillUI OnTurnOffSkillUI;


    public delegate void UpdateFade(float scale);
    public static UpdateFade OnUpdateFade;
}
