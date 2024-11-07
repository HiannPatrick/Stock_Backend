using MediatR;

namespace Stock_Backend.Application
{
    public record CreateProductCommand( string Name, string PartNumber, decimal AverageCostPrice ) :IRequest<ReturnCommon>;
}
