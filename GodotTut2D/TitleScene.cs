using Godot;
using System;

public class TitleScene : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        globals.kills = 0;
		globals.currentStage = 0;
    }

	private void _on_QuitGameButton_pressed()
	{
	    GetTree().Quit();
	}
	
	private void _on_NewGameButton_pressed()
	{
	    GetTree().ChangeScene("GameScene.tscn");
	}
}
