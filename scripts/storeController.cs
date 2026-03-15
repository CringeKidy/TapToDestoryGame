using Godot;
using System;

public partial class storeController : CanvasLayer
{
	[Signal]
	public delegate void ShopFinishedEventHandler();


	[Export]
	public Label playersGoldLabel;

	[Export]
	public Panel notEnoughGoldPopUpPanel;

	private Timer popupTimer;

	//TODO add dynamic upgrades so they getting added to the shop dynamically instead of manually having to make one every time. 

	//TODO touch up things so when the user doesn't have enough money for something, Buttons get disabled. 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get timer to start it when there is not enough gold to buy something. 
		updateGoldLabel();
		popupTimer = GetNode<Timer>("popupTimer");
	}

	public void OnBuyClickDamage()
	{
		if (GameManager.Gold < 5)
		{
			notEnoughGoldPopUpPanel.Visible = true;
			popupTimer.Start();
		}
		else
		{
			GameManager.clickDamage++;
			GameManager.Gold -= 5;
			updateGoldLabel();
		}
	}
	public void CloseShop()
	{
		GetTree().Paused = false;
		EmitSignal(SignalName.ShopFinished);
		this.Visible = false;
		GD.Print("Close Shop Button has been pressed.");
	}

	public void popupTimerTimeout()
	{
		notEnoughGoldPopUpPanel.Visible = false;
	}


	public void updateGoldLabel()
	{
		playersGoldLabel.Text = "You have " + GameManager.Gold + " Gold Peaces";
	}
}
