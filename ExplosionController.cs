using UnityEngine;

namespace SpaceShooter
{
    public class ExplosionController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private SpaceShip m_Prefab;
        private GameObject explosion;

        private void Start()
        {
            m_Prefab.EventOnDeath.AddListener(OnExplode);
        }

        private void OnExplode()
        {
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
        }
    }
}