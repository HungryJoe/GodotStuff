using Godot;
using System;

public class globals : Node
{
    public static int currentStage {get; set;}
	public static int kills {get; set;}
	
	public override void _Ready() {
		currentStage = 1;
		kills = 0;
	}
}
