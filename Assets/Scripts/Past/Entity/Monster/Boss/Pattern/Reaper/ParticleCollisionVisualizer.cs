using UnityEngine;

[ExecuteAlways]
public class ParticleCollisionVisualizer : MonoBehaviour
{
    public ParticleSystem ps;
    public float radiusScale = 1f;

    private void OnDrawGizmos()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        if (ps == null) return;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];
        int count = ps.GetParticles(particles);

        Gizmos.color = Color.red;
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawWireSphere(particles[i].position, particles[i].GetCurrentSize(ps) * 0.5f * radiusScale);
        }
    }
}