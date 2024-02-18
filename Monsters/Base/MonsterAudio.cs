using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudio : MonoBehaviour
{
	private AudioSource audioSource;
	[SerializeField] private AudioClip[] footsteps;
	[SerializeField] private AudioClip landEatingSound;
	[SerializeField] private AudioClip waterSplashSound;
	[SerializeField] [Range(0.5f, 3)] private float minPitchScale;
	[SerializeField] [Range(0.5f, 3)] private float maxPitchScale;
	[SerializeField] [Range(0, 1)] private float minVolumeScale;
	[SerializeField] [Range(0, 1)] private float maxVolumeScale;
	[SerializeField] [Range(0, 30)] private float minMinDistanceScale;
	[SerializeField] [Range(0, 30)] private float maxMinDistanceScale;
	private SoundManager sm;
	private Monster monster;

	private void Start() {
		audioSource = GetComponent<AudioSource>();
		monster = GetComponentInParent<Monster>();

		if (monster == null) {
			Debug.LogError("MonsterAudio couldn't find Monster in parent.");
		}

		sm = SoundManager.Instance;
	}

	public void PlayFootstep() {
		if (!monster.movement.IsInWater) {
			int index = Random.Range(0, footsteps.Length);
			AudioClip clip = footsteps[index];
			audioSource.PlayOneShot(clip);
		}
	}

	public void PlayEatSound() {
		if (!monster.movement.IsInWater) {
			audioSource.PlayOneShot(landEatingSound);
		} else {

		}
	}

	public void PlayWaterSplashSound() {
		audioSource.PlayOneShot(waterSplashSound);
	}

	public void SetScaleInfluence(float scale) {
		audioSource.pitch = Mathf.Lerp(minPitchScale, maxPitchScale, scale);

		// 1 - scale cuz higher = higher pitched
		audioSource.volume = Mathf.Lerp(minVolumeScale, maxVolumeScale, scale);

		audioSource.minDistance = Mathf.Lerp(minMinDistanceScale, maxMinDistanceScale, scale);
	}
}
