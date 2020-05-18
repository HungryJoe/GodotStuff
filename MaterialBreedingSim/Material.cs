using Godot;
using System;

public class Material : Godot.Object
{
  public Texture tex;

  public Material(Texture tex) {
    this.tex = tex;
  }
}
