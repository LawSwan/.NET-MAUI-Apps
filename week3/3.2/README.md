# MAUI Data Access on macOS

This version of the assignment is configured to run as a Mac Catalyst app. Windows Machine and Android-specific steps are intentionally omitted.

## Run on a Mac

1. Open the `MAUI_Data_Access` folder in VS Code.
2. Make sure Xcode is installed and selected in **Xcode > Settings > Locations**.
3. Restore dependencies:

   ```bash
   dotnet restore MAUI_Data_Access/MAUI_Data_Access.csproj
   ```

4. Build for Mac Catalyst:

   ```bash
   dotnet build MAUI_Data_Access/MAUI_Data_Access.csproj -f net10.0-maccatalyst
   ```

5. Run the app from VS Code by selecting the Mac Catalyst target, or use:

   ```bash
   dotnet build MAUI_Data_Access/MAUI_Data_Access.csproj -t:Run -f net10.0-maccatalyst
   ```

## Assignment translation

- Use Mac Catalyst instead of Windows Machine.
- Ignore Windows emulator and Android emulator instructions.
- Complete the SQLite package, model, data-access, XAML, validation, and save/list requirements exactly as shown.
- Test by entering a first name, last name, and date of birth, then selecting **Save Person**.
- Confirm saved people appear in the list after the app is reopened.

The database is stored in the app's Mac Catalyst application-data directory as `PeopleSQLite.db3`.
