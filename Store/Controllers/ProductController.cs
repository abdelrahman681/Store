using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IUnitOfWork;
using Store.DTO;
using Store.Helpers;

namespace Store.Controllers
{

    public class ProductController : BaseApiController
    {
        #region Field
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor
        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion
        [HttpGet("GetAllProduct")]
        [ProducesResponseType(typeof(IReadOnlyList<Pagination<ProductDTO>>),StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductDTO>>>> GetAllProduct([FromQuery]ProductParams @params)
        {
            var spec = new ProductWithBrandAndCategorySpec(@params);
            var product =await _unitOfWork.Repository<Product>().GetAllAsyncWithSpecification(spec);
            var mapeddProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(product);
            var countSpec = new CountOfProductOfSpec(@params);
            var count=await _unitOfWork.Repository<Product>().GetCountWithSpecification(countSpec);
            var productPagination = new Pagination<ProductDTO>
            {
                PageIndex = @params.PageIndex,
                PageSize = @params.PageSize,
                CountOfAllItem = spec.CountOfAllItem,
                CountOfSpec=count,
                Data = mapeddProduct
            };
            return Ok(productPagination);
        }
        [HttpGet("GetProductById/{Id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int Id)
        {
            var spec = new ProductWithBrandAndCategorySpec(Id);
            var product = await _unitOfWork.Repository<Product>().GetByIdAsyncWithSpecification(spec);
            var mapeddProduct = _mapper.Map<Product,ProductDTO>(product);
            if (mapeddProduct is null) return NotFound();
            return Ok(mapeddProduct);
        }
    }
}
