using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CMS.Data.DataEntity;
using CMS.Data.ModelsDTO;
using CMS.Services.RepositoriesBase;
using CMS.Website.Logging;
using Microsoft.AspNetCore.Identity;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Hosting;
using CMS.Data.ModelsStore;
using CMS.Data.ModelFilter;
using System;
using System.Collections.Generic;

namespace CMS.Website.Pages.Shared
{
    public class _HomeBlockThreePagingPartialModel : PageModel
    {
        private readonly CmsContext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _maper;
        private readonly ILoggerManager _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _env;

        public _HomeBlockThreePagingPartialModel(CmsContext context, IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger,
            UserManager<IdentityUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _repositoryWrapper = repositoryWrapper;
            _maper = mapper;
            _logger = logger;
            _userManager = userManager;
            _env = env;
        }
        public List<ArticleGetByBlockIdDTO> BlockThree;
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public async Task<IActionResult> OnGetAsync(int? id, string url ,int? p)
        {
            if (id != null)
            {
                var blockThreeResult = await _repositoryWrapper.Article.ArticleGetByBlockId(id ?? 0);
                BlockThree = _maper.Map<List<ArticleGetByBlockIdDTO>>(blockThreeResult);
            }
            else
            {
                if (String.IsNullOrEmpty(url))
                {
                    return NotFound();
                }
                var articleCategory = await _repositoryWrapper.ArticleCategory.GetArticleCategoryByUrl(url.Trim());
                if (articleCategory == null)
                {
                    return NotFound();
                }                
                
             
                var blockThreeResult = await _repositoryWrapper.Article.ArticleGetByCategoryId(articleCategory.Id,10,p);
                BlockThree = _maper.Map<List<ArticleGetByBlockIdDTO>>(blockThreeResult.Item1);
                Count = blockThreeResult.Item2;
                CurrentPage = p ?? 1;
                ViewData["ReturnUrl"] = url;
            }
            return Page();
        }
    }
}