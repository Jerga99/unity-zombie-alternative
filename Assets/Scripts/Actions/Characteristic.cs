using UnityEngine;

namespace Eincode.ZombieSurvival.Actions
{
    public class Characteristic : MonoBehaviour
    {
        public int MaxHealth;
        public int Damage;
        public float AttackSpeed;
        public int Level;

        public int Health { get; set; }

        public void InitHealth()
        {
            Health = MaxHealth;
        }

        private void Start()
        {
            InitHealth();
        }

        public void DecreaseHealth(int by)
        {
            if (Health > 0)
            {
                Health -= by;
            }
        }
    }

}