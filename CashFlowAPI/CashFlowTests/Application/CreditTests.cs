using Application.UseCases.Credit.Commands.AddCreditCommand;
using CashFlowAPI.Controllers;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.MessageBroker;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CashFlowTests.Application
{
    public class CreditTests
    {
        [Fact]
        public async Task Handle_ShouldPublishOperation()
        {
            // Arrange
            var mockRabbitMqService = new Mock<IRabbitMqService>();
            var handler = new AddCreditHandler(mockRabbitMqService.Object);

            var command = new AddCreditCommand
            {
                Value = 100,
                Description = "Teste de crédito"
            };

            // Act
            await handler.Handle(command, default);

            // Assert
            mockRabbitMqService.Verify(s => s.PublishOperation(It.Is<OperationEntity>(o =>
                o.Value == 100 &&
                o.Description == "Teste de crédito" &&
                o.Type == CashFlowType.Credit
            )), Times.Once);
        }

        [Fact]
        public async Task AddCredit_ShouldCallMediatorSend()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new CreditController(mockMediator.Object);

            var command = new AddCreditCommand
            {
                Value = 100m,
                Description = "Teste de crédito"
            };

            // Act
            var result = await controller.AddCredit(command);

            // Assert
            mockMediator.Verify(m => m.Send(It.Is<AddCreditCommand>(c =>
                c.Value == 100m &&
                c.Description == "Teste de crédito"
            ), default), Times.Once);

            Assert.IsType<OkResult>(result);
        }
    }
}
