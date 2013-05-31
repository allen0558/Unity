using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelInitialize : MonoBehaviour {
	Transform KD;  //The total gameobjects of  the machine;
	public Transform OuterSkin1; //The outer skin of the machine;
	public Transform X_axis1; 
	public Transform Y_axis1;
	public Transform Z_axis1;
	public Transform Tool_Changer;
	public Transform ProtectingCrust;
	public Transform TheRest; //The rest part of the machine except for the 3 axis and the outer skin;
	
	public Transform leftDoor;
	public Transform rightDoor;
	
	void Awake() 
	{
		Initialize();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	void Initialize()
	{
		List<Transform> AllTransform = new List<Transform>(); //The children transform of KD(The machine);
		List<string> Y_axis_name;
		List<string> ProtectingCrust_name;
		KD = GameObject.Find("KD").transform;
		foreach(Transform child in KD.transform)
			AllTransform.Add(child);
		Y_axis_name = new List<string>();
		Y_axis_name.Add("X axle_1");
		Y_axis_name.Add("XYZ protecting crust_12");
		Y_axis_name.Add("XYZ protecting crust_13");
		Y_axis_name.Add("XYZ protecting crust_16");
		Y_axis_name.Add("XYZ protecting crust_17");
		Y_axis_name.Add("XYZ protecting crust_18");
		Y_axis_name.Add("XYZ protecting crust_19");
		Y_axis_name.Add("XYZ protecting crust_20");
		Y_axis_name.Add("XYZ protecting crust_25");
		Y_axis_name.Add("XYZ protecting crust_26");
		Y_axis_name.Add("XYZ protecting crust_27");
		Y_axis_name.Add("XYZ protecting crust_28");
		Y_axis_name.Add("XYZ protecting crust_29");
		ProtectingCrust_name = new List<string>();
		ProtectingCrust_name.Add("XYZ protecting crust_3");
		ProtectingCrust_name.Add("XYZ protecting crust_4");
		ProtectingCrust_name.Add("XYZ protecting crust_5");
		ProtectingCrust_name.Add("XYZ protecting crust_6");
		ProtectingCrust_name.Add("XYZ protecting crust_7");
		ProtectingCrust_name.Add("XYZ protecting crust_8");
		ProtectingCrust_name.Add("XYZ protecting crust_9");
		ProtectingCrust_name.Add("XYZ protecting crust_10");
		ProtectingCrust_name.Add("XYZ protecting crust_11");
		List<string> LeftDoor_str = new List<string>();
		List<string> RightDoor_str = new List<string>();
		LeftDoor_str.Add("main protecting crust_13");
		LeftDoor_str.Add("main protecting crust_15");
		RightDoor_str.Add("main protecting crust_14");
		RightDoor_str.Add("main protecting crust_16");
		
		try
		{
			ProtectingCrust = GameObject.Find("GameObject").transform;
		}
		catch
		{
			Debug.LogError("Need to add 7 Empty GameObject by manually: Error caused by Eric Jiang.");
			return;
		}
		ProtectingCrust.name = "ProtectingCrust";
		ProtectingCrust.parent = KD;
		Transform temp_tran01 = GameObject.Find("XYZ protecting crust_7").transform;
		ProtectingCrust.localPosition = temp_tran01.localPosition;
		
		try
		{
			Tool_Changer = GameObject.Find("GameObject").transform;
		}
		catch
		{
			Debug.LogError("Need to add 6 Empty GameObject by manually: Error caused by Eric Jiang.");
			return;
		}
		Tool_Changer.name = "Tool_Changer";
		Tool_Changer.parent = KD;
		Tool_Changer.localPosition = GameObject.Find("main axle_3").transform.localPosition;
		//Tool_Changer.localPosition = Vector3.zero;
		
		try
		{
			OuterSkin1 = GameObject.Find("GameObject").transform;
		}
		catch
		{
			Debug.LogError("Need to add 5 Empty GameObject by manually: Error caused by Eric Jiang.");
			return;
		}
		OuterSkin1.name = "OuterSkin1";
		OuterSkin1.parent = KD;
		Transform temp_tran02 = GameObject.Find("main protecting crust_1").transform;
		OuterSkin1.localPosition = temp_tran02.localPosition;
		leftDoor = GameObject.Find("main protecting crust_11").transform;
		leftDoor.gameObject.AddComponent("BoxCollider");
		leftDoor.gameObject.AddComponent("door");
		rightDoor = GameObject.Find("main protecting crust_12").transform;
		rightDoor.gameObject.AddComponent("BoxCollider");
		rightDoor.gameObject.AddComponent("door");
		
		try
		{
			Y_axis1 = GameObject.Find("GameObject").transform;
		}
		catch
		{
			Debug.LogError("Need to add 3 Empty GameObject by manually: Error caused by Eric Jiang.");
			return;
		}
		Y_axis1.name = "Y_axis1";
		Y_axis1.parent = KD;
		Y_axis1.localPosition = GameObject.Find("X axle_1").transform.localPosition;
		
		try
		{
			X_axis1 = GameObject.Find("GameObject").transform;
		}
		catch
		{
			Debug.LogError("Need to add 4 Empty GameObject by manually: Error caused by Eric Jiang.");
			return;
		}
		X_axis1.name = "X_axis1";
		Transform temp_tran = GameObject.Find("workbench_1").transform;
		temp_tran.parent = Y_axis1;
		X_axis1.parent = Y_axis1;
		X_axis1.localPosition = temp_tran.localPosition;
		temp_tran.parent = KD;
		
		try
		{
			Z_axis1 = GameObject.Find("GameObject").transform;
		}
		catch
		{
			Debug.LogError("Need to add 2 Empty GameObject by manually: Error caused by Eric Jiang.");
			return;
		}
		Z_axis1.name = "Z_axis1";
		Z_axis1.parent = KD;
		Z_axis1.localPosition = GameObject.Find("main axle_2").transform.localPosition;

		try
		{
			TheRest = GameObject.Find("GameObject").transform;
		}
		catch
		{
			Debug.LogError("Need to add 1 Empty GameObject by manually: Error caused by Eric Jiang.");
			return;
		}
		TheRest.name = "TheRest";
		TheRest.parent = KD;
		TheRest.localPosition = Vector3.zero;

		for(int i = 0; i < AllTransform.Count; i++)
		{
			//Classify the X axis parts.
			if(AllTransform[i].name == "workbench_1" || AllTransform[i].name == "XYZ protecting crust_21" || AllTransform[i].name == "XYZ protecting crust_22" || AllTransform[i].name == "XYZ protecting crust_23" || AllTransform[i].name == "XYZ protecting crust_24" || AllTransform[i].name.StartsWith("XYZ protecting crust_30") || AllTransform[i].name.StartsWith("fixture"))
			{
				AllTransform[i].parent = X_axis1;
				AllTransform.RemoveAt(i);
				i--;
			}
			//Classify the Y axis parts.
			else if(Y_axis_name.IndexOf(AllTransform[i].name) != -1)
			{
				AllTransform[i].parent = Y_axis1;
				AllTransform.RemoveAt(i);
				i--;
			}
			//Classify the Z axis parts.
			else if(AllTransform[i].name.StartsWith("main axle") || AllTransform[i].name.StartsWith("cooling hose"))
			{
				AllTransform[i].parent = Z_axis1;
				AllTransform.RemoveAt(i);
				i--;
			}
			//Classify the Left Door parts
			else if(LeftDoor_str.IndexOf(AllTransform[i].name) != -1)
			{
				AllTransform[i].parent = leftDoor;
				AllTransform.RemoveAt(i);
				i--;
			}
			//Classify the Right Door parts
			else if(RightDoor_str.IndexOf(AllTransform[i].name) != -1)
			{
				AllTransform[i].parent = rightDoor;
				AllTransform.RemoveAt(i);
				i--;
			}
			//Classify the Outer Skin parts.
			else if(AllTransform[i].name.StartsWith("face plate") || AllTransform[i].name.StartsWith("main protecting") || AllTransform[i].name.StartsWith("power box") || AllTransform[i].name.StartsWith("scrap iron box") || AllTransform[i].name.StartsWith("cooling pump") || AllTransform[i].name.StartsWith("hose"))
			{
				AllTransform[i].parent = OuterSkin1;
				AllTransform.RemoveAt(i);
				i--;
			}
			//Classify the Tool Changer parts.
			else if(AllTransform[i].name.StartsWith("tools box"))
			{
				AllTransform[i].parent = Tool_Changer;
				AllTransform.RemoveAt(i);
				i--;
			}
			//Classify the removable Protecting Crust parts.
			else if(ProtectingCrust_name.IndexOf(AllTransform[i].name) != -1)
			{
				AllTransform[i].parent = ProtectingCrust;
				AllTransform.RemoveAt(i);
				i--;
			}
			//Classify the Rest parts.
			else
			{
				AllTransform[i].parent = TheRest;
				AllTransform.RemoveAt(i);
				i--;
			}	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
