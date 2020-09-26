using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Errors;
using AutoMapper;
using Core.Models;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductType> productTypeRepository, IGenericRepository<ProductBrand> productBrandRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            //var products = await _productRepository.GetAllAsync();
            var specification = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productRepository.GetAllWithSpecificationAsync(specification);
            return Ok(_mapper.Map<IEnumerable<ProductReturnToDto>>(products));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReturnToDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<IActionResult> GetProduct(int id)
        {
            //var product = await _productRepository.GetByIdAsync(id);
            var specification = new ProductsWithTypesAndBrandsSpecification(p => p.Id == id);
            var product = await _productRepository.GetWithSpecificationAsync(specification);
            if (product == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));
            }
            return Ok(_mapper.Map<ProductReturnToDto>(product));
        }
        [HttpGet("types")]
        public async Task<IActionResult> GetProductTypes()
        {
            var productTypes = await _productTypeRepository.GetAllAsync();
            return Ok(productTypes);
        }
        [HttpGet("brands")]
        public async Task<IActionResult> GetProductBrands()
        {
            var productBrands = await _productBrandRepository.GetAllAsync();
            return Ok(productBrands);
        }
    }
}
