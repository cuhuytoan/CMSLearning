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
using System;

namespace CMS.Website.Pages.Shared
{
    public class _HomeBlockOnePartialModel : PageModel
    {
        private readonly CmsContext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _maper;
        private readonly ILoggerManager _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _env;

        public _HomeBlockOnePartialModel(CmsContext context, IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger,
            UserManager<IdentityUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _repositoryWrapper = repositoryWrapper;
            _maper = mapper;
            _logger = logger;
            _userManager = userManager;
            _env = env;
        }
        public ArticleGetByBlockIdDTO BlockOne;
        
        public async Task<IActionResult> OnGetAsync(int? id,string url)
        {
            if(id != null)
            {
                var singerBlockOne = new ArticleGetByBlockId_Result();

                var blockOneResult = await _repositoryWrapper.Article.ArticleGetByBlockId(id ?? 0);
                if (blockOneResult.Count > 0)
                {
                    singerBlockOne = blockOneResult.Take(1).OrderBy(p => p.LastEditDate).FirstOrDefault();
                }
                BlockOne = _maper.Map<ArticleGetByBlockIdDTO>(singerBlockOne);
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
                var singerBlockOne = new ArticleGetTopByCategoryId_Result();

                var blockOneResult = await _repositoryWrapper.Article.ArticleGetTopByCategoryId(articleCategory.Id);
                if (blockOneResult.Count > 0)
                {
                    singerBlockOne = blockOneResult.Take(1).OrderBy(p => p.LastEditDate).FirstOrDefault();
                }
                BlockOne = _maper.Map<ArticleGetByBlockIdDTO>(singerBlockOne);
            }
            
            return Page();
        }
        
    }
}