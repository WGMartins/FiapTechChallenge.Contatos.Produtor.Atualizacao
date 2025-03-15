using FluentValidation;
using Moq;
using TechChallenge.Domain.RegionalAggregate;
using TechChallenge.UseCase.ContatoUseCase.Alterar;
using UseCase.Interfaces;

namespace TechChallenge.UnitTest.UseCase.ContatoUseCase.Alterar
{
    public class AlterarContatoUseCaseTest
    {
        private readonly AlterarContatoDtoBuilder _builder;        
        private readonly Mock<IMessagePublisher> _messagePublisher;
        private readonly IValidator<AlterarContatoDto> _validator;
        private readonly IAlterarContatoUseCase _alterarContatoUseCase;

        public AlterarContatoUseCaseTest()
        {
            _validator = new AlterarContatoValidator();
            _messagePublisher = new Mock<IMessagePublisher>();
            _builder = new AlterarContatoDtoBuilder();
            _alterarContatoUseCase = new AlterarContatoUseCase(_validator, _messagePublisher.Object);
        }

        [Fact]
        public void AlterarContatoUseCase_Alterar_Sucesso()
        {
            // Arrange
            var alterarContatoDto = _builder.Default().Build();

            _messagePublisher.Setup(s => s.PublishAsync(It.IsAny<Contato>()));           

            // Act
            _alterarContatoUseCase.Alterar(alterarContatoDto);

            // Assert            
            _messagePublisher.Verify(x => x.PublishAsync(It.IsAny<Contato>()), Times.Once());            

        }
        
        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        public void AlterarContatoUseCase_Alterar_ErroValidacaoNome(string nome)
        {
            // Arrange
            var alterarContatoDto = _builder.Default().WithName(nome).Build();

            _messagePublisher.Setup(s => s.PublishAsync(It.IsAny<Contato>()));

            // Act
            var result = Assert.Throws<Exception>(() => _alterarContatoUseCase.Alterar(alterarContatoDto));

            // Assert
            Assert.Contains("Nome não pode ser nulo ou vazio", result.Message);
        }

        [Theory]        
        [InlineData("")]
        [InlineData("     ")]
        public void AlterarContatoUseCase_Alterar_ErroValidacaoTelefoneNuloVazio(string telefone)
        {
            // Arrange
            var alterarContatoDto = _builder.Default().WithTelefone(telefone).Build();

            _messagePublisher.Setup(s => s.PublishAsync(It.IsAny<Contato>()));

            // Act
            var result = Assert.Throws<Exception>(() => _alterarContatoUseCase.Alterar(alterarContatoDto));

            // Assert
            Assert.Contains("Telefone não pode ser nulo ou vazio", result.Message);

        }

        [Theory]
        [InlineData("1234598-8546789")]        
        public void AlterarContatoUseCase_Alterar_ErroValidacaoTelefoneTamanho(string telefone)
        {
            // Arrange
            var alterarContatoDto = _builder.Default().WithTelefone(telefone).Build();

            _messagePublisher.Setup(s => s.PublishAsync(It.IsAny<Contato>()));

            // Act
            var result = Assert.Throws<Exception>(() => _alterarContatoUseCase.Alterar(alterarContatoDto));

            // Assert
            Assert.Contains("Foi atingido o número máximo de caracteres (10)", result.Message);
        }

        [Theory]        
        [InlineData("08645-6441")]
        [InlineData("34887037")]
        [InlineData("999999999")]        
        public void AlterarContatoUseCase_Alterar_ErroValidacaoTelefoneFormato(string telefone)
        {
            // Arrange
            var alterarContatoDto = _builder.Default().WithTelefone(telefone).Build();

            _messagePublisher.Setup(s => s.PublishAsync(It.IsAny<Contato>()));

            // Act
            var result = Assert.Throws<Exception>(() => _alterarContatoUseCase.Alterar(alterarContatoDto));

            // Assert
            Assert.Contains("Telefone inválido", result.Message);
        }

        [Theory]        
        [InlineData("")]
        [InlineData("     ")]        
        public void AlterarContatoUseCase_Alterar_ErroValidacaoEmailNuloVazio(string email)
        {
            // Arrange
            var alterarContatoDto = _builder.Default().WithEmail(email).Build();

            _messagePublisher.Setup(s => s.PublishAsync(It.IsAny<Contato>()));

            // Act
            var result = Assert.Throws<Exception>(() => _alterarContatoUseCase.Alterar(alterarContatoDto));

            // Assert
            Assert.Contains("Email não pode ser nulo ou vazio", result.Message);
        }

        [Theory]        
        [InlineData("testetestetestetestetestetestetestetestetestetestetestetestetestetestetestetestetestetestetesteteste" +
            "@testetestetestetestetestetestetestetestetestetestetesteteste")]
        public void AlterarContatoUseCase_Alterar_ErroValidacaoEmailTamanho(string email)
        {
            // Arrange
            var alterarContatoDto = _builder.Default().WithEmail(email).Build();

            _messagePublisher.Setup(s => s.PublishAsync(It.IsAny<Contato>()));

            // Act
            var result = Assert.Throws<Exception>(() => _alterarContatoUseCase.Alterar(alterarContatoDto));

            // Assert
            Assert.Contains("Foi atingido o número máximo de caracteres (150)", result.Message);
        }

        [Theory]
        [InlineData("testedeemail")]
        [InlineData("email@@gmail.com")]
        [InlineData("teste@live")]        
        public void AlterarContatoUseCase_Alterar_ErroValidacaoEmailFormato(string email)
        {
            // Arrange
            var alterarContatoDto = _builder.Default().WithEmail(email).Build();

            _messagePublisher.Setup(s => s.PublishAsync(It.IsAny<Contato>()));

            // Act
            var result = Assert.Throws<Exception>(() => _alterarContatoUseCase.Alterar(alterarContatoDto));

            // Assert
            Assert.Contains("E-mail inválido", result.Message);
        }
    }
}
