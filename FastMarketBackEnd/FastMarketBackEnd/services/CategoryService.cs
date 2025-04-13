using FastMarketBackEnd.Data;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.models;

namespace FastMarketBackEnd.services;

public class CategoryService
{
    private readonly ApplicationContext _db;
    private readonly ILogger<TokensService> _logger;
    private readonly IWebHostEnvironment _env;
    public CategoryService(ApplicationContext db, ILogger<TokensService> logger, IWebHostEnvironment env)
    {
        _db = db;
        _logger = logger;
        _env = env;
    }
    // public async Task<PrimaryСategoryDTO> PrimaryCategoryModelToDTO(PrimaryСategory primaryСategory)
    // {
    //     
    //     var primaryCategoryDTO = new PrimaryСategoryDTO()
    //     {
    //         ID = primaryСategory.Id,
    //         Name = primaryСategory.Name,
    //         IsActive = primaryСategory.IsActive,
    //     };
    //
    // }
}