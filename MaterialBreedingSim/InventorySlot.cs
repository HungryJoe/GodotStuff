using Godot;
using System;

public class InventorySlot : Panel
{
  [Export]
  public readonly float ALPHA_SELECTED = 1f;
  [Export]
  public readonly float ALPHA_UNSELECTED = 0.5f;
  public Material mat;
  public bool selected;

  private TextureRect displayMaterial;
  private int index;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    mat = null;
    index = Int32.Parse(this.Name);
    displayMaterial = GetNode<TextureRect>("Inside/Material");
    selected = false;
    GetNode("..").Connect("MaterialChanged", this, "ChangeMaterial");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    if (selected) {
      Color c = this.Modulate;
      c.a = ALPHA_SELECTED;
      this.Modulate = c;
    } else {
      Color c = this.Modulate;
      c.a = ALPHA_UNSELECTED;
      this.Modulate = c;
    } 
  }

  public void ChangeMaterial(int index, Material mat) {
    if (index == this.index) {
      this.mat = mat;
      displayMaterial.Texture = mat.tex;
    }
  }
}
