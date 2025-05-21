
public class HoleEvent
{
    public delegate void LevelUpHole();
    public static LevelUpHole OnLevelUp;
    
    
    
    public delegate void StartIncreaseSpecialSkill(float timeSkill);
    public static StartIncreaseSpecialSkill OnStartIncreaseSpecialSkill;




    public delegate void HoleSkiEvent();

    public static HoleSkiEvent OnSkinSelected;



    public delegate void TurnOffSkillUI();
    public static TurnOffSkillUI OnTurnOffSkillUI;
}
