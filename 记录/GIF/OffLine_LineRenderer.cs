/****************************************************

	�ļ���
	���ߣ�WWS
	���ڣ�2022/09/25 22:00:27
	���ܣ��˽�������Ҫ���õĲ������Ժ������Ϊ�������ݱ���

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public partial class OffLine_LineRenderer
{
    public bool loop=true;
    public bool useWorldSpace=true;
    public float width_Start=0.1f;
    public float width_End=0.1f;
    public Material material;

    public LineRenderer line;
    // Start is called before the first frame update
    void Init()
    {
        line.startWidth = width_Start;
        line.endWidth= width_End;
        line.loop = loop;
        line.material = material;
        line.useWorldSpace = useWorldSpace;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
