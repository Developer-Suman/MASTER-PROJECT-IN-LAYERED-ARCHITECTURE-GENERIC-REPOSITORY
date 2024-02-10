using Master_BLL.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MASTER_PROJECT_IN_LAYERED_ARCHITECTURE_GENERIC_REPOSITORY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesRepository _articlesRepository;


        public ArticlesController(IArticlesRepository articlesRepository, IMemoryCacheRepository memoryCacheRepository)
        {
            _articlesRepository = articlesRepository;
     
            
        }

        [HttpGet("GetArticlesById")]
        public async Task<IActionResult> GetArticlesById(Guid ArticlesId)
        {
            
            var articles = await _articlesRepository.GetArticlesById(ArticlesId);
            if(articles.Data is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new {articles.Errors});
            }

            return Ok(articles.Data);

        }

        [HttpGet("GetAllArticles")]
        public async Task<IActionResult> GetAllArticles(int page, int pageSize)
        {
            var articles = await _articlesRepository.GetAllArticles(page, pageSize);
            if(articles.Data is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new {articles.Errors});
            }
            return Ok(articles.Data);
        }

        [HttpGet("GetArticlesWithComments")]
        public IActionResult GetArticlesWithComments(int page, int pageSize)
        {
            var articlesWithComments = _articlesRepository.GetArticlesWithComments(page, pageSize);
            return Ok(articlesWithComments.Data);
        }

    }
}
