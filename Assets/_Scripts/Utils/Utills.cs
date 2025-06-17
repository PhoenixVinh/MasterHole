using System;
using System.Threading.Tasks;
using _Scripts.Firebase;

namespace _Scripts.UI
{
    public static class Utills
    {
        public static async Task DelayUntil(Func<bool> condition)
        {
            
            while (!condition())
            {
               
                await Task.Yield();
            }
        }
        
        public static DateTime StringToDate(string datetime)
        {
            if (String.IsNullOrEmpty(datetime))
            {
                return DateTime.Now;
            }
            else
            {
                return DateTime.Parse(datetime);
            }
        }

        public static string GetBoosterNameByIndex(int index)
        {
            string resourceName = Enum.GetName(typeof(ResourceName), index + 2);
            return resourceName;
        }


        public static void ChangePositionBoosterFirebase(int index)
        {
            switch (index)
            {
                case 0:
                 //   ManagerFirebase.Instance.positionPopup = PositionFirebase.scale_popup_ingame;
                    break;
                case 1:
                 //   ManagerFirebase.Instance.positionPopup = PositionFirebase.magnet_popup_ingame;
                    break;
                case 2:
                //    ManagerFirebase.Instance.positionPopup = PositionFirebase.location_popup_ingame;
                    break;
                case 3:
                  //  ManagerFirebase.Instance.positionPopup = PositionFirebase.ice_popup_ingame;
                    break;
            }
        }

        public static long GetMinusTime(DateTime timeNeedMinus)
        {
            DateTime first = DateTime.Now; // Hoặc một DateTime khác


            TimeSpan difference = first - timeNeedMinus;

            long milliseconds = (long)difference.TotalMilliseconds;

        
            return milliseconds;
        }
        
        
        public static string NOT_ENOUGH_COIN = "coins is currently not enough";
        public static string NOT_ENOUGH_ENERGY = "energy is currently not enough";
    }
}