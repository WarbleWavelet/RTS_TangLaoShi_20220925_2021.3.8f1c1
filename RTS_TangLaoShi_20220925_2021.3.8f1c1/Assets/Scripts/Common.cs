/****************************************************

	�ļ���
	���ߣ�WWS
	���ڣ�2022/09/25 21:41:04
	���ܣ�������

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
