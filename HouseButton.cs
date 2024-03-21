using Godot;
using System;

public partial class HouseButton : Button
{
	public int x = 0;
	public int y = 0;

	public void SetIntersected(bool intersected)
	{
		if (intersected)
		{
			Modulate = Color.Color8(255, 55, 55);
		} 
		else
		{
			Modulate = Color.Color8(255, 255, 255);
		}
	}


}
