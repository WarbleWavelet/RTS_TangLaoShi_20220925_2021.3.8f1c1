/****************************************************

	文件：
	作者：WWS
	日期：2022/09/25 21:41:04
	功能：公共类

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Common 
{
	public static Vector3 Screen2World(Vector3 pos)
	{
		return Camera.main.ScreenToWorldPoint(pos);
	}
}
