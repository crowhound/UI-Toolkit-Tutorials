using UnityEngine;

namespace SF.Damageable
{
    public class HealthBase : MonoBehaviour
    {
        public int CurrentHealth = 10;
        public int MaxHealth = 10;

        private void Awake()
        {
            if (MaxHealth < 0)
                MaxHealth = 10;

            CurrentHealth = MaxHealth;
        }

        /// <summary>
        /// Increases the <see cref="CurrentHealth"/> value and prevents it from going over the max health value.
        /// </summary>
        /// <param name="amount"></param>
        public void RestoreHealth(int amount)
        {
            // Yeah you could do this with an if statement, but wanted to show one way to clamp healing values.
            // Little extra showcase of API.
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        }

        /// <summary>
        /// Decreases the <see cref="CurrentHealth"/> value and prevents it from going under a value of zero.
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(int amount)
        {
            // Prevents the current health from going below 0.
            CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        }
    }
}
