using System;
using System.Linq;
using System.Text.RegularExpressions;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class UICraftingMenu : Control, IUIElement
{
	private GridContainer _itemsGrid;
	private Button _craftButton;
	private LineEdit _itemsToCraftInput;
	private int _itemsToCraft = 1;
	private TextureRect _recepieIcon;
	private Label _recepieName;
	private Label _recepieDescription;
	private VBoxContainer _ingridientsContainer;

	private CraftingRecepie _selectedRecepie;

	private InventorySystem _playerInventory;
	private RecepieManager _recepieManager;
	private PackedScene _recepieIngridientScene = GD.Load<PackedScene>("res://Scenes/UI/UIRecepieIngredient.tscn");

	[Export] private InputEventAction _inputEventAction;
	[Export] private bool _isActiveOnStart;

	public InputEventAction InputEventAction => _inputEventAction;
	public bool IsActiveOnStart => _isActiveOnStart;

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

		_playerInventory = ((InventorySystemGroup)ReferenceCenter.Player.FindChild("InventoryGroup")).GetInventory();

		_recepieManager = GetNode<RecepieManager>("/root/RecepieManager");

		foreach (Node child in _itemsGrid.GetChildren())
		{
			child.QueueFree();
		}

		foreach (CraftingRecepie recepie in _recepieManager.CraftingRecepies)
		{
			UICraftingRecepie uICraftingRecepie = new UICraftingRecepie(recepie);
			_itemsGrid.AddChild(uICraftingRecepie);
			uICraftingRecepie.OnRecepieClicked += () =>
			{
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
		foreach (Control child in _ingridientsContainer.GetChildren())
		{
			child.QueueFree();
		}

		bool isEnoughResources = true;
		foreach (Item input in _selectedRecepie.Input)
		{
			UIRecepieIngredient recepieIngredient = _recepieIngridientScene.Instantiate<UIRecepieIngredient>();
			_ingridientsContainer.AddChild(recepieIngredient);

			int itemsCount = _playerInventory.CountItems(input.ItemData);

			if (itemsCount >= input.Amount * _itemsToCraft)
			{
				recepieIngredient.Set(input.ItemData.Icon, input.ItemData.Name, (input.Amount * _itemsToCraft) + "/" + itemsCount, true);
			}
			else
			{
				recepieIngredient.Set(input.ItemData.Icon, input.ItemData.Name, (input.Amount * _itemsToCraft) + "/" + itemsCount, false);
				isEnoughResources = false;
			}
		}

		_craftButton.Disabled = !isEnoughResources;

		if (_itemsToCraft == 0)
		{
			_craftButton.Disabled = true;
		}
	}

	public void Craft()
	{
		if (_selectedRecepie == null)
		{
			return;
		}

		foreach (Item input in _selectedRecepie.Input)
		{
			if (_playerInventory.CountItems(input.ItemData) < input.Amount * _itemsToCraft)
			{
				return;
			}
		}

		for (int i = 0; i < _itemsToCraft; i++)
		{
			foreach (Item input in _selectedRecepie.Input)
			{
				_playerInventory.RemoveItems(input.ItemData, input.Amount);
			}

			int amountCrafted = _selectedRecepie.Output.Amount;
			int availableSpace = _playerInventory.CountAvailableItemSpace(_selectedRecepie.Output.ItemData);

			if (availableSpace >= amountCrafted)
			{
				_playerInventory.AddItems(_selectedRecepie.Output.ItemData, amountCrafted);
			}
			else
			{
				_playerInventory.AddItems(_selectedRecepie.Output.ItemData, availableSpace);
				amountCrafted -= availableSpace;
				throw new NotImplementedException("UICraftingMenu._craftButton.Pressed: Drop item.");
			}
		}
	}

	public void OnInputTextChanged(string new_text)
	{
		GD.Print("Text");
		int caretPos = _itemsToCraftInput.CaretColumn;
		string numString = Regex.Replace(new_text.Substr(0, caretPos), @"[^0-9]", "");
		int newCaretPos = numString.Length;

		if (caretPos < new_text.Length)
		{
			numString += Regex.Replace(new_text.Substr(caretPos, new_text.Length - caretPos), @"[^0-9]", "");
		}

		if (numString == "")
		{
			_itemsToCraft = 1;
		}
		else
		{
			_itemsToCraft = numString.ToInt();
		}
		_itemsToCraftInput.Text = numString;
		_itemsToCraftInput.CaretColumn = newCaretPos;
		Reload();
	}

	public void OnPlusButtonPressed()
	{
		_itemsToCraft++;
		_itemsToCraftInput.Text = _itemsToCraft.ToString();
		Reload();
	}

	public void OnMinusButtonPressed()
	{
		if (_itemsToCraft > 1)
		{
			_itemsToCraft--;
			_itemsToCraftInput.Text = _itemsToCraft.ToString();
			Reload();
		}
	}

	public void Close()
	{
		Visible = false;
	}

	public void Open()
	{
		Visible = true;
	}
}
