using ItemCatalog.Data;
using ItemCatalog.Models;

namespace ItemCatalog;

public partial class MainPage : ContentPage
{
	private readonly ItemRepository _itemRepository;

	public MainPage(ItemRepository itemRepository)
	{
		InitializeComponent();
		_itemRepository = itemRepository;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		LoadItems();
	}

	private async void OnSaveClicked(object? sender, EventArgs e)
	{
		var itemId = ItemIdEntry.Text?.Trim();
		var itemName = ItemNameEntry.Text?.Trim();
		var itemDescription = ItemDescriptionEditor.Text?.Trim();

		if (string.IsNullOrWhiteSpace(itemId) ||
			string.IsNullOrWhiteSpace(itemName) ||
			string.IsNullOrWhiteSpace(itemDescription))
		{
			await DisplayAlertAsync("Missing information", "Please provide an Item ID, Item Name, and Item Description.", "OK");
			return;
		}

		_itemRepository.AddItem(new Item
		{
			ItemId = itemId,
			Name = itemName,
			Description = itemDescription
		});

		LoadItems();
		ItemIdEntry.Text = string.Empty;
		ItemNameEntry.Text = string.Empty;
		ItemDescriptionEditor.Text = string.Empty;
		await DisplayAlertAsync("Saved", "The item was added successfully.", "OK");
	}

	private void LoadItems()
	{
		ItemsCollectionView.ItemsSource = _itemRepository.GetAllItems();
	}
}
