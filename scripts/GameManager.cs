using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
	public static int clickDamage { get; set; }
	public static int Gold { get; set; }
	public static int waveCounter { get; set; }

	public override void _Ready()
	{
		Instance = this;
		clickDamage = 1;
	}

}
