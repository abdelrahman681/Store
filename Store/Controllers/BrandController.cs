using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IUnitOfWork;
using Store.DTO;
using Store.Errors;
using Store.Helpers;

namespace Store.Controllers
{

    public class BrandController : BaseApiController
    {
        #region Field
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor
        public BrandController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion
        [Cache(300)]
        [HttpGet("GetAllBrand")]
        [ProducesResponseType(typeof(IReadOnlyList<Pagination<ProductBrand>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductBrand>>>> GetAllBrand([FromQuery] BrandAndCategoryParams @params)
        {
            var Spec = new BrandSpecification(@params);
            var brand = await _unitOfWork.Repository<ProductBrand>().GetAllAsyncWithSpecification(Spec);
            var brandPagination = new Pagination<ProductBrand>
            {
                PageIndex = @params.PageIndex,
                PageSize = @params.PageSize,
                CountOfAllItem = Spec.CountOfAllItem,
                Data = brand
            };
            return Ok(brandPagination);
        }
        [HttpGet("GetBrandById/{Id}")]
        public async Task<ActionResult<ProductBrand>> GetBrandById(int Id)
        {
            var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(Id);
            if (brand is null) return NotFound(new ErrorApiResponse(404));
            return Ok(brand);
        }
    }
}
