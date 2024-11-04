namespace Fiap.TechChallenge.Foundation.Core.Messaging.Queries;

public interface IPagedQuery
{
    int Page { get; set; }
    int Size { get; set; }
}

public interface IOrderQuery
{
    string OrderColumn { get; set; }
    Order Order { get; set; }
}

public enum Order
{
    Asc,
    Desc
}