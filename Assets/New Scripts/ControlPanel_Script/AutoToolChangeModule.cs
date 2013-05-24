using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class AutoToolChangeModule : MonoBehaviour {
	Transform Tool_changer;//换刀模块，2013-5-24
	public Transform ToolWheel;//转盘，刘旋，2013-5-24
	public Transform ToolChanger;//机械手，刘旋，2013-5-24
	public Transform Tool_Num;//当前换刀的刀柄，刘旋，2013-5-24
	public Transform Tool_NumD;//当前换刀的刀具，刘旋，2013-5-24
	public Transform Tool_NumB;//当前换刀的刀座，刘旋，2013-5-24
	public List<Transform> Tool_H=new List<Transform>();//20把刀的刀柄，刘旋，2013-5-24
	public List<Transform> Tool_D=new List<Transform>();//20把刀，刘旋，2013-5-24
	public List<Transform> Tool_Bottom=new List<Transform>();//20把刀的刀座，刘旋，2013-5-24
	public List<bool> Tool_F=new List<bool>();//20把刀的状态，false表示没有刀，true表示有刀，刘旋，2013-5-24
	public Transform installedTool_NumD;//已经安装的刀，刘旋，2013-5-24
	public Transform Tool_mainAxis;//换刀的主轴，刘旋，2013-5-24
	public Transform Tool_holder;//刀座，刘旋，2013-5-24
	
	public bool revolution_flag=false;//将已安装的刀放回刀柄时，getToolID（）的输出为ToolID=installedToolID，刘旋，2013-5-24
	public bool installedTool_flag=false;//已安装刀的状态，刘旋，2013-5-24
	public bool Tool_flag=false;//当前换刀的刀的状态，刘旋，2013-5-24
	public bool interrupt_flag=false;//中断复位的状态，刘旋，2013-5-24
	public bool mainAxis_flag=false;//主轴移动的状态，刘旋，2013-5-24
	public int changer_mode=0;//机械手转动的模式，刘旋，2013-5-24
	public int putTool_mode=0;//安装刀具的模式，刘旋，2013-5-24
	public int getTool_mode=0;//取刀的模式，刘旋，2013-5-24
	public int revolution_mode=0;//转盘转动的模式，刘旋，2013-5-24
	public int rotation_mode=0;//刀具自转的模式，刘旋，2013-5-24
	
	public Vector3 Revolution_point;//转盘转动的中心点，刘旋，2013-5-24
	public Vector3 Rotate_point;//刀具自转的中心点，刘旋，2013-5-24
	public float time_value=0;//两次键盘1的时间间隔，只有时间大于time-value是才能输出11，刘旋，2013-5-24
	public float Revolution_angles=0;//转盘转动的角度，刘旋，2013-5-24
	public float Rotation_angles=0;//刀具自转的角度，刘旋，2013-5-24
	public float getTool_position=0;//取刀的位置，刘旋，2013-5-24
	public float putTool_position=0;//放到的位置，刘旋，2013-5-24
	public float mainAxis_position=0;//主轴的位置，刘旋，2013-5-24
	public float reset_getTool_position=0;//中断复位时，获取当前取刀的位置，刘旋，2013-5-24
	public float reset_putTool_position=0;//中断复位时，获取当前装刀的位置，刘旋，2013-5-24
	public float reset_Rotation_angles=0;//中断复位时，获取当前刀具自转的角度，刘旋，2013-5-24
	public float reset_ChangerRotation_angles=0;//中断复位时，获取机械手的角度，刘旋，2013-5-24
	public string tempToolID="";//键盘输入的刀号，刘旋，2013-5-24
	public int ToolID=0;//当前换刀刀号，刘旋，2013-5-24
	public int tempID=0;//存储当前刀号，刘旋，2013-5-24
	public int installedToolID=0;//已安装的刀号，刘旋，2013-5-24
	public float ToolWheel_angles=0;//当前转盘的角度，刘旋，2013-5-24
	public float ToolWheel_diffangles=0;//程序运行前转盘的角度，刘旋，2013-5-24
	public float ToolWheel_revolutionAngles=0;//转盘需要转动的角度，刘旋，2013-5-24
	public float ToolWheel_revolutionDirection=1;//转盘转动的方向，刘旋，2013-5-24
	public float Tool_rotationDirection=1;//刀具自转的方向，刘旋，2013-5-24
	public float Changer_rotationDirection=-1;//机械手转动的方向，刘旋，2013-5-24
	public float getTool_Direction=-1;//取刀的方向，刘旋，2013-5-24
	public float putTool_Direction=1;//放刀的方向，刘旋，2013-5-24
	public float ChangerRotation_angles=0;//机械手转动的角度，刘旋，2013-5-24
	public int Current_status=0;//换刀当前状态，刘旋，2013-5-24
	//public float 
	// Use this for initialization
	void Start () {
		ToolWheel=GameObject.Find("tools box").transform;//添加空物体tools-box，用于转盘，放置所有刀具，刀柄和刀座,刘旋，2013-5-24
		ToolChanger=GameObject.Find("tools box_5").transform;//机械手,刘旋，2013-5-24
		GameObject.Find("tools box_34").transform.parent=ToolChanger;//为机械手添加转轴,刘旋，2013-5-24
		Tool_mainAxis=GameObject.Find("main axle_3").transform;//换刀主轴,刘旋，2013-5-24
		Tool_holder=GameObject.Find("tools box_22").transform;//刀座,刘旋，2013-5-24
		List<Transform> allTransform=new List<Transform>();
		Tool_changer=GameObject.Find("Tool_Changer").transform;
		foreach(Transform child in Tool_changer.transform)
			allTransform.Add(child);
		Tool_H.Add(GameObject.Find("tools box_29").transform);
		Tool_D.Add(GameObject.Find("tools box_29").transform);
		Tool_Bottom.Add(GameObject.Find("tools box_29").transform);
		int count_1=1;
		int count_2=1;
		for(int i=0;i<allTransform.Count;i++)
		{
			if(Tool_H.Count<22)
			{
				if(allTransform[i].name.StartsWith("tools box_30_"))
					Tool_H.Add(allTransform[i]);
			}
			if(count_2<20)
			{
				if(allTransform[i].name.StartsWith("tools box_31_"))
				{
					allTransform[i].parent=Tool_H[count_2];
					allTransform[i+20].parent=Tool_H[count_2];
					count_2++;
				}	
			}
			if(Tool_D.Count<17)
			{
				if(allTransform[i].name.StartsWith("tools box_29_"))
					Tool_D.Add(allTransform[i]);
			}
			if(Tool_Bottom.Count<22)
			{
				if(allTransform[i].name.StartsWith("tools box_33_"))
				{
					Tool_Bottom.Add(allTransform[i]);
					allTransform[i+20].parent=allTransform[i];
					allTransform[i].parent=ToolWheel;
				}
			}
			if(count_1<17)
			{
				if(allTransform[i].name.StartsWith("tools box_2"))
				{
					count_1++;
					if(count_1>2)
						allTransform[i].parent=Tool_holder;
				}
			}
		}
		Tool_D.RemoveAt(1);Tool_D.RemoveAt(1);
		for(int j=1;j<6;j++)
		    Tool_D.Add(GameObject.Find("tools box_29").transform);
		Tool_D.Add(GameObject.Find("tools box_29_1").transform);
		Tool_D.Add(GameObject.Find("tools box_29_2").transform);
		GameObject.Find("9.8 end mill").transform.parent=Tool_D[1];
		GameObject.Find("7.8 end mill").transform.parent=Tool_D[2];
		GameObject.Find("3.8 end mill").transform.parent=Tool_D[3];
		GameObject.Find("1.8 end mill").transform.parent=Tool_D[4];
		GameObject.Find("40 spher mill_1").transform.parent=Tool_D[5];
		GameObject.Find("40 spher mill_2").transform.parent=Tool_D[5];
		GameObject.Find("40 spher mill_3").transform.parent=Tool_D[5];
		GameObject.Find("20 spher mill_1").transform.parent=Tool_D[6];
		GameObject.Find("20 spher mill_2").transform.parent=Tool_D[6];
		GameObject.Find("20 spher mill_3").transform.parent=Tool_D[6];
		GameObject.Find("8 spher mill_1").transform.parent=Tool_D[7];
		GameObject.Find("8 spher mill_2").transform.parent=Tool_D[7];
		GameObject.Find("8 spher mill_3").transform.parent=Tool_D[7];
		GameObject.Find("4 spher mill_1").transform.parent=Tool_D[8];
		GameObject.Find("4 spher mill_2").transform.parent=Tool_D[8];
		GameObject.Find("4 spher mill_3").transform.parent=Tool_D[8];
		GameObject.Find("2 spher mill_1").transform.parent=Tool_D[9];
		GameObject.Find("2 spher mill_2").transform.parent=Tool_D[9];
		GameObject.Find("2 spher mill_3").transform.parent=Tool_D[9];
		GameObject.Find("7.9 spiral drill").transform.parent=Tool_D[10];
		GameObject.Find("3.9 spiral drill").transform.parent=Tool_D[11];
		GameObject.Find("8 reamer").transform.parent=Tool_D[12];
		GameObject.Find("4 reamer").transform.parent=Tool_D[13];
		GameObject.Find("50 face mill_1").transform.parent=Tool_D[19];
		GameObject.Find("50 face mill_2").transform.parent=Tool_D[19];
		GameObject.Find("50 face mill_3").transform.parent=Tool_D[19];
		GameObject.Find("19.8 end mill").transform.parent=Tool_D[20];
		for(int m=1;m<21;m++)
		{
			Tool_D[m].parent=Tool_H[m];
			Tool_H[m].parent=ToolWheel;
		}
		for(int n=0;n<21;n++)
			Tool_F.Add(true);
		for(int k=14;k<19;k++)
			Tool_F[k]=false;
		Revolution_point=GameObject.Find("tools box_9").transform.position;
		Rotate_point=GameObject.Find("tools box_34_19").transform.position;
		ToolWheel_angles=ToolWheel.eulerAngles.z;
		ToolWheel_diffangles=ToolWheel_angles;
	}
	void OnGUI()
	{
		//开始换刀，刘旋，2013-5-24
		if(GUI.Button(new Rect(100f,100f,100f,60f),"Rotate"))
		{
			mainAxis_flag=true;
			revolution_mode=1;
			if(installedTool_flag)
				revolution_mode=2;
			ToolWheel_angles=ToolWheel.eulerAngles.z;
		}
		//显示输入的刀号，刘旋，2013-5-24
		GUI.Button(new Rect(100f,20f,100f,60f),"ID:"+tempToolID);
		//再次输入刀号，刘旋，2013-5-24
		if(GUI.Button(new Rect(210f,20f,100f,60f),"InputAgain"))
		{
			tempToolID="";
		}
		//中断复位，刘旋，2013-5-24
		if(GUI.Button(new Rect(320f,20f,100f,60f),"Interrupt"))
		{
			ToolWheel_angles=ToolWheel.eulerAngles.z;
			interrupt_flag=true;
			Revolution_angles=0;
			reset_Rotation_angles=Rotation_angles;
			Rotation_angles=0;
			reset_getTool_position=getTool_position;
			getTool_position=0;
			reset_ChangerRotation_angles=ChangerRotation_angles;
			ChangerRotation_angles=0;
			reset_putTool_position=putTool_position;
			putTool_position=0;
			if((revolution_mode+rotation_mode+changer_mode+getTool_mode+putTool_mode)==0)
			{
				if(installedTool_flag)
				{
					Current_status=9;
					changer_mode=7;
				}
				else
				{
					Current_status=0;
					revolution_mode=1;
				}
			}
			if(Current_status==20)
			{
				ToolWheel_angles=ToolWheel.eulerAngles.z;
			    ToolID=tempID;
			    selectTool();
			    getToolInfo();
			}
		}
	}
	void Update()
	{
		//主轴移动，刘旋，2013-5-24
		if(mainAxis_flag)
		{
			if(mainAxis_position<0.0375868f)
		    {
			    Tool_mainAxis.Translate(0,-0.00835f*Time.deltaTime,0,Space.Self);
			    mainAxis_position+=0.00835f*Time.deltaTime;
		    }
		}
		//正常情况下换刀，刘旋，2013-5-24
		if(!interrupt_flag)
		{
			getToolID();//获取刀号，刘旋，2013-5-24
		    selectTool();//获取当前刀柄，刀具和刀座，刘旋，2013-5-24
		    getToolInfo();//获取转盘转动的信息，刘旋，2013-5-24
			//主轴上没有安装刀具，刘旋，2013-5-24
			if(!installedTool_flag)
			{
				if(revolution_mode==1)
		        {
			        Revolution();//转盘转动，刘旋，2013-5-24
			        Current_status=1;//为当前换刀状态赋值，共21个，0--20，刘旋，2013-5-24
		        }
		        if(rotation_mode==1)
		        {
			        Rotation(90f);//刀具自转，刘旋，2013-5-24
			        Current_status=2;
		        }
		        if(changer_mode==1)
		        {
			        ChangerRotation(75f);//机械手转动，刘旋，2013-5-24
			        Current_status=3;
		        }
				if(getTool_mode==1)
				{
					Tool_NumD.parent=ToolChanger;
					getTool(0.105f);//取刀，刘旋，2013-5-24
					Current_status=4;
				}
				if(changer_mode==2)
				{
					ChangerRotation(180f);//机械手转动，刘旋，2013-5-24
					Current_status=5;
				}
				if(putTool_mode==1)
				{
					putTool(0.105f);//安装刀，刘旋，2013-5-24
					Current_status=6;
				}
				if(rotation_mode==2)
				{
					Rotation(-90f);//刀柄回位，刘旋，2013-5-24
					Current_status=7;
				}
				if(changer_mode==3)
				{
					Tool_NumD.parent=Tool_mainAxis;
					ChangerRotation(105f);//机械手回位，刘旋，2013-5-24
					Current_status=8;
				}
			}
			//主轴上已安装刀具，刘旋，2013-5-24
			else
			{
				if(revolution_mode==2)
				{
					Revolution();//转盘转动，刘旋，2013-5-24
					Current_status=10;
				}
				if(rotation_mode==3)
				{
					Rotation(90f);//刀具自转，刘旋，2013-5-24
					Current_status=11;
				}
				if(changer_mode==4)
				{
					ChangerRotation(75f);//机械手转动，刘旋，2013-5-24
					Current_status=12;
				}
				if(getTool_mode==2)
				{
					installedTool_NumD.parent=ToolChanger;
					Tool_NumD.parent=ToolChanger;
					getTool(0.105f);//取刀（当前刀具和已安装刀具），刘旋，2013-5-24
					Current_status=13;
				}
				if(changer_mode==5)
				{
					ChangerRotation(180f);//机械手转动，刘旋，2013-5-24
					Current_status=14;
				}
				if(rotation_mode==4)
				{
					Rotation(-90f);//当前刀柄回位，刘旋，2013-5-24
					Current_status=15;
				}
				if(revolution_mode==3)
				{
					Revolution();//转盘转动，将已安装刀具的刀柄转到最下面，刘旋，2013-5-24
					Current_status=16;
				}
				if(rotation_mode==5)
				{
					Rotation(90f);//已安装刀柄的转动，刘旋，2013-5-24
					Current_status=17;
				}
				if(putTool_mode==2)
				{
					putTool(0.105f);//安装刀（当前刀装在主轴上，已安装刀装在相应的刀柄上），刘旋，2013-5-24
					Current_status=18;
				}
				if(rotation_mode==6)
				{
					Tool_NumD.parent=Tool_Num;
					Rotation(-90f);//已安装刀具和刀柄自转，刘旋，2013-5-24
					Current_status=19;
				}
				if(changer_mode==6)
				{
					getToolID();
					selectTool();
					getToolInfo();
					Tool_NumD.parent=Tool_mainAxis;
					ChangerRotation(105f);//机械手回位，刘旋，2013-5-24
					Current_status=20;
				}
			}
		}
		//中断复位，刘旋，2013-5-24
		else
		{
			Debug.Log("currentstatus="+Current_status);
			//根据状态不同进行换刀复位，刘旋，2013-5-24
			switch(Current_status)
			{
			case 0:
				Reset_status0();
				break;
			case 1:
				Reset_status1();
				break;
			case 2:
				Reset_status2();
				break;
			case 3:
				Reset_status3();
				break;
			case 4:
				Reset_status4();
				break;
			case 5:
				Reset_status5();
				break;
			case 6:
				Reset_status6();
				break;
			case 7:
				Reset_status7();
				break;
			case 8:
				Reset_status8();
				break;
			case 9:
				Reset_status9();
				break;
			case 10:
				Reset_status10();
				break;
			case 11:
				Reset_status11();
				break;
			case 12:
				Reset_status12();
				break;
			case 13:
				Reset_status13();
				break;
			case 14:
				Reset_status14();
				break;
			case 15:
				Reset_status15();
				break;
			case 16:
				Reset_status16();
				break;
			case 17:
				Reset_status17();
				break;
			case 18:
				Reset_status18();
				break;
			case 19:
				Reset_status19();
				break;
			case 20:
				Reset_status20();
				break;
			}
		}	
	}
	void Reset_status0()
	{
		Reset_status1();
	}
	void Reset_status1()
	{
		if(revolution_mode==1)
		{
			ToolID=19;
		    selectTool();
		    getToolInfo();
		    Revolution();
		}
	}
	void Reset_status2()
	{
		if(rotation_mode==1)
			Rotation(-reset_Rotation_angles);
		Reset_status1();
	}
	void Reset_status3()
	{
		if(changer_mode==1)
			ChangerRotation(-reset_ChangerRotation_angles);
		if(rotation_mode==1)
			Rotation(-90f);
		Reset_status1();
	}
	void Reset_status4()
	{
		if(getTool_mode==1)
			getTool(-reset_getTool_position);
		if(changer_mode==1)
		{
			Tool_NumD.parent=Tool_Num;
			ChangerRotation(-75f);
		}
		if(rotation_mode==1)
			Rotation(-90f);
		Reset_status1();
	}
	void Reset_status5()
	{
		if(changer_mode==2)
			ChangerRotation(-reset_ChangerRotation_angles);
		if(getTool_mode==1)
			getTool(-0.105f);
		if(changer_mode==1)
		{
			Tool_NumD.parent=Tool_Num;
			ChangerRotation(-75f);
		}
		if(rotation_mode==1)
			Rotation(-90f);
		Reset_status1();
	}
	void Reset_status6()
	{
		if(putTool_mode==1)
			putTool(-reset_putTool_position);
		if(changer_mode==2)
			ChangerRotation(-180f);
		if(getTool_mode==1)
			getTool(-0.105f);
		if(changer_mode==1)
		{
			Tool_NumD.parent=Tool_Num;
			ChangerRotation(-75f);
		}
		if(rotation_mode==1)
			Rotation(-90f);
		Reset_status1();
	}
	void Reset_status7()
	{
		if(rotation_mode==2)
			Rotation(reset_Rotation_angles);
		if(putTool_mode==1)
			putTool(-0.105f);
		if(changer_mode==2)
			ChangerRotation(-180f);
		if(getTool_mode==1)
			getTool(-0.105f);
		if(changer_mode==1)
		{
			Tool_NumD.parent=Tool_Num;
			ChangerRotation(-75f);
		}
		if(rotation_mode==1)
			Rotation(-90f);
		Reset_status1();
	}
	void Reset_status8()
	{
		if(changer_mode==3)
			ChangerRotation(-reset_ChangerRotation_angles);
		if(rotation_mode==2)
			Rotation(reset_Rotation_angles);
		if(putTool_mode==1)
		{
			Tool_NumD.parent=ToolChanger;
			putTool(-0.105f);
		}
		if(changer_mode==2)
			ChangerRotation(-180f);
		if(getTool_mode==1)
			getTool(-0.105f);
		if(changer_mode==1)
		{
			Tool_NumD.parent=Tool_Num;
			ChangerRotation(-75f);
		}
		if(rotation_mode==1)
			Rotation(-90f);
		Reset_status1();
	}
	void Reset_status9()
	{
		if(changer_mode==7)
			ChangerRotation(-105f);
		if(rotation_mode==2)
			Rotation(90f);
		if(putTool_mode==1)
		{
			Tool_NumD.parent=ToolChanger;
			putTool(-0.105f);
		}
		if(changer_mode==2)
			ChangerRotation(-180f);
		if(getTool_mode==1)
			getTool(-0.105f);
		if(changer_mode==1)
		{
			Tool_NumD.parent=Tool_Num;
			ChangerRotation(-75f);
		}
		if(rotation_mode==1)
			Rotation(-90f);
		Reset_status1();
	}
	void Reset_status10()
	{
		if(revolution_mode==2)
		{
			ToolID=installedToolID;
		    selectTool();
		    getToolInfo();
		    Revolution();
		}
		if(rotation_mode==7)
			Rotation(90f);
		if(changer_mode==8)
			ChangerRotation(75f);
		if(getTool_mode==3)
		{
			installedTool_NumD.parent=ToolChanger;
			getTool(0.105f);
		}
		if(changer_mode==9)
			ChangerRotation(180f);
		if(putTool_mode==3)
			putTool(0.105f);
		if(rotation_mode==8)
		{
			installedTool_NumD.parent=Tool_Num;
			Rotation(-90f);
		}
		if(changer_mode==10)
			ChangerRotation(105f);
		Reset_status1();
	}
	void Reset_status11()
	{
		if(rotation_mode==3)
			Rotation(-reset_Rotation_angles);
		Reset_status10();
	}
	void Reset_status12()
	{
		if(changer_mode==4)
			ChangerRotation(75f-reset_ChangerRotation_angles);
		Reset_status();
	}
	void Reset_status13()
	{
		if(getTool_mode==2)
			getTool(-reset_getTool_position);
		Reset_status();
	}
	void Reset_status14()
	{
		if(changer_mode==5)
			ChangerRotation(-reset_ChangerRotation_angles);
		if(getTool_mode==2)
			getTool(-0.105f);
		Reset_status();
	}
	void Reset_status15()
	{
		if(!Tool_flag)
		{
			if(rotation_mode==4)
				Rotation(reset_Rotation_angles-90f);
			if(revolution_mode==5)
			{
				ToolID=installedToolID;
		        selectTool();
		        getToolInfo();
		        Revolution();
			}
			if(rotation_mode==12)
				Rotation(90f);
			if(putTool_mode==5)
				putTool(0.105f);
			if(rotation_mode==13)
			{
				Tool_NumD.parent=Tool_Num;
				Rotation(-90f);
			}
			if(changer_mode==12)
				ChangerRotation(105f);
			Reset_status1();
		}
		else
		{
			if(rotation_mode==4)
				Rotation(reset_Rotation_angles);
			if(changer_mode==13)
				ChangerRotation(180f);
			if(putTool_mode==6)
				putTool(0.105f);
			Reset_status();
		}
	}
	void Reset_status16()
	{
		if(revolution_mode==3)
		{
			ToolID=installedToolID;
			selectTool();
			getToolInfo();
			Revolution();
		}
		if(rotation_mode==14)
			Rotation(90f);
		if(putTool_mode==7)
			putTool(0.105f);
		if(rotation_mode==15)
		{
			Tool_NumD.parent=Tool_Num;
			Rotation(-90f);
		}
		if(!Tool_flag)
		{
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
		else
		{
			if(changer_mode==15)
				ChangerRotation(180f);
			if(getTool_mode==5)
			{
				Tool_NumD.parent=ToolChanger;
				getTool(0.105f);
			}
			if(changer_mode==16)
				ChangerRotation(180f);
			if(revolution_mode==7)
				Revolution();
			if(rotation_mode==17)
				Rotation(90f);
			if(putTool_mode==8)
				putTool(0.105f);
			if(rotation_mode==16)
			{
				Tool_NumD.parent=Tool_Num;
				Rotation(-90f);
			}
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
	}
	void Reset_status17()
	{
		if(rotation_mode==5)
			Rotation(90f-reset_Rotation_angles);
		if(putTool_mode==7)
			putTool(0.105f);
		if(rotation_mode==15)
		{
			Tool_NumD.parent=Tool_Num;
			Rotation(-90f);
		}
		if(!Tool_flag)
		{
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
		else
		{
			if(changer_mode==15)
				ChangerRotation(180f);
			if(getTool_mode==5)
			{
				Tool_NumD.parent=ToolChanger;
				getTool(0.105f);
			}
			if(changer_mode==16)
				ChangerRotation(180f);
			if(revolution_mode==7)
				Revolution();
			if(rotation_mode==17)
				Rotation(90f);
			if(putTool_mode==8)
				putTool(0.105f);
			if(rotation_mode==16)
			{
				Tool_NumD.parent=Tool_Num;
				Rotation(-90f);
			}
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
	}
	void Reset_status18()
	{
		if(putTool_mode==2)
			putTool(0.105f-reset_putTool_position);
		if(rotation_mode==15)
		{
			Tool_NumD.parent=Tool_Num;
			Rotation(-90f);
		}
		if(!Tool_flag)
		{
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
		else
		{
			if(changer_mode==15)
				ChangerRotation(180f);
			if(getTool_mode==5)
			{
				Tool_NumD.parent=ToolChanger;
				getTool(0.105f);
			}
			if(changer_mode==16)
				ChangerRotation(180f);
			if(revolution_mode==7)
				Revolution();
			if(rotation_mode==17)
				Rotation(90f);
			if(putTool_mode==8)
				putTool(0.105f);
			if(rotation_mode==16)
			{
				Tool_NumD.parent=Tool_Num;
				Rotation(-90f);
			}
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
	}
	void Reset_status19()
	{
		if(rotation_mode==6)
		{
			Tool_NumD.parent=Tool_Num;
			Rotation(reset_Rotation_angles-90f);
		}
		if(!Tool_flag)
		{
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
		else
		{
			if(changer_mode==15)
				ChangerRotation(180f);
			if(getTool_mode==5)
			{
				Tool_NumD.parent=ToolChanger;
				getTool(0.105f);
			}
			if(changer_mode==16)
				ChangerRotation(180f);
			if(revolution_mode==7)
				Revolution();
			if(rotation_mode==17)
				Rotation(90f);
			if(putTool_mode==8)
				putTool(0.105f);
			if(rotation_mode==16)
			{
				Tool_NumD.parent=Tool_Num;
				Rotation(-90f);
			}
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
	}
	void Reset_status20()
	{
		if(!Tool_flag)
		{
			if(changer_mode==6)
				ChangerRotation(105f-reset_ChangerRotation_angles);
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
		}
		else
		{
			if(changer_mode==6)
				ChangerRotation(-reset_ChangerRotation_angles);
			if(getTool_mode==5)
			{
				Tool_NumD.parent=ToolChanger;
				getTool(0.105f);
			}
			if(changer_mode==16)
				ChangerRotation(180f);
			if(revolution_mode==7)
				Revolution();
			if(rotation_mode==17)
				Rotation(90f);
			if(putTool_mode==8)
				putTool(0.105f);
			if(rotation_mode==16)
			{
				Tool_NumD.parent=Tool_Num;
				Rotation(-90f);
			}
			if(revolution_mode==6)
			{
				ToolID=19;
				getToolInfo();
				Revolution();
			}
			if(changer_mode==14)
				ChangerRotation(105f);
		}
	}
	void Reset_status()
	{
		if(rotation_mode==9)
			Rotation(-90f);
		if(revolution_mode==4)
		{
			ToolID=installedToolID;
		    selectTool();
		    getToolInfo();
		    Revolution();
		}
		if(rotation_mode==10)
			Rotation(90f);
		if(getTool_mode==4)
		{
			installedTool_NumD.parent=ToolChanger;
			getTool(0.105f);
		}
		if(changer_mode==11)
			ChangerRotation(180f);
		if(putTool_mode==4)
			putTool(0.105f);
		if(rotation_mode==11)
		{
			installedTool_NumD.parent=Tool_Num;
			Rotation(-90f);
		}
		if(changer_mode==12)
			ChangerRotation(105f);
		Reset_status1();
	}
	//获取当前模式信息，刘旋，2013-5-24
	void get_mode()
	{
		if(revolution_mode==1)
		{
			if(interrupt_flag)
				revolution_mode=0;
			else
			    rotation_mode=1;
			
		}
		else if(rotation_mode==1)
		{
			if(interrupt_flag)
				revolution_mode=1;
			else
				changer_mode=1;
		}
		else if(changer_mode==1)
		{
			if(interrupt_flag)
				rotation_mode=1;
			else
			    getTool_mode=1;
		}
		else if(getTool_mode==1)
		{
			if(interrupt_flag)
				changer_mode=1;
			else
			    changer_mode=2;
		}
		else if(changer_mode==2)
		{
			if(interrupt_flag)
				getTool_mode=1;
			else
			    putTool_mode=1;
		}
		else if(putTool_mode==1)
		{
			if(interrupt_flag)
				changer_mode=2;
			else
			    rotation_mode=2;
		}
		else if(rotation_mode==2)
		{
			if(interrupt_flag)
				putTool_mode=1;
			else
			    changer_mode=3;
		}
		else if(changer_mode==3)
		{
		    installedToolID=ToolID;
			installedTool_NumD=Tool_NumD;
			installedTool_flag=Tool_flag;
			if(installedTool_flag)
				Current_status=9;
			else
				Current_status=0;
		}
		else if(changer_mode==7)
		{
			if(interrupt_flag)
				rotation_mode=2;
		}
		else if(revolution_mode==2)
		{
			if(interrupt_flag)
				rotation_mode=7;
			else
			    rotation_mode=3;
		}
		else if(rotation_mode==3)
		{
			if(interrupt_flag)
				revolution_mode=2;
			else
			    changer_mode=4;
		}
		else if(changer_mode==4)
		{
			if(interrupt_flag)
				rotation_mode=9;
			else
			    getTool_mode=2;
		}
		else if(getTool_mode==2)
		{
			if(interrupt_flag)
				rotation_mode=9;
			else
			    changer_mode=5;
		}
		else if(changer_mode==5)
		{
			if(interrupt_flag)
				getTool_mode=2;
			else
			    rotation_mode=4;
		}
		else if(rotation_mode==4)
		{
			if(interrupt_flag)
			{
				if(Tool_flag)
					changer_mode=13;
				else 
					revolution_mode=5;
			}
			else
			{
			    revolution_mode=3;
				ToolWheel_angles=ToolWheel.eulerAngles.z;
				revolution_flag=true;
				tempID=ToolID;
			}
		}
		else if(revolution_mode==3)
		{
			if(interrupt_flag)
				rotation_mode=14;
			else
			    rotation_mode=5;
		}
		else if(rotation_mode==5)
		{
			if(interrupt_flag)
				putTool_mode=7;
			else
			    putTool_mode=2;
		}
		else if(putTool_mode==2)
		{
			if(interrupt_flag)
				rotation_mode=15;
			else
			    rotation_mode=6;
		}
		else if(rotation_mode==6)
		{
			if(interrupt_flag)
			{
				ToolWheel_angles=ToolWheel.eulerAngles.z;
				ToolID=tempID;
				selectTool();
				getToolInfo();
				if(Tool_flag)
					changer_mode=15;
				else
					revolution_mode=6;
			}
			else
			{
			    changer_mode=6;
				revolution_flag=false;
			}
		}
		else if(changer_mode==6)
		{
			if(interrupt_flag)
			{
				if(Tool_flag)
					getTool_mode=5;
				else
					revolution_mode=6;
			}
			else
			{
				installedToolID=ToolID;
				installedTool_NumD=Tool_NumD;
				installedTool_flag=Tool_flag;
			}
		}
		else if(rotation_mode==7)
			changer_mode=8;
		else if(changer_mode==8)
			getTool_mode=3;
		else if(getTool_mode==3)
			changer_mode=9;
		else if(changer_mode==9)
			putTool_mode=3;
		else if(putTool_mode==3)
			rotation_mode=8;
		else if(rotation_mode==8)
			changer_mode=10;
		else if(changer_mode==10)
		{
			revolution_mode=1;
			ToolWheel_angles=ToolWheel.eulerAngles.z;
		}
		else if(rotation_mode==9)
			revolution_mode=4;
		else if(revolution_mode==4)
			rotation_mode=10;
		else if(rotation_mode==10)
			getTool_mode=4;
		else if(getTool_mode==4)
			changer_mode=11;
		else if(changer_mode==11)
			putTool_mode=4;
		else if(putTool_mode==4)
			rotation_mode=11;
		else if(rotation_mode==11)
			changer_mode=12;
		else if(changer_mode==12)
		{
			revolution_mode=1;
			ToolWheel_angles=ToolWheel.eulerAngles.z;
		}
		else if(revolution_mode==5)
			rotation_mode=12;
		else if(rotation_mode==12)
			putTool_mode=5;
		else if(putTool_mode==5)
			rotation_mode=13;
		else if(rotation_mode==13)
			changer_mode=12;
		else if(changer_mode==13)
			putTool_mode=6;
		else if(putTool_mode==6)
		{
			rotation_mode=9;
			Tool_NumD.parent=Tool_Num;
		}
		else if(rotation_mode==14)
			putTool_mode=7;
		else if(putTool_mode==7)
			rotation_mode=15;
		else if(rotation_mode==15)
		{
			ToolWheel_angles=ToolWheel.eulerAngles.z;
			ToolID=tempID;
			selectTool();
			getToolInfo();
			if(Tool_flag)
			    changer_mode=15;
			else
				revolution_mode=6;
		}
		else if(revolution_mode==6)
			changer_mode=14;
		if(changer_mode==15)
			getTool_mode=5;
		else if(getTool_mode==5)
			changer_mode=16;
		else if(changer_mode==16)
			revolution_mode=7;
		else if(revolution_mode==7)
			rotation_mode=17;
		else if(rotation_mode==17)
			putTool_mode=8;
		else if(putTool_mode==8)
			rotation_mode=16;
		else if(rotation_mode==16)
			revolution_mode=6;
	}
	//转盘转动，刘旋，2013-5-24
	void Revolution()
	{
		if(Revolution_angles<ToolWheel_revolutionAngles)
		{
			//转盘按设定的角度和方向绕点转动,刘旋，2013-5-24
			ToolWheel.RotateAround(Revolution_point,GameObject.Find("tools box_9").transform.forward,ToolWheel_revolutionDirection*40*Time.deltaTime);
			Revolution_angles+=40*Time.deltaTime;
		}
		else
		{
			for(int i=0;i<100000000;i++)
				;
			if(revolution_mode!=0)
			    get_mode();
			revolution_mode=0;
			Revolution_angles=0;
		}
	}
	void Rotation(float angles)
	{
		if(angles>0)
			Tool_rotationDirection=-1;
		else
		{
			Tool_rotationDirection=1; 
			angles=-angles;
		}
		if(Rotation_angles<angles)
		{
			Tool_NumB.parent=Tool_holder;
			Tool_Num.RotateAround(Tool_NumB.position,GameObject.Find("tools box_9").transform.right,Tool_rotationDirection*40*Time.deltaTime);//刀柄绕点转动,刘旋，2013-5-24
			Tool_Num.Translate(Tool_Num.InverseTransformDirection(new Vector3(0,1,0))*(-Tool_rotationDirection)*0.01044f*Time.deltaTime,Space.Self);//刀柄上下移动,刘旋，2013-5-24
			Tool_holder.Translate(0,-Tool_rotationDirection*0.01044f*Time.deltaTime,0,Space.Self);//刀座的移动,刘旋，2013-5-24
			Rotation_angles+=40*Time.deltaTime;
		}
		else
		{
			for(int i=0;i<100000000;i++)
				;
			Tool_NumB.parent=ToolWheel;
			if(rotation_mode!=0)
			    get_mode();
			rotation_mode=0;
			Rotation_angles=0;
		}
	}
	//刀具自转，刘旋，2013-5-24
	void ChangerRotation(float angles)
	{
		if(angles>0)
			Changer_rotationDirection=-1;
		else
		{
			Changer_rotationDirection=1;
			angles=-angles;
		}
		if(ChangerRotation_angles<angles)
		{
			ToolChanger.Rotate(0,Changer_rotationDirection*40*Time.deltaTime,0,Space.Self);
			ChangerRotation_angles+=40*Time.deltaTime;
		}
		else
		{
			for(int i=0;i<100000000;i++)
				;
			if(changer_mode!=0)
			    get_mode();
			changer_mode=0;
			ChangerRotation_angles=0;
		}
	}
	//取刀，刘旋，2013-5-24
	void getTool(float distance)
	{
		if(distance>0)
			getTool_Direction=-1;
		else
		{
			getTool_Direction=1;
			distance=-distance;
		}
		if(getTool_position<distance)
		{
			ToolChanger.Translate(0,getTool_Direction*0.02f*Time.deltaTime,0,Space.Self);
			getTool_position+=0.02f*Time.deltaTime;
		}
		else
		{
			for(int i=0;i<100000000;i++)
				;
			if(getTool_mode!=0)
			    get_mode();
			getTool_mode=0;
			getTool_position=0;
		}
	}
	//安装刀具，刘旋，2013-5-24
	void putTool(float distance)
	{
		if(distance>0)
			putTool_Direction=1;
		else
		{
			distance=-distance;
			putTool_Direction=-1;
		}
		if(putTool_position<distance)
		{
			ToolChanger.Translate(0,putTool_Direction*0.02f*Time.deltaTime,0,Space.Self);
			putTool_position+=0.02f*Time.deltaTime;
		}
		else 
		{
			for(int i=0;i<100000000;i++)
				;
			if(putTool_mode!=0)
			    get_mode();
			putTool_mode=0;
			putTool_position=0;
		}
	}
	//输入刀号，刘旋，2013-5-24
	void getToolID()
	{
		
		if(Input.GetKey(KeyCode.Alpha0))
		{
			if(tempToolID=="")
				tempToolID="0";
			if(tempToolID=="1")
				tempToolID="10";
			if(tempToolID=="2")
				tempToolID="20";
		}
		if(Input.GetKey(KeyCode.Alpha1))
		{
			if(tempToolID=="")
			{
			    tempToolID="1";
				time_value=0;
			}
			if(tempToolID=="0")
				tempToolID="01";
		}
		if(tempToolID=="1")
		{
			time_value+=Time.deltaTime;
			if(time_value>0.5f)
			{
				Debug.Log("time_value>0.5f");
				if(Input.GetKey(KeyCode.Alpha1))
				{
					tempToolID="11";
					time_value=0;
				}
			}
		}
		if(Input.GetKey(KeyCode.Alpha2))
		{
			if(tempToolID=="")
				tempToolID="2";
			if(tempToolID=="0")
				tempToolID="02";
			if(tempToolID=="1")
				tempToolID="12";
		}
		if(Input.GetKey(KeyCode.Alpha3))
		{
			if(tempToolID=="0")
				tempToolID="03";
			if(tempToolID=="1")
				tempToolID="13";
		}
		if(Input.GetKey(KeyCode.Alpha4))
		{
			if(tempToolID=="0")
				tempToolID="04";
			if(tempToolID=="1")
				tempToolID="14";
		}
		if(Input.GetKey(KeyCode.Alpha5))
		{
			if(tempToolID=="0")
				tempToolID="05";
			if(tempToolID=="1")
				tempToolID="15";
		}
		if(Input.GetKey(KeyCode.Alpha6))
		{
			if(tempToolID=="0")
				tempToolID="06";
			if(tempToolID=="1")
				tempToolID="16";
		}
		if(Input.GetKey(KeyCode.Alpha7))
		{
			if(tempToolID=="0")
				tempToolID="07";
			if(tempToolID=="1")
				tempToolID="17";
		}
		if(Input.GetKey(KeyCode.Alpha8))
		{
			if(tempToolID=="0")
				tempToolID="08";
			if(tempToolID=="1")
				tempToolID="18";
		}
		if(Input.GetKey(KeyCode.Alpha9))
		{
			if(tempToolID=="0")
				tempToolID="09";
			if(tempToolID=="1")
				tempToolID="19";
		}
		if(tempToolID.Length>1)
		{
			int temp=Convert.ToInt32(tempToolID);
		    if(temp>0)
		    {
			    if(temp<21)
			    {
				    ToolID=temp;
			    }
			    if(revolution_flag)
				{
					ToolID=installedToolID;
				}
		    }
		}
		
	}
	//获取当前刀柄，刀具和刀座，刘旋，2013-5-24
	void selectTool()
	{
		Debug.Log("Hcount="+Tool_H.Count+"Dcount="+Tool_D.Count+"Fcount="+Tool_F.Count);
		Tool_Num=Tool_H[ToolID];
		Tool_NumD=Tool_D[ToolID];
		Tool_NumB=Tool_Bottom[ToolID];
		Tool_flag=Tool_F[ToolID];
	}
	//获取转盘转动信息，刘旋，2013-5-24
	void getToolInfo()
	{
		ToolWheel_revolutionAngles=18f*(ToolID+1)+ToolWheel_angles-ToolWheel_diffangles;
		Debug.Log("yuanshi="+ToolWheel_revolutionAngles);
		if(ToolWheel_revolutionAngles>540f)
		{
			ToolWheel_revolutionDirection=-1;
			ToolWheel_revolutionAngles=720f-ToolWheel_revolutionAngles;
		}
		else if(ToolWheel_revolutionAngles>360f)
		{
			ToolWheel_revolutionDirection=1;
			ToolWheel_revolutionAngles-=360f;
		}
		else if(ToolWheel_revolutionAngles>180f)
		{
			ToolWheel_revolutionDirection=-1;
			ToolWheel_revolutionAngles=360f-ToolWheel_revolutionAngles;
		}
		else if(ToolWheel_revolutionAngles>0)
		{
			ToolWheel_revolutionDirection=1;
		}
		else if(ToolWheel_revolutionAngles<-180f)
		{
			ToolWheel_revolutionDirection=-1;
			ToolWheel_revolutionAngles+=360;
		}
		else if(ToolWheel_revolutionAngles<0)
		{
			ToolWheel_revolutionDirection=-1;
			ToolWheel_revolutionAngles=-ToolWheel_revolutionAngles;
		}
	}
	
}
