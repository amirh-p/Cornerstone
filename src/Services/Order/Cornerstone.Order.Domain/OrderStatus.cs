namespace Cornerstone.Order.Domain;

public enum OrderStatus
{
    Draft,
    Placed,
    StockReserved,
    AwaitingPayment,
    Paid,
    Cancelled
}
