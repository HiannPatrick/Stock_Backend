using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock_Backend.Application;
using Stock_Backend.Domain;

namespace Stock_Backend.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class ProductMovement :ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductMovement( IMediator mediator )
        {
            _mediator = mediator;
        }

        [HttpPut( "setMovement" )]
        public async Task<IActionResult> Update( [FromBody] ProductMovementCommand command )
        {
            if( command is null )
                return BadRequest( ReturnCommon.FailureMessage( "Comando inválido." ) );

            var result = await _mediator.Send(command);

            if( result.Success )
                return Ok( result );

            return BadRequest( result );
        }

        [HttpGet( "dailyMovement" )]
        public async Task<ActionResult<List<ProductMovementDto>>> GetDailyProductMovement( [FromQuery] DateTime date )
        {
            var query = new DailyProductMovementRequestQuery(date);

            var result = await _mediator.Send(query);

            if( result == null || result.Count == 0 )
                return NotFound( "Sem movimentação de produtos na data selecionada!" );

            return Ok( result );
        }
    }
}
