using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Buff { 

	public BuffID buffID;
	public float value;
	public float duration;


	public void AddBuff(BuffID buffID, float value, float duration) {
		this.buffID = buffID;
		this.value = value;
		this.duration = duration;
	}
}
