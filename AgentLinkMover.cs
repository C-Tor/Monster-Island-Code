using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System;

public enum OffMeshLinkMoveMethod {
	Teleport,
	NormalSpeed,
	Parabola,
	Curve
}

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour {
	public OffMeshLinkMoveMethod m_Method = OffMeshLinkMoveMethod.Parabola;
	public AnimationCurve curve = new AnimationCurve();

	[SerializeField] private float parabolaJumpTime;
	[SerializeField] private float parabolaJumpHeight;
	[SerializeField] private float curveJumpTime;

	public event Action OnJumpInWater;
	public event Action OnJumpOutWater;

	IEnumerator Start() {
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		agent.autoTraverseOffMeshLink = false;
		while (true) {
			if (agent.isOnOffMeshLink) {
				if (m_Method == OffMeshLinkMoveMethod.NormalSpeed)
					yield return StartCoroutine(NormalSpeed(agent));
				else if (m_Method == OffMeshLinkMoveMethod.Parabola)
					yield return StartCoroutine(Parabola(agent, parabolaJumpHeight, parabolaJumpTime));
				else if (m_Method == OffMeshLinkMoveMethod.Curve)
					yield return StartCoroutine(Curve(agent, curveJumpTime));
				agent.CompleteOffMeshLink();
			}
			yield return null;
		}
	}

	IEnumerator NormalSpeed(NavMeshAgent agent) {
		OffMeshLinkData data = agent.currentOffMeshLinkData;
		Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
		while (agent.transform.position != endPos) {
			agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator Parabola(NavMeshAgent agent, float height, float duration) {
		OffMeshLinkData data = agent.currentOffMeshLinkData;
		Vector3 startPos = agent.transform.position;
		Vector3 endPos = data.endPos;// + Vector3.up * agent.baseOffset;
		float normalizedTime = 0.0f;
		
		//Jumping
		if (startPos.y > endPos.y) {
			//Jumping down
			OnJumpInWater?.Invoke();
		} else {
			//Jumping Up
			OnJumpOutWater?.Invoke();
		}

		while (normalizedTime < 1.0f) {
			float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
			agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
			normalizedTime += Time.deltaTime / duration;
			yield return null;
		}
		GetComponent<MonsterMovement>().StopAgent();
	}

	IEnumerator Curve(NavMeshAgent agent, float duration) {
		OffMeshLinkData data = agent.currentOffMeshLinkData;
		Vector3 startPos = agent.transform.position;
		Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
		float normalizedTime = 0.0f;
		while (normalizedTime < 1.0f) {
			float yOffset = curve.Evaluate(normalizedTime);
			agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
			normalizedTime += Time.deltaTime / duration;
			yield return null;
		}
	}
}