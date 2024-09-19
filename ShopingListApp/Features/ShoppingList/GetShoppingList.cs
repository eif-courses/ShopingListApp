using FastEndpoints;

namespace ShopingListApp.Features.ShoppingList;

public class GetShoppingList : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/list/get-shopping-list");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {

        await SendOkAsync("Hello",ct);

        //return base.HandleAsync(ct);
    }
}