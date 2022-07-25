using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Blind
{
	public class UI_Scene : UI_Base
	{
		public override void Init()
		{
			UIManager.Instance.SetCanvas(gameObject, false);
		}
	}
}

