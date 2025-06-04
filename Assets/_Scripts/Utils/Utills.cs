using System;
using System.Threading.Tasks;

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
    }
}