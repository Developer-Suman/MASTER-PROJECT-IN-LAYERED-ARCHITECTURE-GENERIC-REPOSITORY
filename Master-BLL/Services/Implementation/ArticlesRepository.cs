﻿using AutoMapper;
using Master_BLL.DTOs.Articles;
using Master_BLL.DTOs.Comment;
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
            //Implement IMemoryCache and remove that before delete articles
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

        public Result<IQueryable<ArticlesWithCommentsDTOs>> GetArticlesWithComments(int page, int pageSize)
        {
            try
            {
                IQueryable <ArticlesWithCommentsDTOs> articleswithComments = _context.Articles
                    .Include(x => x.Comments)
                    .Select(articles => new ArticlesWithCommentsDTOs
                    {
                        ArticlesId = articles.ArticlesId,
                        ArticlesTitle = articles.ArticlesTitle,
                        ArticlesContent = articles.ArticlesContent,
                        Comments = articles.Comments.Select(a=>_mapper.Map<CommentsGetDTOs>(a)).ToList(),
                   

                    }).AsNoTracking().Skip((page-1)*pageSize).Take(pageSize).AsQueryable();



                return Result<IQueryable<ArticlesWithCommentsDTOs>>.Success(articleswithComments);

            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while getting AllArticleswithComments");
            }
        }

        public async Task<Result<List<CommentsWithArticles>>> GetCommentsWithArticlesName(int page, int pageSize)
        {
            try
            {
                var cacheKey = $"GetCommentsWithArticlesName{page}{pageSize}";
                var cacheData = await _memoryCacheRepository.GetCahceKey<List<CommentsWithArticles>>(cacheKey);

                if (cacheData is not null)
                {
                    return Result<List<CommentsWithArticles>>.Success(cacheData);
                }

              

                List<CommentsWithArticles> commentsWithArticles = await _context.Articles.SelectMany(x => x.Comments
                .Select(x => new CommentsWithArticles()
                {
                    CommentsId = x.CommentsId,
                    CommentDescription = x.CommentDescription,
                    ArticleName = x.Articles.ArticlesTitle,

                })).AsNoTracking().Skip((1 - page) * pageSize).Take(pageSize).ToListAsync();



                await _memoryCacheRepository.SetAsync(cacheKey, commentsWithArticles, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)

                });



                //List<CommentsWithArticles> commentsWithArticles = await _context.Articles.SelectMany(x => x.Comments
                //.Select(x => _mapper.Map<CommentsWithArticles>(x)));

                #region ImplementArticlesNameInAutomapper
                //List<CommentsWithArticles> commentsWithArticles = await _context.Articles.SelectMany(x => x.Comments
                //.Select(x => new CommentsWithArticles()
                //{
                //    CommentsId = x.CommentsId,
                //    CommentDescription = x.CommentDescription,
                //    ArticleName = x.Articles.ArticlesTitle,

                //})).AsNoTracking().Skip((1 - page) * pageSize).Take(pageSize).ToListAsync();
                #endregion


                return Result<List<CommentsWithArticles>>.Success(commentsWithArticles);

            }
            catch (Exception)
            {
                throw new Exception("An error occured while getting Comments from Articles");
            }
        }

        public Task<ArticlesGetDTOs> SaveArticles(ArticlesCreateDTOs articlesCreateDTOs)
        {
            //Use IMemoryCache and Remove that before creating
            throw new NotImplementedException();
        }

        public Task<ArticlesGetDTOs> UpdateArticles(ArticlesUpdateDTOs articlesUpdateDTOs)
        {
            //Use IMemoryCache and Remove that before Updating
            throw new NotImplementedException();
        }
    }
}
