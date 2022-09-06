﻿using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using DevagramCSharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicacaoController : BaseController
    {
        private readonly ILogger<PublicacaoController> _logger;
        private readonly IPublicacaoRepository _publicacaoRepository;

        public PublicacaoController(ILogger<PublicacaoController> logger,
            IPublicacaoRepository publicacaoRepository, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
        }

        [HttpPost]
        public IActionResult Publicar([FromForm] PublicacaoRequisicaoDto publicacaodto)
        {
            try
            {
                Usuario usuario = LerToken();
                CosmicService cocosmicservice = new CosmicService();
                if (publicacaodto != null)
                {
                    if (String.IsNullOrEmpty(publicacaodto.Descricao) &&
                        String.IsNullOrWhiteSpace(publicacaodto.Descricao))
                    {
                        _logger.LogError("A descrição está inválida");
                        return BadRequest("É obrigatório a descrição na publicação");
                    }
                    if(publicacaodto.Foto == null)
                    {
                        _logger.LogError("A foto está inválida");
                        return BadRequest("É obrigatório a foto na publicação");
                    }
                    Publicacao publicacao = new Publicacao()
                    {
                        Descricao = publicacaodto.Descricao,
                        IdUsuario = usuario.Id,
                        Foto = cocosmicservice.EnviarImagem(new ImagemDto { Imagem = publicacaodto.Foto, Nome = "publicacao" })
                    };
                    _publicacaoRepository.Publicar(publicacao);
                }

                return Ok("Publicação salva com sucesso!");
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro na publicação: " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorRepostaDto()
                {
                    Descricao = "Ocorreu um erro ao fazer o publicação",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

    }
}