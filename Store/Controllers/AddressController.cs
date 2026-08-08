using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.IUnitOfWork;
using Store.DTO;
using Store.Errors;
using Store.Helpers;
using System.Security.Claims;

namespace Store.Controllers
{

    public class AddressController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AddressController(IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }
        [HttpPost("AddNewAddress")]
        [Authorize]
        public async Task<ActionResult<string>> AddNewAddress(AddressDTO address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);
            var mappedAddress=_mapper.Map<AddressDTO,Address>(address);
            mappedAddress.UserId = user.Id;
            await _unitOfWork.Repository<Address>().AddAsync(mappedAddress);
            var result=await _unitOfWork.CompleteAsync();
            if (result > 0)
                return Ok("Address added SucssesFull");
            else
                return BadRequest(new ErrorApiResponse(404));
        }
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<string>> UpdateAddress(AddressDTO address)
        {
            var existingAddress = await _unitOfWork.Repository<Address>()
                .GetByIdAsync(address.Id);

            if (existingAddress == null)
                return NotFound(new ErrorApiResponse(404));

            _mapper.Map(address, existingAddress); 

            _unitOfWork.Repository<Address>().Update(existingAddress);

            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
                return Ok("Address Updated Successfully");

            return BadRequest(new ErrorApiResponse(400));
        }
        [HttpDelete("DeleteAddress/{AddressId}")]
        [Authorize]
        public async Task<ActionResult<string>> DeleteAddress(int AddressId)
        {
            var address=await _unitOfWork.Repository<Address>().GetByIdAsync(AddressId);
            if (address == null) return NotFound(new ErrorApiResponse(404));
            _unitOfWork.Repository<Address>().Delete(address);
            var result=await _unitOfWork.CompleteAsync();
            if (result > 0)
                return Ok("Address Deleted SucssesFull");
            else
                return BadRequest(new ErrorApiResponse(404));
        }
        [HttpGet("GetAddress/{AddressId}")]
        public async Task<ActionResult<AddressDTO>> GetAddress(int AddressId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);
            var address = await _unitOfWork.Repository<Address>().GetByIdAsync(AddressId);
            if (address == null) return NotFound(new ErrorApiResponse(404));
            var mappedAddress = _mapper.Map<Address, AddressDTO>(address);
            return Ok(mappedAddress);
        }
        [HttpGet("GetAllAddress")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<AddressDTO>>> GetAllAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);
            var address = await _unitOfWork.Repository<Address>().GetAllAsync();
            address = address.Where(u => u.UserId == user.Id).ToList();
            if (address == null) return NotFound(new ErrorApiResponse(404));
            var mappedAddress = _mapper.Map< IReadOnlyList<Address>, IReadOnlyList<AddressDTO>>(address);
            return Ok(mappedAddress);
        }
    }
}
