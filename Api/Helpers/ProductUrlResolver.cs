using Api.Dtos;
using AutoMapper;
using Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductReturnToDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductReturnToDto destination, string destMember, ResolutionContext context)
        {
            return _configuration["ApiUrl"] + source.PictureUrl;
        }
    }
}
