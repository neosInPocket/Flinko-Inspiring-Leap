using System;

[Serializable]
public class Data : ISaveable, ICloneable
{
	public int f_level;
	public int f_gold;
	public bool f_trajectory;
	public bool f_speed;
	public bool f_volume;
	public bool f_sfx;
	public bool f_tutorial;

	public object Clone()
	{
		var data = new Data();
		data.f_level = f_level;
		data.f_gold = f_gold;
		data.f_volume = f_volume;
		data.f_sfx = f_sfx;
		data.f_tutorial = f_tutorial;
		data.f_speed = f_speed;
		data.f_trajectory = f_trajectory;
		return data;
	}
}
