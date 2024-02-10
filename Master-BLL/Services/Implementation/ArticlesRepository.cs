using AutoMapper;
using Master_BLL.DTOs.Articles;
using Master_BLL.DTOs.RegistrationDTOs;
using Master_BLL.Services.Interface;
using Master_BLL.Static.Cache;
using Master_DAL.Abstraction;
using Master_DAL.DbContext;
using Master_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master_BLL.Services.Implementation
{
    public class ArticlesRepository : IArticlesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public ArticlesRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _context = applicationDbContext;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<ArticlesGetDTOs> DeleteArticles(Guid ArticlesId)
        {
            throw new NotImplementedException();

        }

        public async Task<Result<List<ArticlesGetDTOs>>> GetAllArticles(int page, int pageSize)
        {
            try
            {
                var cacheKey = CacheKeys.Articles;
                var cacheData = await _memoryCacheRepository.GetCahceKey<List<ArticlesGetDTOs>>(cacheKey);
                if(cacheData is not null && cacheData.Count > 0)
                {
                    return Result<List<ArticlesGetDTOs>>.Success(cacheData);
                }
                List<Articles> artices = await _context.Articles.AsNoTracking().OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

              

                List<ArticlesGetDTOs> articlesGetDTOs = artices.Select(x=> _mapper.Map<ArticlesGetDTOs>(x)).ToList();

                await _memoryCacheRepository.SetAsync(cacheKey, articlesGetDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                });

                return Result<List<ArticlesGetDTOs>>.Success(articlesGetDTOs);

            }catch(Exception ex)
            {
                throw new Exception("An error occured while getting All articles");
            }
        }

        public async Task<Result<ArticlesGetDTOs>> GetArticlesById(Guid Id)
        {
            try
            {
                var cacheKeys = $"GetArticlesById{Id}";
                var cacheData = await _memoryCacheRepository.GetCahceKey<ArticlesGetDTOs>(cacheKeys);
                if(cacheData is not null)
                {
                    return Result<ArticlesGetDTOs>.Success(cacheData);
                }

                var articles = await _context.Articles.SingleOrDefaultAsync(x=>x.ArticlesId == Id);
                if(articles is null)
                {
                    return Result<ArticlesGetDTOs>.Failure("Not Found");

                }
                var articlesDTO = _mapper.Map<ArticlesGetDTOs>(articles);
                await _memoryCacheRepository.SetAsync(cacheKeys, articlesDTO, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                });
                
                return Result<ArticlesGetDTOs>.Success(articlesDTO);




            }catch(Exception ex)
            {
                throw new Exception("An errror occured while getting al articles");
            }
        }

        public Task<Result<IQueryable<ArticlesWithCommentsDTOs>>> GetArticlesWithComments(int page, int pageSize)
        {
            try
            {
                IQueryable<ArticlesWithCommentsDTOs> articles = _context.Articles.Include(x => x.Comments)
                    .Select(artices => new ArticlesWithCommentsDTOs
                    {
                        ArticlesId = artices.ArticlesId,
                        ArticlesTitle = artices.ArticlesTitle,
                        ArticlesContent = artices.ArticlesContent,
                        Comments = artices.Comments,

                    }).AsNoTracking().AsQueryable();
                return Task.FromResult(Result<IQueryable<ArticlesWithCommentsDTOs>>.Success(articles));

            }catch(Exception ex)
            {
                throw new Exception("An error occured while getting AllArticleswithComments");
            }
        }

        public Task<ArticlesGetDTOs> SaveArticles(ArticlesCreateDTOs articlesCreateDTOs)
        {
            throw new NotImplementedException();
        }

        public Task<ArticlesGetDTOs> UpdateArticles(ArticlesUpdateDTOs articlesUpdateDTOs)
        {
            throw new NotImplementedException();
        }
    }
}
