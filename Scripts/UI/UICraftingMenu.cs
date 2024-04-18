using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class UICraftingMenu : PanelContainer
{
	[Export] private GridContainer _itemsGrid;
	[Export] private Button _craftButton;
	[Export] private LineEdit _itemsToCraftInput;
	[Export] private int _itemsToCraft = 1;
	[Export] private TextureRect _recepieIcon;
	[Export] private Label _recepieName;
	[Export] private Label _recepieDescription;
	[Export] private VBoxContainer _ingridientsContainer;

	private CraftingRecepie _selectedRecepie;

	[Export] private InventorySystem _playerInventory;
	[Export] private InventorySystem _playerHotbar;
	[Export] private RecepieManager _recepieManager;
    private PackedScene _recepieIngridientScene = GD.Load<PackedScene>("res://Scenes/UI/UIRecepieIngredient.tscn");

	public override void _Ready()
	{
		base._Ready();
		_itemsGrid = GetNode<GridContainer>("%ItemsGrid");
		_craftButton = GetNode<Button>("%CraftButton");
		_itemsToCraftInput = GetNode<LineEdit>("%ItemsToCraftInput");
		_recepieIcon = GetNode<TextureRect>("%Icon");
		_recepieName = GetNode<Label>("%Name");
		_recepieDescription = GetNode<Label>("%Description");
		_ingridientsContainer = GetNode<VBoxContainer>("%IngridientsContainer");
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
		foreach (Control child in _ingridientsContainer.GetChildren()){
			child.QueueFree();
		}

		bool isEnoughResources = true;
		foreach (Item input in _selectedRecepie.Input)
		{
			UIRecepieIngredient recepieIngredient = _recepieIngridientScene.Instantiate<UIRecepieIngredient>();
			_ingridientsContainer.AddChild(recepieIngredient);

			int itemsCount = _playerInventory.CountItems(input.ItemData) + _playerHotbar.CountItems(input.ItemData);

			if(itemsCount >= input.Amount * _itemsToCraft){
				recepieIngredient.Set(input.ItemData.Icon, input.ItemData.Name, input.Amount * _itemsToCraft + "/" + itemsCount, true);
			}
			else{
				recepieIngredient.Set(input.ItemData.Icon, input.ItemData.Name, input.Amount * _itemsToCraft + "/" + itemsCount, false);
				isEnoughResources = false;
			}
		}

		_craftButton.Disabled = !isEnoughResources;

		if(_itemsToCraft == 0){
			_craftButton.Disabled = true;
		}
	}

	public void Craft()
	{
		if (_selectedRecepie == null){
			return;
		}

		foreach (Item input in _selectedRecepie.Input)
		{
			if(_playerInventory.CountItems(input.ItemData) + _playerHotbar.CountItems(input.ItemData) < input.Amount * _itemsToCraft){
				return;
			}
		}

		for (int i = 0; i < _itemsToCraft; i++)
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

	public void OnInputTextChanged(string new_text){
		int caretPos = _itemsToCraftInput.CaretColumn;
		string numString = Regex.Replace(new_text.Substr(0, caretPos), @"[^0-9]", "");
		int newCaretPos = numString.Length;

		if(caretPos < new_text.Length){
			numString += Regex.Replace(new_text.Substr(caretPos, new_text.Length - caretPos), @"[^0-9]", "");
		}

		if(numString == ""){
			_itemsToCraft = 1;
		}
		else{
			_itemsToCraft = numString.ToInt();
		}
		_itemsToCraftInput.Text = numString;
		_itemsToCraftInput.CaretColumn = newCaretPos;
		Reload();
	}

	public void OnPlusButtonPressed(){
		_itemsToCraft++;
		_itemsToCraftInput.Text = _itemsToCraft.ToString();
		Reload();
	}

	public void OnMinusButtonPressed(){
		if(_itemsToCraft > 1){
			_itemsToCraft--;
			_itemsToCraftInput.Text = _itemsToCraft.ToString();
			Reload();
		}
	}
}
