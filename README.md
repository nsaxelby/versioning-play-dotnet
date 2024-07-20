### Summary
This is a project for playing around with ASP.NET's versioning capabilities.

#### Running
From root of repo: `dotnet run --project src/api/api.csproj`
API will be running at `http://localhost:5003`

It's swagger enabled, which you can access at `http://localhost:5003/swagger/`

<b>Example POST with curl:</b>

```
curl -X 'POST' \
  'http://localhost:5003/api/v1/PizzaOrder' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "cheese": true,
  "pepperoni": true,
  "tomatoSauce": true
}'
```

This will return a guid, e.g.: `e7f77197-3998-4899-bc22-02d2f0a3dbd7` (this is the pizza order ID you'll need to substitute into the GET request below)

<b>Example GET with curl:</b>

```
curl -X 'GET' \
  'http://localhost:5003/api/v1/PizzaOrder?id=e7f77197-3998-4899-bc22-02d2f0a3dbd7' \
  -H 'accept: */*'
```

  This will return your Pizza in JSON format:
  `{"cheese":true,"pepperoni":true,"tomatoSauce":true}`


  #### Testing
  There are integration tests which can be run from root:
  `dotnet test tests/IntegrationTests`


