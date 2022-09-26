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


	/// <summary>
	/// ͨ�����߼��õ�ĳ�����ϵĵ�
	/// </summary>
	/// <param name="from"></param>
	/// <param name="depth">���߳���</param>
	/// <param name="layer"></param>
    public static bool RaycastHit(Vector3 from, out RaycastHit hit ,float depth = 1000f,string layer= Layer.Ground)
    {
		Ray ray = Camera.main.ScreenPointToRay(from);//�����λ�÷�������

		bool isCollided = Physics.Raycast(ray, out hit, depth, 1 << LayerMask.NameToLayer(Layer.Ground)); //6=>0000 0000 0000 0000 0000 0000 0010 0000(�����64)
		if (isCollided)
		{
			return true;
		}
		else 
		{
			return false;
		}
    }



	public static Vector3 WorldPoint_XZ_Cenetr(Vector3 from, Vector3 to, float y=1.0f)
	{
		float x = (from.x + to.x) / 2.0f;
		float z = (from.z + to.z) / 2.0f;
		Vector3 center = new Vector3(x, y, z);

		return center;
	}


	public static Vector3 WorldPoint_XZ_halfExtents(Vector3 from, Vector3 to, float y = 1.0f)
	{
		float x = Mathf.Abs( ( to.x - from.x ) )/ 2.0f;
		float z = Mathf.Abs( ( to.z - from.z ) )/ 2.0f;
		Vector3 halfExtents = new Vector3(x, y, z);

		return halfExtents;
	}

	public static Collider[] GetColliders(Vector3 center,Vector3 half)
	{
		return Physics.OverlapBox(center,half);
	}

	/// <summary>
	/// �����泯�� �õ��ҳ��� ��תy�� 90��
	/// </summary>
	/// <param name="forward"></param>
	/// <returns></returns>
	public static Vector3 Vector3_Forward2Right(Vector3 forward)
	{
		return Quaternion.Euler(0, 90, 0) * forward;
	}
}
