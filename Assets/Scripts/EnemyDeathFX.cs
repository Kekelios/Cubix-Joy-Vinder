using UnityEngine;

/// <summary>
/// Configure et joue un burst de particules à la mort d'un ennemi,
/// puis se détruit automatiquement via ParticleSystemStopAction.Destroy.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class EnemyDeathFX : MonoBehaviour
{
    private void Awake()
    {
        ConfigureParticleSystem(GetComponent<ParticleSystem>());
    }

    private static void ConfigureParticleSystem(ParticleSystem ps)
    {
        // --- Main ---
        var main = ps.main;
        main.duration          = 0.2f;
        main.loop              = false;
        main.startLifetime     = new ParticleSystem.MinMaxCurve(0.25f, 0.5f);
        main.startSpeed        = new ParticleSystem.MinMaxCurve(2f, 6f);
        main.startSize         = new ParticleSystem.MinMaxCurve(0.05f, 0.18f);
        main.startColor        = new ParticleSystem.MinMaxGradient(
            new Color(1f, 0.85f, 0.1f, 1f),
            new Color(1f, 0.25f, 0f,   1f)
        );
        main.gravityModifier   = 0.4f;
        main.simulationSpace   = ParticleSystemSimulationSpace.World;
        main.stopAction        = ParticleSystemStopAction.Destroy;
        main.maxParticles      = 20;

        // --- Emission : burst unique ---
        var emission = ps.emission;
        emission.enabled      = true;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 10, 15) });

        // --- Shape : sphère serrée ---
        var shape = ps.shape;
        shape.enabled    = true;
        shape.shapeType  = ParticleSystemShapeType.Sphere;
        shape.radius     = 0.08f;

        // --- Color over lifetime : fondu vers transparent ---
        var col = ps.colorOverLifetime;
        col.enabled = true;
        var gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(new Color(1f, 0.9f, 0.2f), 0f),
                new GradientColorKey(new Color(1f, 0.15f, 0f),  1f),
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0f, 1f),
            }
        );
        col.color = new ParticleSystem.MinMaxGradient(gradient);

        // --- Size over lifetime : rétrécissement ---
        var sol = ps.sizeOverLifetime;
        sol.enabled = true;
        sol.size = new ParticleSystem.MinMaxCurve(
            1f,
            new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f))
        );
    }
}
