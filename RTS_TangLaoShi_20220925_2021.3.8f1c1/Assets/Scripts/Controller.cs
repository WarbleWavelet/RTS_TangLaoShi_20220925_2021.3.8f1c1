/****************************************************

	文件：
	作者：WWS
	日期：2022/09/25 21:21:23
	功能：总Ctrl

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Controller : MonoBehaviour
{
   
    private bool isMouseDown = false;   //鼠标左键是否按下
    private UnityEngine.LineRenderer line;          //获取LineRenderer组件 用于绘制线段
    private Vector3 leftUpPoint;        //方框左上点
    private Vector3 rightUpPoint;       //右上
    private Vector3 leftDownPoint;      //左下
    private Vector3 rightDownPoint;     //右下
    private float forceDepth;           //相当于Camera往前推的深度



    #region 生命


   // Start is called before the first frame update
    void Start()
    {
        line = this.GetComponent<UnityEngine.LineRenderer>();
        forceDepth = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
           
            leftUpPoint = Input.mousePosition; //记录鼠标当前位置
            isMouseDown = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            line.positionCount = 0;    //将线段的点设置为0个 就不会去绘制
        }

    
       
        if( isMouseDown)    //当鼠标左键处于按下状态时，就在这里面去处理 线段绘制的逻辑
        {
            //注意：目前我们获取的位置 是屏幕坐标系的位置。设置屏幕上的4个点
            leftUpPoint.z = forceDepth;
            rightDownPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, forceDepth);
            rightUpPoint = new Vector3(rightDownPoint.x, leftUpPoint.y, forceDepth);
            leftDownPoint = new Vector3(leftUpPoint.x,rightDownPoint.y, forceDepth);


            //设置线段画线的世界坐标系的点
            line.positionCount = 4;
            line.SetPosition( 0, Common.Screen2World(leftUpPoint)   );
            line.SetPosition( 1, Common.Screen2World(rightUpPoint)  );
            line.SetPosition( 2, Common.Screen2World(rightDownPoint));
            line.SetPosition( 3, Common.Screen2World(leftDownPoint) );
        }
    }
    #endregion
 
}
