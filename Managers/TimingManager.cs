using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
	public static TimingManager Instance { get; private set; }

	public event Action OnTick;
	public event Action OnMinuteChanged;
	public event Action OnHourChanged;

	private float tickTimer;
	[SerializeField] private float TickInterval;

	// Accumulated Time used for precise in-game time tracking
	[SerializeField] private float minuteToRealTime = 0.5f;
	private float timingTimer;


	public static int Minute { get; private set; }
	public static int Hour { get; private set; }


	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		} else {
			Instance = this;
		}
		tickTimer = TickInterval;
	}

	private void Start() {
		Minute = 0;
		Hour = 10;
		timingTimer = minuteToRealTime;
	}

	private void Update() {
		HandleTick();
		HandleTiming();

	}

	private void HandleTick() {
		if (tickTimer <= 0) {
			tickTimer = TickInterval;
			OnTick?.Invoke();
		} else {
			tickTimer -= Time.deltaTime;
		}
	}

	private void HandleTiming() {
		timingTimer -= Time.deltaTime;

		while (timingTimer <= 0) {
			Minute++;
			OnMinuteChanged?.Invoke();
			if (Minute >= 60) {
				Hour++;
				Minute = 0;
				OnHourChanged?.Invoke();
			}

			timingTimer += minuteToRealTime;
		}
	}

}
