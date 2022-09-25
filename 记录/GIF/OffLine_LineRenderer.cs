/****************************************************

	文件：
	作者：WWS
	日期：2022/09/25 22:00:27
	功能：了解该组件主要设置的参数，以后可以作为离线数据保存

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
