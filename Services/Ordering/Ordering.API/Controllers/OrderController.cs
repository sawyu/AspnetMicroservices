using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.DeleteOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _meditor;

        public OrderController(IMediator meditor)
        {
            _meditor = meditor ?? throw new ArgumentNullException(nameof(meditor));
        }
        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var orders = await _meditor.Send(query);
            return Ok(orders); 
        }
        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
           var result = await _meditor.Send(command);
            return Ok(result);
        }
        [HttpPut(Name ="UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] UpdateOrderCommand command)
        {
            await _meditor.Send(command);
            return NoContent();
        }
        [HttpDelete("{id}",Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand() { Id = id };
            await _meditor.Send(command);
            return NoContent();
        }
    }
}
