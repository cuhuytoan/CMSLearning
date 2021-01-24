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
    public class _HomeBlockTwoPartialModel : PageModel
    {
        private readonly CmsContext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _maper;
        private readonly ILoggerManager _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _env;

        public _HomeBlockTwoPartialModel(CmsContext context, IRepositoryWrapper repositoryWrapper, IMapper mapper, ILoggerManager logger,
            UserManager<IdentityUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _repositoryWrapper = repositoryWrapper;
            _maper = mapper;
            _logger = logger;
            _userManager = userManager;
            _env = env;
        }
        public List<ArticleGetByBlockIdDTO> BlockTwo;
        public async Task<IActionResult> OnGetAsync(int? id,string url)
        {
            if(id != null)
            {
                var blockTwoResult = await _repositoryWrapper.Article.ArticleGetByBlockId(id?? 0);
                BlockTwo = _maper.Map<List<ArticleGetByBlockIdDTO>>(blockTwoResult);
            }
            else // Tạm thời lấy theo id cho trang article Category
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
                var blockTwoResult = await _repositoryWrapper.Article.ArticleGetNewByCategoryId(articleCategory.Id,3);
                BlockTwo = _maper.Map<List<ArticleGetByBlockIdDTO>>(blockTwoResult);
            }
            return Page();
        }

    }
}