using Microsoft.AspNetCore.Mvc;
using UseCase.ContatoUseCase.Alterar;
using UseCase.Interfaces;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IAlterarContatoUseCase _alterarContatoUseCase;

        public ContatoController(IAlterarContatoUseCase alterarContatoUseCase)
        {
            _alterarContatoUseCase = alterarContatoUseCase;
        }


        [HttpPut]
        public IActionResult Alterar(AlterarContatoDto alterarContatoDto)
        {
            try
            {
                _alterarContatoUseCase.Alterar(alterarContatoDto);
                return Accepted();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
