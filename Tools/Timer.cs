using System;

public class Timer
{
    public float RemainingSeconds { get; private set; }
	private bool isStarted;

	public Timer(float duration) {
		SetTimer(duration);
		isStarted = false;
	}

	public void SetTimer(float duration) {
		RemainingSeconds = duration;
	}

	public void Start() {
		isStarted = true;
	}

	public event Action OnTimerEnd;

	public void Tick(float deltaTime) {
		if (RemainingSeconds == 0f) { return; }
		if (isStarted) {
			RemainingSeconds -= deltaTime;

			CheckForTimerEnd();
		}
	}

	private void CheckForTimerEnd() {
		if (RemainingSeconds > 0f) { return; }
		
		RemainingSeconds = 0f;
		isStarted = false;

		OnTimerEnd?.Invoke();
	}
}
