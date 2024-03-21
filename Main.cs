using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Control
{
	public HouseMatrix house_matrix { get; set; }
	
	
	public override void _Ready()
	{
		// Connect signals
		GetNode<SpinBox>("Container/GridSize").ValueChanged += GridSize_ValueChanged; ;
		GetNode<SpinBox>("Container/KernelSize").ValueChanged += KernelSize_ValueChanged; ;
		
		// Make initial grid
		house_matrix = new HouseMatrix(10, 3);
		CreateGrid();
	}

	/// <summary>
	/// Signal connected from the KernelSize SpinBox node.
	/// </summary>
	private void KernelSize_ValueChanged(double value)
	{
		house_matrix = new HouseMatrix(house_matrix.matrix_size, (int)value);
		CreateGrid();
	}

	/// <summary>
	/// Signal connected from the GridSize SpinBox node.
	/// </summary>
	private void GridSize_ValueChanged(double value)
	{
		house_matrix = new HouseMatrix((int)value, house_matrix.kernel_size);
		CreateGrid();
	}

	/// <summary>
	/// Create a new n*n grid of buttons with n=house_matrix.matrix_size.
	/// </summary>
	private void CreateGrid()
	{
		GridContainer grid = GetNode<GridContainer>("GridContainer");

		// Remove all existing buttons
		foreach (HouseButton item in grid.GetChildren())
		{
			item.QueueFree();
		}

		grid.Columns = house_matrix.matrix_size;

		// Add new buttons
		var scene = GD.Load<PackedScene>("res://HouseButton.tscn");
		for (int i = 0; i < house_matrix.matrix_size; i++)
		{
			for (int j = 0; j < house_matrix.matrix_size; j++)
			{
				HouseButton button_instance = (HouseButton)scene.Instantiate();
				grid.AddChild(button_instance);
				button_instance.x = i;
				button_instance.y = j;
				button_instance.Toggled += ButtonPressed => Button_instance_Toggled(ButtonPressed, button_instance.x, button_instance.y);
			}
			
		}
	}

	/// <summary>
	/// Signal connected from the HouseButtons in the GridContainer node.
	/// </summary>
	private void Button_instance_Toggled(bool toggledOn, int x, int y)
	{
		if (toggledOn)
		{
			house_matrix.AddHouse(x, y);
		}
		else
		{
			house_matrix.RemoveHouse(x, y);
		}

		DisplayIntersection();
	}

	/// <summary>
	/// Display the house intersections in the GridContainer node.
	/// </summary>
	private void DisplayIntersection()
	{
		GridContainer grid = GetNode<GridContainer>("GridContainer");
		List<(int, int)> intersections = house_matrix.FindIntersection();

		// Clear the display of all buttons
		foreach (HouseButton item in grid.GetChildren())
		{
			item.SetIntersected(false);
		}

		if (intersections.Count == 0) { return; }
		
		// Add display for all buttons with intersecting cells
		foreach ((int, int) item in intersections)
		{
			HouseButton button = (HouseButton)grid.GetChild(item.Item1 * grid.Columns + item.Item2);
			button.SetIntersected(true);
		}
	}
}
