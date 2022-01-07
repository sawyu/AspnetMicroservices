using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var ordertToUpdate = await _orderRepository.GetByIdAsync(request.Id);
            if(ordertToUpdate  == null)
            {
                //_logger.LogError("Order not exist on database.");
                throw new NotFoundException(nameof(Order), request.Id);
            }
            _mapper.Map(request, ordertToUpdate, typeof(UpdateOrderCommand), typeof(Order));
            await _orderRepository.UpdateAsync(ordertToUpdate);
            _logger.LogInformation($"Order { ordertToUpdate.Id} is successfully updated.");
            return Unit.Value;
        }
    }
}
