using AutoMapper;
using Firmeza.Application.DTOs.Customers;
using Firmeza.Application.DTOs.Products;
using Firmeza.Domain.Entities;

namespace Application.Mappings;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        // CLIENTS 
        CreateMap<Customer, CustomerIndexViewModel>().ReverseMap();
        CreateMap<Customer, CustomerCreateViewModel>().ReverseMap();
        CreateMap<Customer, CustomerEditViewModel>().ReverseMap();
        
        // PRODUCTS
        CreateMap<Product, ProductIndexViewModel>().ReverseMap();
        CreateMap<Product, ProductCreateViewModel>().ReverseMap();
        CreateMap<Product, ProductEditViewModel>().ReverseMap();
    }
}