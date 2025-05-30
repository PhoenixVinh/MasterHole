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
    }
}