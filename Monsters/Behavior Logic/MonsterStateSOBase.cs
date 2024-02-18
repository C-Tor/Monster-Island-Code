using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateSOBase : ScriptableObject
{
	protected Monster monster;
	protected MonsterBehaviorController monsterBehavior;
	protected Transform transform;
	protected GameObject gameObject;

	public virtual void Initialize(GameObject gameObject, Monster monster) { }

	public virtual void DoEnterLogic() { }
	public virtual void DoExitLogic() { ResetValues(); }
	public virtual void DoFrameUpdateLogic() { }
	public virtual void DoTickUpdateLogic() { }
	public virtual void DoFrequentUpdateLogic() { }
	public virtual void ResetValues() { }
}
