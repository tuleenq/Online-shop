using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.Category.Queries
{
    public class ViewCategoryCommand : IRequest<List<CategoryDto>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
       

    }



    public class GetAllCategoriesHandler : IRequestHandler<ViewCategoryCommand, List<CategoryDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllCategoriesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> Handle(ViewCategoryCommand request, CancellationToken cancellationToken)
        {
            var categories = await _context.category
         .Select(c => new CategoryDto
         {
             Id = c.Id,
             Name = c.Name
         })
         .ToListAsync();
            return categories;
        }

    }
}
