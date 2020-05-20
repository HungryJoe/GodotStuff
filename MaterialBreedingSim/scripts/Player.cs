using Godot;
using System;

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
    
    private bool is_airborn;
    private Spatial head;
    private Camera cam;
    private RayCast raycast;
    private Material mat;
    private GridMap world_grid;

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
      acc_grav = 2f;
      velocity = new Vector3(0,0,0);
      mouse_sens = 0.8f;
      cam_angle = 0;
      is_airborn = false;
      
      Node inv = GetNode("GUI/Inventory");
      inv.Connect("SelectChanged", this, "ChangeSelected");
      EmitSignal("PlayerReady");
    }

    public override void _Input(InputEvent @event) {
      if (@event is InputEventMouseMotion event_mouse_motion) {
     	  head.RotateY(Deg2Rad(-1 * event_mouse_motion.Relative.x * mouse_sens));

      	float dx_rot = event_mouse_motion.Relative.y * mouse_sens;
      	if (cam_angle - dx_rot > -90 && cam_angle - dx_rot < 90) {
      	  cam.RotateX(Deg2Rad(-1 * dx_rot));
      	  cam_angle -= dx_rot;
      	}
      }

      //Place blocks!
      if (@event.IsActionPressed("player_block_place")) {
        if (raycast.IsColliding() && mat != null) {
          Block bk_inst = mat.getBlock();
          bk_inst.Translation = raycast.GetCollisionPoint();
          GetNode("/root/WorldRoot").AddChild(bk_inst);
        }
      }
    }

    public override void _PhysicsProcess(float delta) {
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
      
      float oldY = velocity.y;
      velocity = spd_XZ * direction;
      velocity.y = oldY;
      if (Input.IsActionPressed("player_jump") && !is_airborn) {
        velocity.y += spd_jump;
	is_airborn = true;
      } else if (IsOnFloor()) {
        velocity.y = 0;
	is_airborn = false;
      }
      if (is_airborn) {
        velocity.y -= acc_grav;
      }

      this.MoveAndSlide(velocity, new Vector3(0,1,0), false, 1, Deg2Rad(45f), false);
    }

    private float Deg2Rad(float degrees) {
      return (float)Math.PI * degrees / 180;
    }

    public void ChangeSelected(int index) {
      string selPath = String.Format("GUI/Inventory/{0}", index);
      InventorySlot selected = GetNode<InventorySlot>(selPath);
      mat = selected.mat;
    }
}
