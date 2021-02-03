using UnityEngine;

public class Weaponless : BaseAnimController
{
	public string _lookInStumik, _walk;

	protected override void BaseButtons ()
	{
		base.BaseButtons ();

		if (GUI.Button (new Rect (10, 140, 100, 50), "LookInStumik")) {
			_anim.SetTrigger (_lookInStumik);
		}

		if (GUI.Button (new Rect (10, 200, 100, 50), "Walk")) {
			_anim.SetTrigger (_walk);
		}
	}

}
