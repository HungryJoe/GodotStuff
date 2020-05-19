using Godot;
using System;

public class Material : Godot.Object
{
  public Texture tex;
  private PackedScene block;

  public Material(string name) {
    this.tex = GD.Load<Texture>(String.Format("res://textures/{0}.png", name));
    this.block = GD.Load<PackedScene>(String.Format("res://Block-{0}.tscn", name));
  }

  public Block getBlock() {
    return (Block)block.Instance();
  }
}
