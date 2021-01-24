using System;
using System.Collections.Generic;
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

namespace CMS.Website.Pages.Shared
{
    public class _CategoryTwoPartialModel : PageModel
    {
        private readonly CmsContext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _maper;
        private readonly ILoggerManager _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _env;

        public _CategoryTwoPartialModel(CmsContext context, IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger,
            UserManager<IdentityUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _repositoryWrapper = repositoryWrapper;
            _maper = mapper;
            _logger = logger;
            _userManager = userManager;
            _env = env;
        }
        public Tuple<ArticleGetTopByCategoryIdDTO, List<ArticleSearchDTO>, ArticleCategoryDTO> CategoryBlockTwo;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var articleTopBlockTwo = new ArticleGetTopByCategoryId_Result();
            var articleCate = await _repositoryWrapper.ArticleCategory.FirstOrDefaultAsync(p => p.Id == id);
            var articleTop2 = await _repositoryWrapper.Article.ArticleGetTopByCategoryId(id);
            if (articleTop2.Count > 0)
            {
                articleTopBlockTwo = articleTop2.Take(1).OrderBy(p => p.LastEditDate).FirstOrDefault();
            }
           
            var lstArticle2 = await _repositoryWrapper.Article.ArticleGetNewByCategoryId(id,2);
            CategoryBlockTwo = Tuple.Create(_maper.Map<ArticleGetTopByCategoryIdDTO>(articleTopBlockTwo), _maper.Map<List<ArticleSearchDTO>>(lstArticle2), _maper.Map<ArticleCategoryDTO>(articleCate));
            return Page();
        }
    }
}