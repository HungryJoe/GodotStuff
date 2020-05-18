using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System;

public class Inventory : HBoxContainer
{
    [Signal]//For when a hotbar slot's material changes
    public delegate void MaterialChanged(int index, Material mat);
    [Signal]//For when the selected hotbar slot changes
    public delegate void SelectChanged(int index);

    private const int START_SELECTED = 0;//The index to start off selected

    private System.Collections.Generic.Dictionary<string, Material> mats;
    private int inv_size;
    private List<InventorySlot> inv;
    private int selected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      //Initalize materials hashmap from scratch, for now
      mats = new System.Collections.Generic.Dictionary<string, Material>(4);
      mats.Add("stone", new Material(GD.Load<Texture>("res://textures/stone.png")));
      mats.Add("wood", new Material(GD.Load<Texture>("res://textures/wood.png")));
      mats.Add("metal", new Material(GD.Load<Texture>("res://textures/metal.png")));
      mats.Add("gel", new Material(GD.Load<Texture>("res://textures/gel.png")));

      inv_size = this.GetChildCount();
      inv = new List<InventorySlot>(inv_size);
      Godot.Collections.Array children = this.GetChildren();//Create a deep copy of this array because I don't want to deal with Godot's data structures
      foreach (InventorySlot child in children) {
        inv.Add(child);
      }
      selected = START_SELECTED;
      EmitSignal("SelectChanged", selected);

      EmitSignal("MaterialChanged", 0, mats["stone"]);
      EmitSignal("MaterialChanged", 1, mats["wood"]);
      EmitSignal("MaterialChanged", 2, mats["metal"]);
      EmitSignal("MaterialChanged", 3, mats["gel"]);
    }

    public override void _Input(InputEvent @event) {
      if (@event.IsActionPressed("inv_select_next")) {
	selected = mod(selected + 1, inv_size);
	EmitSignal("SelectChanged", selected);
      } else if (@event.IsActionPressed("inv_select_prev")) {
	selected = mod(selected - 1, inv_size);
	EmitSignal("SelectChanged", selected);
      }
    }

    private int mod(int a, int modulus) {
      int residue = a % modulus;
      while (residue < 0) {
	residue += modulus;
      }
      return residue;
    }
}
