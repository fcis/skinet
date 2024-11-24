using System;
using Core.Entities;

namespace Core.Interfaces.Specifications;

public class TypeListSpecification:BaseSpecification<Product,string>
{
    public TypeListSpecification()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }

}
