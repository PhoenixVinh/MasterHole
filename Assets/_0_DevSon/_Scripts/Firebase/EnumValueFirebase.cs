namespace _Scripts.Firebase
{
    public enum EnumValueFirebase
    {
        UserProperties,
        level_start,
        level_end,
        earn_resources,
        spend_resources,
        iap_show,
        iap_click,
        iap_purchase,
        ad_request,
        ad_show,
        ad_complete,
        screen_reach
    }
    
    
    public enum InternetStatus{
        offline,
        wifi,
        mobile_data, 
        unknown,
    }

    public enum PlayType
    {
        home,
        retry,
        next
    }

    public enum LevelResult
    {
        win,
        lose,
        quit,
        exitgame
    }

    public enum LoseBy
    {
        Null, 
        OutOfTime, 
        OutOfObjDeath
    }

    public enum PositionFirebase
    {
        home, 
        ingame,
        shop_home_byAddCoin,
        popup_ingame_byAddHeart, 
        complete_popup_next, 
        complete_popup_home, 
        lose_popup_playon,
        lose_popup_retry,
        shop_home, 
        skin_home,
        collection_home, 
        scale_popup_ingame,
        magnet_popup_ingame,
        location_popup_ingame,
        ice_popup_ingame,
        quit_popuppause_ingame,
        refill_popup_heart,
        morelift_popup_heart,
        setting_home,
        setting_ingame,
        win_level,
        lose_level,
        none
    }
    
    public enum ResourceType {
        currency,
        booster,
        item,
        pack
    }

    public enum ResourceName
    {
        Coin = 0,
        PlusTime = 1,
        Exp = 2,
        Scale = 3,
        Magnet = 4,
        Location = 5, 
        Ice = 6, 
        Heart = 7,
    }
    
    

    public enum Reson
    {
        reward,
        purchase,
        video,
        exchange,
        winlevel,
        use
    }

    public enum AdFormat
    {
        interstitial,
        video_rewarded,
        banner,
        native
    }

    public enum AdPlatform
    {
        MaxApplovin
    }

    public enum EndType
    {
        quit,
        done
    }

    public enum ShowType
    {
        shop,
        pack
    }
    
    
}