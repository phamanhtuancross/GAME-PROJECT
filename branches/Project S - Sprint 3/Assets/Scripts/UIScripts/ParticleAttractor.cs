using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour {

    private Transform attractor;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();

        attractor = null;
        List<Target> targets = MissionManager.Instance.CurrentMission.targets;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].TargetType == TargetType.Scored)
                attractor = UIManager.Instance.MissionIngame[i].Icon.transform;
        }
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (attractor == null)
            return;

        if (ps.isPlaying)
        {
            int length = ps.GetParticles(particles);
            Vector3 attractorPosition = attractor.position;

            for (int i = 0; i < length; i++)
            {
                //particles[i].position = Vector3.MoveTowards(particles[i].position, attractor.position, 2 * Time.deltaTime);
                particles[i].position = particles[i].position + (attractorPosition - particles[i].position) / (particles[i].lifetime) * Time.deltaTime;
            }
            ps.SetParticles(particles, length);
        }
    }
}
