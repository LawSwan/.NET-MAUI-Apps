using System.Collections.ObjectModel;
using AssessmentApp.Models;
using AssessmentApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AssessmentApp.ViewModels;

public partial class ItemsViewModel(ApiService apiService) : ObservableObject
{
    [ObservableProperty] private string itemId = string.Empty;
    [ObservableProperty] private string itemName = string.Empty;
    [ObservableProperty] private string itemDescription = string.Empty;
    [ObservableProperty] private string message = string.Empty;
    [ObservableProperty] private bool isBusy;

    public string DisplayName => AppSettings.DisplayName;
    public ObservableCollection<Item> Items { get; } = [];

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

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(ItemId) || string.IsNullOrWhiteSpace(ItemName) || string.IsNullOrWhiteSpace(ItemDescription))
        {
            Message = "Please provide Item ID, Item Name, and Item Description.";
            return;
        }

        IsBusy = true;
        Message = string.Empty;
        try
        {
            var saved = await apiService.AddItemAsync(new Item { Id = ItemId.Trim(), Name = ItemName.Trim(), Description = ItemDescription.Trim() });
            Message = saved ? "Item saved." : "The item could not be saved.";
            if (saved)
            {
                ItemId = string.Empty;
                ItemName = string.Empty;
                ItemDescription = string.Empty;
                await RefreshItemsAsync();
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

    private async Task RefreshItemsAsync()
    {
        Items.Clear();
        foreach (var item in await apiService.GetItemsAsync())
        {
            Items.Add(item);
        }
    }
}