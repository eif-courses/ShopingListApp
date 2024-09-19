using FastEndpoints;
using ShopingListApp.Data;
using ShopingListApp.Entities;

namespace ShopingListApp.Features.ShoppingList;

public class GetShoppingList : EndpointWithoutRequest<List<Item>>
{
    private readonly MyDb _dbContext;
    public GetShoppingList(MyDb dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public override void Configure()
    {
        Get("/list/get-shopping-list");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var items = _dbContext.Items.ToList();
        
        await SendOkAsync(items,ct);
    }
}