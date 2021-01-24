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
    public class _CategoryOnePartialModel : PageModel
    {
        private readonly CmsContext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _maper;
        private readonly ILoggerManager _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _env;

        public _CategoryOnePartialModel(CmsContext context, IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger,
            UserManager<IdentityUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _repositoryWrapper = repositoryWrapper;
            _maper = mapper;
            _logger = logger;
            _userManager = userManager;
            _env = env;
        }
        public List<Tuple<ArticleGetTopByCategoryIdDTO, List<ArticleSearchDTO>, ArticleCategoryDTO>> CategoryBlockOne;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var articleTopBlockOne = new ArticleGetTopByCategoryId_Result();
            //Nếu muốn thêm category thì add thêm tuple vào block one
            var cateBlockOne = new List<Tuple<ArticleGetTopByCategoryIdDTO, List<ArticleSearchDTO>, ArticleCategoryDTO>>();
            //GetCategory

            var articleCate = await _repositoryWrapper.ArticleCategory.FirstOrDefaultAsync(p => p.Id == id);

            //Get Category block One
            var articleTop = await _repositoryWrapper.Article.ArticleGetTopByCategoryId(id);
            if (articleTop.Count > 0)
            {
                articleTopBlockOne = articleTop.Take(1).OrderBy(p => p.LastEditDate).FirstOrDefault();
            }
            //ArticleSearch
            var modelFilter = new ArticleSearchFilter();
            modelFilter.ArticleCategoryId = id;
            modelFilter.ExceptionArticleTop = true;
            modelFilter.Efficiency = true;
            modelFilter.CurrentPage = 1;
            modelFilter.PageSize = 2;
            modelFilter.FromDate = DateTime.Now.AddYears(-10);
            modelFilter.ToDate = DateTime.Now;
            var lstArticle = await _repositoryWrapper.Article.ArticleSearch(modelFilter);
            var item1 = Tuple.Create(_maper.Map<ArticleGetTopByCategoryIdDTO>(articleTopBlockOne), _maper.Map<List<ArticleSearchDTO>>(lstArticle), _maper.Map<ArticleCategoryDTO>(articleCate));
            cateBlockOne.Add(item1);

            //Set value
            CategoryBlockOne = cateBlockOne;
            return Page();
        }

    }
}