using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum mEffecttype // unity 요소에 순서대로 타입 적용
{
	Asteroid,
	Enmey,
	Player
}

public class EffectPool : OBJPool<Timer>
{

}
