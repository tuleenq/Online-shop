//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using MyShop.Infrastructure.Data;
//using MediatR;
//using MyShop.Application.Helpers;


//public class UpdateReviewCommand : IRequest<(string Review, string UserName)>
//{
//    public string ProductName { get; set; }
//    public string Review { get; set; }
//}
//public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, (string Review, string UserName)>
//{
//    private readonly ApplicationDbContext _dbContext;
//    private readonly UsersHelper _userHelper;

//    public UpdateReviewCommandHandler(ApplicationDbContext dbContext, UsersHelper userHelper)
//    {
//        _dbContext = dbContext;
//        _userHelper = userHelper;
//    }

//    public async Task<(string Review, string UserName)> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
//    {
//        var product = await _dbContext.Products
//            .FirstOrDefaultAsync(p => p.Name == request.ProductName, cancellationToken);

//        if (product == null)
//            return (null, null);

//        product.reviews = string.IsNullOrEmpty(product.reviews)
//            ? request.Review
//            : $"{product.reviews}; {request.Review}";

//        _dbContext.Products.Update(product);
//        await _dbContext.SaveChangesAsync(cancellationToken);

//        var username = _userHelper.GetUserNameFromSession();
//        return (product.reviews, username);
//    }
//}


