using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using AdressAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AddressesDatabaseSettings>(builder.Configuration.GetSection("AddressesDatabaseSettings"));
builder.Services.AddSingleton<AddressesService>();
var app = builder.Build();

/// <summary>
/// Root endpoint
/// </summary>
app.MapGet("/", () => "Address API!");

/// <summary>
/// Get all addresses
/// </summary>
app.MapGet("/api/Address", async (AddressesService addressesService) => await addressesService.Get());

/// <summary>
/// Get an address by id
/// </summary>
app.MapGet("/api/Address/{id}", async (AddressesService addressesService, string id) =>
{
    var address = await addressesService.Get(id);
    return address is null ? Results.NotFound() : Results.Ok(address);
});

/// <summary>
/// Create a new address
/// </summary>
app.MapPost("/api/Address", async (AddressesService addressesService, Address address) =>
{
    await addressesService.Create(address);
    return Results.Ok();
});

/// <summary>
/// Update an address
/// </summary>
app.MapPut("/api/Address/{id}", async (AddressesService addressesService, string id, Address updatedAddress) =>
{
    var address = await addressesService.Get(id);
    if (address is null) return Results.NotFound();

    updatedAddress.Id = address.Id;
    await addressesService.Update(id, updatedAddress);

    return Results.NoContent();
});

/// <summary>
/// Delete an address
/// </summary>
app.MapDelete("/api/Address/{id}", async (AddressesService addressesService, string id) =>
{
    var address = await addressesService.Get(id);
    if (address is null) return Results.NotFound();

    await addressesService.Remove(address.Id);

    return Results.NoContent();
});

app.Run();
