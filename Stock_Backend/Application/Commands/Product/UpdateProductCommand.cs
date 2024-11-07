using MediatR;

namespace Stock_Backend.Application
{
    public record UpdateProductCommand( int Id, string Name, string PartNumber, decimal AverageCostPrice ) :IRequest<ReturnCommon>;

}