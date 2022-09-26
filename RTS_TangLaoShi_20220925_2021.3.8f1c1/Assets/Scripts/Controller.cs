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
    public List<SoldierObj> soldierObjLst=new List<SoldierObj>();

    private RaycastHit hit;
    private float hitDepth = 1000f;

    Vector3 lastMouseClick=new Vector3();
    float unitOffset = 5; //单位间隔
    private int maxSoliders=12;
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
        Update_ControlSoldiersMove();
    }



    #region 辅助
    /// <summary>
    /// 框选与光标
    /// </summary>
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
                    if (soliderObj != null && soldierObjLst.Count < maxSoliders)//Scene也有碰撞器
                    {
                        soldierObjLst.Add(soliderObj);
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
        foreach (var item in soldierObjLst)
        {
            item.SetSelSelf(false);
        }
        soldierObjLst.Clear();
    }


    /// <summary>
    /// 士兵移动
    /// </summary>
    private void Update_ControlSoldiersMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
           
            if (soldierObjLst.Count == 0) //没有士兵不移动
                return;

            
            if ( Common.RaycastHit(Input.mousePosition, out hit, hitDepth, Layer.Ground) )//获取目标点 通过射线检测
            {
                List<Vector3> targetsPos = GetTargetPos(hit.point);      //通过目标点 计算出 真正的 阵型目标点

                for (int i = 0; i < soldierObjLst.Count; i++)   //命令士兵朝向各自的目标点 移动
                { 
                    soldierObjLst[i].Move(targetsPos[i]);
                }
                   
            }
        }
    }

    /// <summary>
    /// 根据鼠标点击的目标点 计算出 阵型的其它点位
    /// </summary>
    /// <param name="tarPos"></param>
    /// <returns></returns>
    private List<Vector3> GetTargetPos(Vector3 tarPos)
    {
        //需要计算目标点 的 面朝向和 右朝向
        Vector3 nowForward = Vector3.zero;
        Vector3 nowRigth = Vector3.zero;


        if (lastMouseClick != Vector3.zero)//是一批士兵 上一次已经移动过一次了 有上一次的位置
        {
            nowForward = (tarPos - lastMouseClick).normalized;//有上一次的点 就直接计算
        }
        else//没有上一次的点 就用第一个士兵的位置 作为上一次的点来计算
        { 
             nowForward = (tarPos - soldierObjLst[0].transform.position).normalized;
        }
        nowRigth = Common.Vector3_Forward2Right(nowForward);
        Vector3 right = nowRigth * unitOffset;
        Vector3 forward = nowForward * unitOffset;

        List<Vector3> tarPosLst = new List<Vector3>();
        switch (soldierObjLst.Count)
        {
            case 1:
                tarPosLst.Add(tarPos);
                break;
            case 2:
                tarPosLst.Add(tarPos + right / 2);
                tarPosLst.Add(tarPos - right / 2);
                break;
            case 3:
                tarPosLst.Add(tarPos);
                tarPosLst.Add(tarPos + right);
                tarPosLst.Add(tarPos - right);
                break;
            case 4:
                tarPosLst.Add(tarPos + forward / 2 - right / 2);
                tarPosLst.Add(tarPos + forward / 2 + right / 2);
                tarPosLst.Add(tarPos - forward / 2 - right / 2);
                tarPosLst.Add(tarPos - forward / 2 + right / 2);
                break;
            case 5:
                tarPosLst.Add(tarPos + forward / 2);
                tarPosLst.Add(tarPos + forward / 2 - right);
                tarPosLst.Add(tarPos + forward / 2 + right);
                tarPosLst.Add(tarPos - forward / 2 - right);
                tarPosLst.Add(tarPos - forward / 2 + right);
                break;
            case 6:
                tarPosLst.Add(tarPos + forward / 2);
                tarPosLst.Add(tarPos + forward / 2 - right);
                tarPosLst.Add(tarPos + forward / 2 + right);
                tarPosLst.Add(tarPos - forward / 2 - right);
                tarPosLst.Add(tarPos - forward / 2 + right);
                tarPosLst.Add(tarPos - forward / 2);
                break;
            case 7:
                tarPosLst.Add(tarPos + forward);
                tarPosLst.Add(tarPos + forward - right);
                tarPosLst.Add(tarPos + forward + right);
                tarPosLst.Add(tarPos - right);
                tarPosLst.Add(tarPos + right);
                tarPosLst.Add(tarPos);
                tarPosLst.Add(tarPos - forward);
                break;
            case 8:
                tarPosLst.Add(tarPos + forward);
                tarPosLst.Add(tarPos + forward - right);
                tarPosLst.Add(tarPos + forward + right);
                tarPosLst.Add(tarPos - right);
                tarPosLst.Add(tarPos + right);
                tarPosLst.Add(tarPos);
                tarPosLst.Add(tarPos - forward - right);
                tarPosLst.Add(tarPos - forward + right);
                break;
            case 9:
                tarPosLst.Add(tarPos + forward);
                tarPosLst.Add(tarPos + forward - right);
                tarPosLst.Add(tarPos + forward + right);
                tarPosLst.Add(tarPos - right);
                tarPosLst.Add(tarPos + right);
                tarPosLst.Add(tarPos);
                tarPosLst.Add(tarPos - forward - right);
                tarPosLst.Add(tarPos - forward + right);
                tarPosLst.Add(tarPos - forward);
                break;
            case 10:
                tarPosLst.Add(tarPos + forward - right / 2);
                tarPosLst.Add(tarPos + forward + right / 2);
                tarPosLst.Add(tarPos + forward - right * 1.5f);
                tarPosLst.Add(tarPos + forward + right * 1.5f);
                tarPosLst.Add(tarPos - right * 1.5f);
                tarPosLst.Add(tarPos + right * 1.5f);
                tarPosLst.Add(tarPos - right / 2);
                tarPosLst.Add(tarPos + right / 2);
                tarPosLst.Add(tarPos - forward - right * 1.5f);
                tarPosLst.Add(tarPos - forward + right * 1.5f);
                break;
            case 11:
                tarPosLst.Add(tarPos + forward - right / 2);
                tarPosLst.Add(tarPos + forward + right / 2);
                tarPosLst.Add(tarPos + forward - right * 1.5f);
                tarPosLst.Add(tarPos + forward + right * 1.5f);
                tarPosLst.Add(tarPos - right * 1.5f);
                tarPosLst.Add(tarPos + right * 1.5f);
                tarPosLst.Add(tarPos - right / 2);
                tarPosLst.Add(tarPos + right / 2);
                tarPosLst.Add(tarPos - forward - right * 1.5f);
                tarPosLst.Add(tarPos - forward + right * 1.5f);
                tarPosLst.Add(tarPos - forward);
                break;
            case 12:
                tarPosLst.Add(tarPos + forward - right / 2);
                tarPosLst.Add(tarPos + forward + right / 2);
                tarPosLst.Add(tarPos + forward - right * 1.5f);
                tarPosLst.Add(tarPos + forward + right * 1.5f);
                tarPosLst.Add(tarPos - right * 1.5f);
                tarPosLst.Add(tarPos + right * 1.5f);
                tarPosLst.Add(tarPos - right / 2);
                tarPosLst.Add(tarPos + right / 2);
                tarPosLst.Add(tarPos - forward - right * 1.5f);
                tarPosLst.Add(tarPos - forward + right * 1.5f);
                tarPosLst.Add(tarPos - forward - right / 2);
                tarPosLst.Add(tarPos - forward + right / 2);
                break;
        }

        //计算完毕后  记录当前次的位置 
        lastMouseClick = tarPos;

        return tarPosLst;
    }



    #endregion  

    #endregion

}
