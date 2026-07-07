namespace Cornerstone.Ordering.Domain;

public enum OrderStatus
{
    Draft,
    Placed,
    StockReserved,
    AwaitingPayment,
    Paid,
    Cancelled
}
