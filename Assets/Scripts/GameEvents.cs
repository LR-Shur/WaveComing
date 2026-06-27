using System;

public static class GameEvents
    {
        public static  System.Action<float, float> OnPlayerHealthChanger;
        public static  System.Action GameOver;
        
        
        /// <summary>
        /// 当敌人死亡时，广播获得的经验值
        /// </summary>
        public static event Action<int> OnEnemyKilled;

        public static void EnemyKilled(int xp)
        {
            OnEnemyKilled?.Invoke(xp);
        }
    }
    
    

