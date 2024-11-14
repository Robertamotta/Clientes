using Clientes.Api.Controllers;
using Clientes.Dominio.DTOs;
using Clientes.Dominio.Entidades;
using Clientes.Dominio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Clientes.Testes.Controller;

public class ClienteControllerTestes
{
    private readonly Mock<ILogger<ClienteController>> loggerMock = new();
    private readonly Mock<IClienteServico> clienteServicoMock = new();
    private readonly ClienteController clienteController;

    public ClienteControllerTestes()
    {
        clienteController = new ClienteController(loggerMock.Object, clienteServicoMock.Object);
    }

    [Fact]
    public async Task Listar_Sucesso()
    {
        // Arrange
        var clientes = new List<Cliente> { new() };
        clienteServicoMock.Setup(servico => servico.ListarClientes()).ReturnsAsync(clientes);

        // Act
        var resposta = await clienteController.Listar() as OkObjectResult;

        // Assert
        Assert.Equal((int)HttpStatusCode.OK, resposta.StatusCode);
        clienteServicoMock.Verify(x => x.ListarClientes(), Times.Once());
    }

    [Fact]
    public async Task Obter_Sucesso()
    {
        // Arrange
        var cliente = new Cliente { Id = 1 };
        clienteServicoMock.Setup(servico => servico.ObterCliente(1)).ReturnsAsync(cliente);

        // Act
        var resultado = await clienteController.Obter(1) as OkObjectResult;

        // Assert
        Assert.Equal((int)HttpStatusCode.OK, resultado.StatusCode);
        clienteServicoMock.Verify(x => x.ObterCliente(1), Times.Once);
    }

    [Fact]
    public async Task Obter_ClienteInexistente_NotFound()
    {
        // Arrange
        clienteServicoMock.Setup(servico => servico.ObterCliente(1)).ReturnsAsync((Cliente)null);

        // Act
        var resultado = await clienteController.Obter(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        clienteServicoMock.Verify(x => x.ObterCliente(1), Times.Once);
    }

    [Fact]
    public async Task Cadastrar_Sucesso()
    {
        // Arrange
        var cliente = new ClienteDto { Nome = "Nome" };
        clienteServicoMock.Setup(servico => servico.CadastrarCliente(cliente)).Returns(Task.CompletedTask);

        // Act
        var result = await clienteController.Cadastrar(cliente);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedCliente = Assert.IsType<ClienteDto>(createdAtActionResult.Value);
        Assert.Equal(cliente, returnedCliente);
        clienteServicoMock.Verify(x => x.CadastrarCliente(cliente), Times.Once);

    }

    [Fact]
    public async Task Cadastrar_Erro()
    {
        // Arrange
        var cliente = new ClienteDto { Nome = "Nome" };
        clienteServicoMock.Setup(servico => servico.CadastrarCliente(cliente)).ThrowsAsync(new Exception("Erro interno"));

        // Act
        var resultado = await clienteController.Cadastrar(cliente);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal("Ocorreu um erro ao processar a solicitação. Erro interno", objectResult.Value);
        clienteServicoMock.Verify(x => x.CadastrarCliente(cliente), Times.Once);
    }

    [Fact]
    public async Task Atualizar_Sucesso()
    {
        // Arrange
        var cliente = new Cliente { Id = 1 };
        clienteServicoMock.Setup(servico => servico.ObterCliente(1)).ReturnsAsync(cliente);

        // Act
        var resposta = await clienteController.Atualizar(cliente) as OkResult;

        // Assert
        Assert.Equal((int)HttpStatusCode.OK, resposta.StatusCode);
        clienteServicoMock.Verify(x => x.AtualizarCliente(cliente), Times.Once());
    }
       
    [Fact]
    public async Task Atualizar_Erro()
    {
        // Arrange
        var cliente = new Cliente { Id = 1 };
        clienteServicoMock.Setup(servico => servico.ObterCliente(1)).ThrowsAsync(new Exception("Erro interno"));

        // Act
        var resultado = await clienteController.Atualizar(cliente);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.Equal("Ocorreu um erro ao processar a solicitação. Erro interno", objectResult.Value);

        clienteServicoMock.Verify(x => x.ObterCliente(1), Times.Once);
        clienteServicoMock.Verify(x => x.AtualizarCliente(cliente), Times.Never);
    }

    [Fact]
    public async Task Delete_Sucesso()
    {
        // Act
        var resposta = await clienteController.Delete(1) as OkResult;

        // Assert
        Assert.Equal((int)HttpStatusCode.OK, resposta.StatusCode);
        clienteServicoMock.Verify(x => x.RemoverCliente(1), Times.Once);
    }
}
