# Remote Data Storage Lab (macOS)

This project matches the assignment objective: create a RESTful web service and update the Week 3 MAUI app to use remote data storage instead of local SQLite.

The API stores `Person` records in SQLite and exposes them through `/api/Person`.

## Run the web service

From this folder:

```sh
cd /Users/amber/School/maui/week4/RemoteDataStorage
dotnet run --project RemoteDataStorage/RemoteDataStorage.csproj --launch-profile http
```

Then open:

```text
http://localhost:5289/swagger
```

## Test the API

```sh
curl http://localhost:5289/api/Person

curl -X POST http://localhost:5289/api/Person \
  -H 'Content-Type: application/json' \
  -d '{"firstName":"James","lastName":"Smith","doB":"2004-06-15"}'

curl -X DELETE http://localhost:5289/api/Person/1
```

## App connection

The MAUI app in [MAUI_Data_Access](MAUI_Data_Access) has been updated to use HTTP calls to the API rather than a local database.

The database file for the web service is created under the API project output folder during runtime.