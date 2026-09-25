using System.Collections.ObjectModel;
using System.Net;
using AssessmentApp.Models;
using AssessmentApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AssessmentApp.ViewModels;

/// <summary>
/// Data and commands for the data entry page (ItemsPage). The [ObservableProperty] fields
/// generate bindable properties and the [RelayCommand] methods generate LoadCommand and SaveCommand.
/// </summary>
/// <param name="apiService">Used to load and save items through the web service.</param>
public partial class ItemsViewModel(ApiService apiService) : ObservableObject
{
    /// <summary>Text in the "Item ID" entry.</summary>
    [ObservableProperty] private string itemId = string.Empty;

    /// <summary>Text in the "Item Name" entry.</summary>
    [ObservableProperty] private string itemName = string.Empty;

    /// <summary>Text in the "Item Description" editor.</summary>
    [ObservableProperty] private string itemDescription = string.Empty;

    /// <summary>Status or warning message shown at the bottom of the page.</summary>
    [ObservableProperty] private string message = string.Empty;

    /// <summary>True while waiting on the web service; shows the activity indicator.</summary>
    [ObservableProperty] private bool isBusy;

    /// <summary>Name shown at the top of the page.</summary>
    public string DisplayName => AppSettings.DisplayName;

    /// <summary>Items retrieved from the web service, shown in the list on the page.</summary>
    public ObservableCollection<Item> Items { get; } = [];

    /// <summary>
    /// Loads the stored items when the page appears.
    /// </summary>
    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            await RefreshItemsAsync();
        }
        catch (HttpRequestException)
        {
            Message = "Could not load items from the web service.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Save button: warns if any field is empty; otherwise saves the item through the
    /// web service, clears the fields and reloads the list of stored items.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
        {
            return;
        }

        // All three fields are required before anything is sent to the web service.
        if (string.IsNullOrWhiteSpace(ItemId) || string.IsNullOrWhiteSpace(ItemName) || string.IsNullOrWhiteSpace(ItemDescription))
        {
            Message = "Please provide Item ID, Item Name, and Item Description.";
            return;
        }

        IsBusy = true;
        Message = string.Empty;
        try
        {
            var item = new Item { Id = ItemId.Trim(), Name = ItemName.Trim(), Description = ItemDescription.Trim() };
            var status = await apiService.AddItemAsync(item);

            switch (status)
            {
                case HttpStatusCode.OK:
                    Message = "Item saved.";
                    ItemId = string.Empty;
                    ItemName = string.Empty;
                    ItemDescription = string.Empty;
                    await RefreshItemsAsync();
                    break;
                case HttpStatusCode.Conflict:
                    Message = $"An item with ID '{item.Id}' already exists. Please use a different Item ID.";
                    break;
                default:
                    Message = "The item could not be saved.";
                    break;
            }
        }
        catch (HttpRequestException)
        {
            Message = "Could not save the item to the web service.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Replaces the displayed list with all items currently stored by the web service.
    /// </summary>
    private async Task RefreshItemsAsync()
    {
        var storedItems = await apiService.GetItemsAsync();
        Items.Clear();
        foreach (var item in storedItems)
        {
            Items.Add(item);
        }
    }
}
