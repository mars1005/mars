using Godot;
using System;

public partial class player : Area2D
{
	[Export]
	public int Speed {get;set;} = 400;
	public Vector2 ScreenSize;

    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
		Hide();
    }

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2d").Disabled = false;
	}

		

    public override void _Process(double delta)
    {
        var Velocity = Vector2.Zero;

		if(Input.IsActionPressed("move_right"))
		{
			Velocity.X += 1;
		}

		if(Input.IsActionPressed("move_left"))
		{
			Velocity.X -= 1;
		}

		if(Input.IsActionPressed("move_up"))
		{
			Velocity.Y -= 1;
		}

		if(Input.IsActionPressed("move_down"))
		{
			Velocity.Y += 1;
		}

		var animateSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if(Velocity.Length()>0)
		{
			Velocity = Velocity.Normalized() *Speed;
			animateSprite2D.Play();
		}
		else
		{
			animateSprite2D.Stop();
		}
		
		Position += Velocity * (float)delta;
		Position = new Vector2(

			x:Mathf.Clamp(Position.X,0,ScreenSize.X),
			y:Mathf.Clamp(Position.Y,0,ScreenSize.Y)

		);

		if(Velocity.X != 0) //水平翻轉
		{
			animateSprite2D.Animation = "walk";
			animateSprite2D.FlipV = false; //左右移動時，保持頭朝上
			animateSprite2D.FlipH = Velocity.X < 0;
		}
		else if (Velocity.Y != 0) //垂直翻轉
		{
			animateSprite2D.Animation = "up";
			animateSprite2D.FlipV = Velocity.Y > 0;
		}
	
    }
	
	[Signal]
    public delegate void HitEventHandler();

	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2d").SetDeferred(CollisionShape2D.PropertyName.Disabled,true);
	}

}