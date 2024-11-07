using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock_Backend.Application;

namespace Stock_Backend.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class ProductController :ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController( IMediator mediator )
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateProductCommand command )
        {
            var result = await _mediator.Send(command);

            if( !result.Success )
                return BadRequest( result );

            return Ok( result );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> Get( int id )
        {
            var query = new GetProductByIdCommand(id);

            var result = await _mediator.Send(query);

            if( result == null )
                return NotFound( ReturnCommon.FailureMessage( "Produto não localizado." ) );

            return Ok( result );
        }

        [HttpGet( "all" )]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllProductsCommand();

            var result = await _mediator.Send(query);

            if( result == null )
                return NotFound( ReturnCommon.FailureMessage( "Produtos não localizados." ) );

            return Ok( result );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateProductCommand command )
        {
            if( id != command.Id )
                return BadRequest( ReturnCommon.FailureMessage( "ID do produto inválido." ) );

            var result = await _mediator.Send(command);

            if( result.Success )
                return Ok( result );

            return BadRequest( result );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id )
        {
            var command = new DeleteProductCommand(id);

            var result = await _mediator.Send(command);

            if( result.Success )
                return Ok( result );

            return BadRequest( result );
        }
    }
}
