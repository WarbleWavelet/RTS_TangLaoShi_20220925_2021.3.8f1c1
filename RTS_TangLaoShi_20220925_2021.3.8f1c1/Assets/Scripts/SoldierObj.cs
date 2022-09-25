/****************************************************

	文件：
	作者：WWS
	日期：2022/09/25 21:09:34
	功能：士兵单位

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierObj : MonoBehaviour
{

   
    private Animator animator;      //动画的切换
    private NavMeshAgent agent;     //移动方法
    private GameObject footEffect;  //设置是否处于选中状态



    #region 生命


    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        footEffect = this.transform.Find("FootEffect").gameObject;

        SetSelSelf(false);
    }

    // Update is called once per frame
    void Update()
    {
       
        animator.SetBool(Constants_Animator.Key_IsMove, agent.velocity.magnitude > 0);  //根据当前的移动速度 决定动画时 待机 还是移动
    }
    #endregion  


    /// <summary>
    ///  移动方法 传入目标点即可
    /// </summary>
    /// <param name="pos"></param>
    public void Move(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    /// <summary>
    /// 设置自己是否被选中 决定光圈是否显示
    /// </summary>
    /// <param name="isSel"></param>
    public void SetSelSelf(bool isSel)
    {
        footEffect.SetActive(isSel);
    }
}
