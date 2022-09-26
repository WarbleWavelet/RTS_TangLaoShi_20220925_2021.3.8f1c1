/****************************************************

	文件：
	作者：WWS
	日期：2022/09/25 21:21:23
	功能：总Ctrl

*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Controller : MonoBehaviour
{
   
    private bool isMouseDown = false;   //鼠标左键是否按下
    private UnityEngine.LineRenderer line;          //获取LineRenderer组件 用于绘制线段
    private Vector3 screenPoint_LeftUp;        //方框左上点,先有这两个
    private Vector3 screenPoint_RightDown;     //右下
    private Vector3 screenPoint_RightUp;       //右上
    private Vector3 screenPoint_LeftDown;      //左下
    private float forceDepth;                   //相当于Camera往前推的深度

    private Vector3   worldPoint_LeftUp;
    private Vector3   worldPoint_RightDown;
    public List<SoldierObj> soliderObjLst=new List<SoldierObj>();

    private RaycastHit hit;
    private float hitDepth = 1000f;

    #region 生命


    // Start is called before the first frame update
    void Start()
    {
        hitDepth = 10000f;
        line = this.GetComponent<UnityEngine.LineRenderer>();
        forceDepth = 5;

    }

    // Update is called once per frame
    void Update()
    {
        Update_SetSolider();
    }



    #region 辅助

    private void Update_SetSolider()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            screenPoint_LeftUp = Input.mousePosition; //记录鼠标当前位置

            if (Common.RaycastHit(Input.mousePosition, out hit, hitDepth, Layer.Ground))
            {
                worldPoint_LeftUp = hit.point;
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            line.positionCount = 0;    //将线段的点设置为0个 就不会去绘制

            ResetSoliderObjLst();

            if (Common.RaycastHit(Input.mousePosition, out hit, hitDepth, Layer.Ground))
            {
                worldPoint_RightDown = hit.point;
                Vector3 centerPos = Common.WorldPoint_XZ_Cenetr(worldPoint_LeftUp, worldPoint_RightDown);
                Vector3 halfExtents = Common.WorldPoint_XZ_halfExtents(worldPoint_LeftUp, worldPoint_RightDown);
                Collider[] colliders = Common.GetColliders(centerPos, halfExtents);

                foreach (var item in colliders)
                {
                    SoldierObj soliderObj = item.GetComponent<SoldierObj>();
                    if (soliderObj != null)//Scene也有碰撞器
                    {
                        soliderObjLst.Add(soliderObj);
                        soliderObj.SetSelSelf(true);
                    }

                }
            }
        }



        if (isMouseDown)    //当鼠标左键处于按下状态时，就在这里面去处理 线段绘制的逻辑
        {
            //注意：目前我们获取的位置 是屏幕坐标系的位置。设置屏幕上的4个点
            screenPoint_LeftUp.z = forceDepth;
            screenPoint_RightDown = new Vector3(Input.mousePosition.x, Input.mousePosition.y, forceDepth);
            screenPoint_RightUp = new Vector3(screenPoint_RightDown.x, screenPoint_LeftUp.y, forceDepth);
            screenPoint_LeftDown = new Vector3(screenPoint_LeftUp.x, screenPoint_RightDown.y, forceDepth);

            //设置线段画线的世界坐标系的点
            line.positionCount = 4;
            line.SetPosition(0, Common.Screen2World(screenPoint_LeftUp));
            line.SetPosition(1, Common.Screen2World(screenPoint_RightUp));
            line.SetPosition(2, Common.Screen2World(screenPoint_RightDown));
            line.SetPosition(3, Common.Screen2World(screenPoint_LeftDown));

        }
    }



    private void ResetSoliderObjLst()
    {
        foreach (var item in soliderObjLst)
        {
            item.SetSelSelf(false);
        }
        soliderObjLst.Clear();
    }
    #endregion  

    #endregion

}
