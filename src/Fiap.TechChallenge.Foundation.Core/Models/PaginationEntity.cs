namespace Fiap.TechChallenge.Foundation.Core.Models;

public class PaginationEntity
{
    public PaginationEntity()
    {
    }

    public PaginationEntity(long totalRecords, int currentPage, int pageSize)
    {
        TotalRecords = totalRecords;
        CurrentPage = currentPage;
        PageSize = pageSize;
        HasPreviousPage = currentPage > 1;
        HasNextPage = !(pageSize * currentPage > totalRecords);
    }

    /// <summary>
    ///     Número total de registros na resposta da requisição.
    /// </summary>
    public long TotalRecords { get; set; }

    /// <summary>
    ///     Número da página atual, utilizado para paginação da resposta da requisição.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    ///     Quantidade máxima de registros por página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    ///     Indica se existe página anterior.
    /// </summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    ///     Indica se existe próxima página.
    /// </summary>
    public bool HasNextPage { get; set; }
}