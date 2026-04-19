namespace OrderService.Api.Constracts.Responses
{
    public sealed record ErrorResponse(
        string Code,
        string Message
    );
}
