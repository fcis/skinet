using System;
using Core.Entities;

namespace Core.Interfaces.Specifications;

public class BrandListSpecification : BaseSpecification<Product,string>
{
    public BrandListSpecification()
    {
        AddSelect(x=>x.Brand);
        ApplyDistinct();
    }


}
