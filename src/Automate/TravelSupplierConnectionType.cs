using Umbraco.Automate.Core.Connections;
using Webinar.Supplier;

namespace Webinar.Automate;

/// <summary>
/// The credential, defined once. Editors set it up once, then pick it by name on
/// every step that needs it.
/// </summary>
[ConnectionType("travel.supplierPortal", "Global Travel Suppliers",
    Description = "Connect to our travel supplier network.",
    Group = "Bookings",
    Icon = "icon-handshake")]
public sealed class TravelSupplierConnectionType : ConnectionTypeBase<TravelSupplierConnectionSettings>
{
    private readonly ITravelSupplierClient _supplier;

    public TravelSupplierConnectionType(ConnectionTypeInfrastructure infrastructure, ITravelSupplierClient supplier)
        : base(infrastructure)
        => _supplier = supplier;

    public override async Task<ConnectionValidationResult> ValidateAsync(
        object? settings,
        CancellationToken cancellationToken)
    {
        if (settings is not TravelSupplierConnectionSettings { ApiKey.Length: > 0 } typed)
        {
            return ConnectionValidationResult.Failure("No API key set.");
        }

        return await _supplier.PingAsync(typed.ApiKey, cancellationToken)
            ? ConnectionValidationResult.Success("Connected to Global Travel Suppliers.")
            : ConnectionValidationResult.Failure("Global Travel Suppliers rejected the API key.");
    }
}
