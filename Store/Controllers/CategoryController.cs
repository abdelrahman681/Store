using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IUnitOfWork;
using Store.Helpers;

namespace Store.Controllers
{

    public class CategoryController : BaseApiController
    {
        #region Field
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        [HttpGet("GetAllCategory")]
        [ProducesResponseType(typeof(IReadOnlyList<Pagination<ProductCategory>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductCategory>>>> GetAllCategory([FromQuery] BrandAndCategoryParams @params)
        {
            var Spec = new CategorySpecification(@params);
            var category = await _unitOfWork.Repository<ProductCategory>().GetAllAsyncWithSpecification(Spec);
            var categoryPagination = new Pagination<ProductCategory>
            {
                PageIndex = @params.PageIndex,
                PageSize = @params.PageSize,
                CountOfAllItem = Spec.CountOfAllItem,
                Data = category
            };
            return Ok(categoryPagination);
        }
        [HttpGet("GetCategoryById/{Id}")]
        public async Task<ActionResult<ProductCategory>> GetCategoryById(int Id)
        {
            var category = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(Id);
            if (category is null) return NotFound();
            return Ok(category);
        }
    }
}
