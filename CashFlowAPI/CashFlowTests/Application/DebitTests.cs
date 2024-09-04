using Application.UseCases.Debit.Commands.AddDebitCommand;
using CashFlowAPI.Controllers;
using Domain.Entities;
using Infrastructure.MessageBroker;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CashFlowTests.Application
{
    public class DebitTests
    {
        [Fact]
        public async Task Handle_ShouldPublishOperation()
        {
            // Arrange
            var mockRabbitMqService = new Mock<IRabbitMqService>();
            var handler = new AddDebitHandler(mockRabbitMqService.Object);

            var command = new AddDebitCommand
            {
                Value = 50m,
                Description = "Débito"
            };

            // Act
            await handler.Handle(command, default);

            // Assert
            mockRabbitMqService.Verify(s => s.PublishOperation(It.Is<OperationEntity>(o =>
                o.Value == 50m &&
                o.Description == "Débito" &&
                o.Type == Domain.Enums.CashFlowType.Debit
            )), Times.Once);
        }

        [Fact]
        public async Task Post_ShouldCallMediatorSend()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new DebitController(mockMediator.Object);

            var command = new AddDebitCommand
            {
                Value = 50m,
                Description = "Débito"
            };

            // Act
            var result = await controller.Post(command);

            // Assert
            mockMediator.Verify(m => m.Send(It.Is<AddDebitCommand>(c =>
                c.Value == 50m &&
                c.Description == "Débito"
            ), default), Times.Once);

            Assert.IsType<OkResult>(result);
        }
    }
}
