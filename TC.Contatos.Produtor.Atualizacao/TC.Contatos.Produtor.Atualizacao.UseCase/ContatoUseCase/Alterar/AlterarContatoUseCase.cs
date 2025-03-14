using FluentValidation;
using TechChallenge.Domain.RegionalAggregate;
using UseCase.Interfaces;

namespace TechChallenge.UseCase.ContatoUseCase.Alterar
{
    public class AlterarContatoUseCase : IAlterarContatoUseCase    {
        
        private readonly IValidator<AlterarContatoDto> _validator;
        private readonly IMessagePublisher _messagePublisher;

        public AlterarContatoUseCase(IValidator<AlterarContatoDto> validator, IMessagePublisher messagePublisher)
        {            
            _validator = validator;
            _messagePublisher = messagePublisher;
        }

        public void Alterar(AlterarContatoDto alterarContatoDto)
        {
            var validacao = _validator.Validate(alterarContatoDto);
            if (!validacao.IsValid)
            {
                string mensagemValidacao = string.Empty;
                foreach (var item in validacao.Errors)
                {
                    mensagemValidacao = string.Concat(mensagemValidacao, item.ErrorMessage, ", ");
                }

                throw new Exception(mensagemValidacao.Remove(mensagemValidacao.Length-2));
            }

            var contato = Contato.Criar(alterarContatoDto.Id, alterarContatoDto.Nome, alterarContatoDto.Telefone, alterarContatoDto.Email, alterarContatoDto.RegionalId);            

            _messagePublisher.PublishAsync(contato);

        }
    }
}
