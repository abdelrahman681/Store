using AutoMapper;
using DashBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Enum;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IUnitOfWork;
using StoreDashboard.Helpers;

namespace StoreDashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<IActionResult> Index(ProductParams productParams)
        {
            var totalItems = await _unitOfWork.Repository<Product>().GetAllAsync();
            ViewBag.TotalPage =(int)Math.Ceiling(totalItems.Count / (double)productParams.PageSize);
            var spec = new ProductWithBrandAndCategorySpec(productParams);
            var products = await _unitOfWork.Repository<Product>().GetAllAsyncWithSpecification(spec);
            var mappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(products);
            ViewBag.PageIndex = productParams.PageIndex;
            ViewBag.PageSize = productParams.PageSize;
            ViewBag.Sorts = new List<SelectListItem>
            {
                new() { Value = ((int)Sorting.NameAsc).ToString(), Text = "Name (A-Z)" },
                new() { Value = ((int)Sorting.NameDesc).ToString(), Text = "Name (Z-A)" },
                new() { Value = ((int)Sorting.PriceAsc).ToString(), Text = "Price (Low → High)" },
                new() { Value = ((int)Sorting.PriceDesc).ToString(), Text = "Price (High → Low)" }
            };

            return View(mappedProducts);
        }
         [HttpGet]
        public async Task<IActionResult> Details(int Id,string ViewName= "Details")
        {
            var spec = new ProductWithBrandAndCategorySpec(Id);
            var product = await _unitOfWork.Repository<Product>().GetByIdAsyncWithSpecification(spec);
            if(product is not null)
            {
                var mappedProduct = _mapper.Map<Product, ProductViewModel>(product);
                return View(ViewName, mappedProduct);
            }
            return View(product);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    model.PictureUrl = DecoumentSetting.UploadFile(model.Image, "products");
                }

                else
                model.PictureUrl = "imagess/products/hat-react2.png";
                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);
                await _unitOfWork.Repository<Product>().AddAsync(mappedProduct);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            return await Details(Id,nameof(Edit));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model,[FromRoute]int Id)
        {
            if (Id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    if (model.PictureUrl != null)
                    {
                        DecoumentSetting.DeleteFile(model.PictureUrl, "products");
                        model.PictureUrl = DecoumentSetting.UploadFile(model.Image, "products");
                    }

                    else
                        model.PictureUrl = DecoumentSetting.UploadFile(model.Image, "products");

                    var mapedProduct = _mapper.Map<ProductViewModel, Product>(model);
                    _unitOfWork.Repository<Product>().Update(mapedProduct);
                    var result=await _unitOfWork.CompleteAsync();
                    if(result<0)
                        return View(result);
                    else
                        return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            return await Details(Id, nameof(Delete));
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute]int Id, ProductViewModel model)
        {
            if(Id!=model.Id) return BadRequest();
            try
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(model.Id);

                if (product.PictureUrl != null)
                    DecoumentSetting.DeleteFile(product.PictureUrl, "products");

                _unitOfWork.Repository<Product>().Delete(product);

                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
             ModelState.AddModelError(string.Empty, ex.Message);
            }
                return View(model);
        }
    }
}
