using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// Уничтожаемый объект на сцене. То, что может иметь хитпоинты.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// Объект игнорирует повреждение.
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Стартовое количество хитпоинтов.
        /// </summary>
        [SerializeField] private int m_HitPoints;

        public int HitPoints => m_HitPoints;

        /// <summary>
        /// Текущие хитпоинты.
        /// </summary>
        private int m_CurrentHitPoints;
        public int HP => m_CurrentHitPoints;
        #endregion

        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
        }

        #endregion

        #region Public API
        /// <summary>
        /// Применение дамага к объекту.
        /// </summary>
        /// <param name="damage"> Урон наносимый объекту </param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
                OnDeath();
        }
        #endregion
        [SerializeField] private GameObject m_ExplosionPrefab;
        /// <summary>
        /// Предопределяемое событие уничтожения объекта, когда хитпоинты ниже нуля.
        /// </summary>
        protected virtual void OnDeath()
        {
            if (m_ExplosionPrefab != null)
            {
                Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        #region Score
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
        #endregion
    }
}