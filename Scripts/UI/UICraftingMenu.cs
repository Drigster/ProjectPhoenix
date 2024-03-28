using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class UICraftingMenu : PanelContainer
{
	[Export] private GridContainer _itemsGrid;
	[Export] private Button _craftButton;
	[Export] private SpinBox _itemsToCraftInput;
	[Export] private TextureRect _recepieIcon;
	[Export] private Label _recepieName;
	[Export] private Label _recepieDescription;

	private CraftingRecepie _selectedRecepie;

	[Export] private InventorySystem _playerInventory;
	[Export] private InventorySystem _playerHotbar;
	[Export] private RecepieManager _recepieManager;

	public override void _Ready()
	{
		base._Ready();
		_itemsGrid = GetNode<GridContainer>("%ItemsGrid");
		_craftButton = GetNode<Button>("%CraftButton");
		_itemsToCraftInput = GetNode<SpinBox>("%ItemsToCraftInput");
		_recepieIcon = GetNode<TextureRect>("%Icon");
		_recepieName = GetNode<Label>("%Name");
		_recepieDescription = GetNode<Label>("%Description");
		_recepieIcon.Texture = null;
		_recepieName.Text = "";
		_recepieDescription.Text = "";

		ReferenceCenter referenceCenter = GetNode<ReferenceCenter>("/root/ReferenceCenter");
		_playerInventory = referenceCenter.Player.GetNode<InventorySystem>("%Inventory");
		_playerHotbar = referenceCenter.Player.GetNode<InventorySystem>("%Hotbar");

		_recepieManager = GetNode<RecepieManager>("/root/RecepieManager");
		
		foreach (Node child in _itemsGrid.GetChildren())
		{
			child.QueueFree();
		}

		foreach (CraftingRecepie recepie in _recepieManager.CraftingRecepies)
		{
			UICraftingRecepie uICraftingRecepie = new UICraftingRecepie(recepie);
			_itemsGrid.AddChild(uICraftingRecepie);
			uICraftingRecepie.OnRecepieClicked += () => {
				_selectedRecepie = recepie;
				_recepieIcon.Texture = _selectedRecepie.Output.ItemData.Icon;
				_recepieName.Text = _selectedRecepie.Output.ItemData.Name;
				_recepieDescription.Text = _selectedRecepie.Output.ItemData.Description;

				Reload();
			};
		}

		_selectedRecepie = _recepieManager.CraftingRecepies.FirstOrDefault();
		_recepieIcon.Texture = _selectedRecepie.Output.ItemData.Icon;
		_recepieName.Text = _selectedRecepie.Output.ItemData.Name;
		_recepieDescription.Text = _selectedRecepie.Output.ItemData.Description;
		_craftButton.Disabled = true;

		_craftButton.Pressed += Craft;
		_playerInventory.OnInventoryChanged += Reload;
		
		Reload();
	}

	public void Reload()
	{
		foreach (Item input in _selectedRecepie.Input)
		{
			if(_playerInventory.CountItems(input.ItemData) + _playerHotbar.CountItems(input.ItemData) >= input.Amount){
				_craftButton.Disabled = false;
			}
			else{
				_craftButton.Disabled = true;
			}
		}
	}

	public void Craft()
	{
		if (_selectedRecepie == null){
			return;
		}

		int itemsToCraft = (int)_itemsToCraftInput.Value;

		foreach (Item input in _selectedRecepie.Input)
		{
			if(_playerInventory.CountItems(input.ItemData) + _playerHotbar.CountItems(input.ItemData) < input.Amount * itemsToCraft){
				return;
			}
		}

		for (int i = 0; i < itemsToCraft; i++)
		{
			foreach (Item input in _selectedRecepie.Input)
			{
				int itemsToRemove = Math.Min(input.Amount, _playerInventory.CountItems(input.ItemData));
				if(itemsToRemove  > 0){
					_playerInventory.RemoveItems(input.ItemData, itemsToRemove);
				}
				if(input.Amount - itemsToRemove > 0){
					_playerHotbar.RemoveItems(input.ItemData, input.Amount - itemsToRemove);
				}
			}
			if(_playerInventory.CanAddItem(_selectedRecepie.Output)){
				_playerInventory.AddItem(_selectedRecepie.Output);
			}
			else if(_playerInventory.CanAddItemsCount(_selectedRecepie.Output.ItemData, out int amount)){
				_playerInventory.AddItems(_selectedRecepie.Output.ItemData, amount);
				_playerHotbar.AddItems(_selectedRecepie.Output.ItemData, _selectedRecepie.Output.Amount - amount);
			}
			else if(_playerHotbar.CanAddItem(_selectedRecepie.Output)){
				_playerHotbar.AddItem(_selectedRecepie.Output);
			}
			else{
				throw new NotImplementedException("UICraftingMenu._craftButton.Pressed: Drop item.");
			}
		}
	}
}
