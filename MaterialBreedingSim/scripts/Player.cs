//Player controls the player's movement and block placing

using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody
{
    [Signal]//So the inventory can pass the player its state even though it loads first
    public delegate void PlayerReady();
    public int spd_XZ;
    public int spd_jump;
    public float acc_grav;
    public Vector3 velocity;
    public float mouse_sens;
    public float cam_angle;

    private Spatial head;
    private Camera cam;
    private RayCast raycast;
    private Material mat;
    private GridMap world_grid;
    private bool cap_mouse;

    private const float TOLERANCE = 0.1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      head = GetNode<Spatial>("Head");
      cam = GetNode<Camera>("Head/Camera");
      raycast = GetNode<RayCast>("Head/Camera/PlaceBlocksRC");
      mat = null;
      world_grid = GetNode<GridMap>("/root/WorldRoot/GridMap");

      spd_XZ = 15;
      spd_jump = 30;
      acc_grav = 1.8f;
      velocity = new Vector3(0,0,0);
      mouse_sens = 0.5f;
      cam_angle = 0;
      cap_mouse = true;

      SwitchMouseMode();//Capture the mouse from the first

      GetNode("GUI/Inventory").Connect("SelectChanged", this, "ChangeSelected");
      EmitSignal("PlayerReady");
    }

    //Called whenever the player inputs something
    public override void _Input(InputEvent @event) {
      if (@event is InputEventMouseMotion event_mouse_motion) {
        //Move camera according to mouse motion
     	  head.RotateY(Deg2Rad(-1 * event_mouse_motion.Relative.x * mouse_sens));
      	float dx_rot = event_mouse_motion.Relative.y * mouse_sens;
      	if (cam_angle - dx_rot > -90 && cam_angle - dx_rot < 90) {
      	  cam.RotateX(Deg2Rad(-1 * dx_rot));
      	  cam_angle -= dx_rot;
      	}
      }

      if (@event.IsActionPressed("player_block_place") && raycast.IsColliding() && mat != null) {
        //Place blocks!
        int[] g_pos = Vector3ToInts(world_grid.WorldToMap(raycast.GetCollisionPoint() + TOLERANCE * Heading()));
        if (world_grid.GetCellItem(g_pos[0], g_pos[1], g_pos[2]) == -1) {
          world_grid.SetCellItem(g_pos[0], g_pos[1], g_pos[2], mat.ml_idx);
        }
      }

      if (@event.IsActionReleased("player_pause")) {
        cap_mouse = !cap_mouse;
        SwitchMouseMode();
      }
    }

    //Converts a Vector3 to an array of ints
    private int[] Vector3ToInts(Vector3 v) {
      int[] a = new int[3];
      a[0] = (int)v.x;
      a[1] = (int)v.y;
      a[2] = (int)v.z;
      return a;
    }

    //Gets the direction the player is currently facing as a unit vector
    private Vector3 Heading() {
      Basis hb = head.Transform.basis;
      Vector3 v = new Vector3(0,0,0);
      v = hb.x + hb.y + hb.z;
      return v.Normalized();
    }

    //Switch between capturing the cursor in the game window, rendering it invisible,
    //and normal cursor behavior
    private void SwitchMouseMode() {
      if (cap_mouse) {
        Input.SetMouseMode(Input.MouseMode.Captured);
      } else {
        Input.SetMouseMode(Input.MouseMode.Visible);
      }
    }

    //Called whenever the game wants to update its physics simulation
    public override void _PhysicsProcess(float delta) {
      //Gets the direction, in terms of X and Z, that the player would like to move in
      Basis head_basis = head.Transform.basis;
      Vector3 direction = new Vector3(0,0,0);
      if (Input.IsActionPressed("player_forward")) {
        direction -= head_basis.z;
      }
      if (Input.IsActionPressed("player_backward")) {
        direction += head_basis.z;
      }
      if (Input.IsActionPressed("player_left")) {
        direction -= head_basis.x;
      }
      if (Input.IsActionPressed("player_right")) {
        direction += head_basis.x;
      }
      direction = direction.Normalized();

      //Calculates current velocity
      Vector3 maxVel = spd_XZ * direction;
      maxVel.y = velocity.y;
      if (IsOnFloor()) {
        if (Input.IsActionPressed("player_jump"))
          maxVel.y += spd_jump;
        else
          maxVel.y = 0;
      } else {
        maxVel.y -= acc_grav;
      }
      velocity = velocity.LinearInterpolate(maxVel, 0.75f);//Allows some sliding, but not too much

      velocity = this.MoveAndSlide(velocity, new Vector3(0,1,0), false, 1, Deg2Rad(45f), false);
    }

    //Converts degrees to radians
    private float Deg2Rad(float degrees) {
      return (float)Math.PI * degrees / 180;
    }

    //Called when Inventory emits its SelectChanged signal
    public void ChangeSelected(int index) {
      string selPath = String.Format("GUI/Inventory/{0}", index);
      InventorySlot selected = GetNode<InventorySlot>(selPath);
      mat = selected.mat;
    }
}
