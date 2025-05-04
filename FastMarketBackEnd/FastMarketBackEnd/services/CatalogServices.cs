using FastMarketBackEnd.Data;
using FastMarketBackEnd.DTOs;
using FastMarketBackEnd.models;
using Microsoft.EntityFrameworkCore;


namespace FastMarketBackEnd.services;

public class CatalogServices
{
    private readonly ApplicationContext _db;
    private readonly ILogger<TokensService> _logger;
    private readonly IWebHostEnvironment _env;
    public HttpContext HttpContext { get; set; }
    public CatalogServices(ApplicationContext db, ILogger<TokensService> logger, IWebHostEnvironment env)
    {
        this._db = db;
        this._logger = logger;
        this._env = env;
    }

    
    public async Task<ProductDTO> CreateProduct(List<IFormFile> images, ProductDTO productDto)
    {

        var category = new Category();
        var seller = new Seller();
        if (productDto.CategoryID != null)
        {
            category = await this._db.Categories.FindAsync(productDto.CategoryID);
        }

        if (productDto.SellerID != null)
        {
            seller = await this._db.Sellers.FindAsync(productDto.SellerID);
        }
        Product product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Stock_quantity = productDto.Stock_quantity,
            Category = category,
            Model = productDto.Model,
            Brand = productDto.Brand,
            SellerId = (int)productDto.SellerID,
            StatusModeration = StatusModeration.notModerated,
        };
        List<PictureProduct> pictureProducts = new List<PictureProduct>();
        var uploadsDir = Path.Combine(_env.WebRootPath, "images");
        Directory.CreateDirectory(uploadsDir);

        // var savedFilePaths = new List<string>();

        foreach (var image in images)
        {
            var savedPicture = new PictureProduct();
           
            var uniqueName = Path.GetExtension(image.FileName);
            var savePath = Path.Combine(uploadsDir, image.FileName);

            await using var stream = new FileStream(savePath, FileMode.Create);
            
            await image.CopyToAsync(stream);
            savedPicture.FileName = image.FileName;
            savedPicture.Path = "/images";
            savedPicture.Link = $"{savedPicture.Path}/{savedPicture.FileName}";
            pictureProducts.Add(savedPicture);
            // savedFilePaths.Add(savePath);
        }
        product.Pictures = pictureProducts;
        var productEntity = await this._db.Products.AddAsync(product);
        await this._db.SaveChangesAsync();
        
        var productDTOs = ConvertProductToDTO(productEntity.Entity);
        
        

        return productDTOs;

    }

    public async Task<List<ProductDTO>> GetModeratedProducts()
    {
        List<ProductDTO> productDTOs = new List<ProductDTO>();
        var products = await this._db.Products
            .Include(p=>p.Pictures)
            .Include(p => p.Category)
            .Include(p=>p.Seller)
            .Include(p=>p.Characteristics).Where(p=>p.StatusModeration==StatusModeration.moderated)
            .ToListAsync();
        foreach (var product in products)
        {
            productDTOs.Add(ConvertProductToDTO(product));
        }
        return productDTOs;
    }
    
    public async Task<List<ProductDTO>> GetNotModeratedProducts()
    {
        List<ProductDTO> productDTOs = new List<ProductDTO>();
        var products = await this._db.Products
            .Include(p=>p.Pictures)
            .Include(p => p.Category)
            .Include(p=>p.Seller)
            .Include(p=>p.Characteristics).Where(p=>p.StatusModeration==StatusModeration.notModerated)
            .ToListAsync();
        foreach (var product in products)
        {
            productDTOs.Add(ConvertProductToDTO(product));
        }
        return productDTOs;
    }
    
    public async Task<List<ProductDTO>> GetAllProducts()
    {
        List<ProductDTO> productDTOs = new List<ProductDTO>();
        var products = await this._db.Products
            .Include(p=>p.Pictures)
            .Include(p => p.Category)
            .Include(p=>p.Seller)
            .Include(p=>p.Characteristics)
            .ToListAsync();
        foreach (var product in products)
        {
            productDTOs.Add(ConvertProductToDTO(product));
        }
        return productDTOs;
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        var product = await this._db.Products
            .Include(p=>p.Pictures)
            .Include(p => p.Category)
            .Include(p=>p.Seller)
            .FirstOrDefaultAsync(p=>p.Id==id && p.StatusModeration==StatusModeration.moderated);
        var characteristics = _db.Characteristics.Include(c=>c.NameCharacteristics)
            .Where(c=>c.ProductId==product.Id)
            .ToList();
        product.Characteristics = characteristics;
        return ConvertProductToDTO(product);
    }
    public async Task<List<ProductDTO>> GetProductByCategoryId(int CatalogId)
    {
        List<ProductDTO> productDTOs = new List<ProductDTO>();
        var products = await this._db.Products
            .Include(p=>p.Pictures)
            .Include(p => p.Category)
            .Include(p=>p.Seller)
            .Include(p=>p.Characteristics)
            .Where(p=>p.CategoryId==CatalogId && p.StatusModeration==StatusModeration.moderated).ToListAsync();
        if (!products.Any())
        {
            _logger.LogError("There are no products in the selected categories.");
            throw new Exception("There are no products in the selected categories.");
        }
        foreach (var product in products)
        {
            productDTOs.Add(ConvertProductToDTO(product));
        }
        return productDTOs;
    }
    
    public async Task<List<ProductDTO>> GetProductBySellerId(int SellerId)
    {
        List<ProductDTO> productDTOs = new List<ProductDTO>();
        var products = await this._db.Products
            .Include(p=>p.Pictures)
            .Include(p => p.Category)
            .Include(p=>p.Seller)
            .Include(p=>p.Characteristics)
            .Where(p=>p.SellerId==SellerId).ToListAsync();
        if (!products.Any())
        {
            _logger.LogError("There are no products in the selected seller.");
            throw new Exception("There are no products in the selected seller.");
        }
        foreach (var product in products)
        {
            productDTOs.Add(ConvertProductToDTO(product));
        }
        return productDTOs;
    }
    
    public Task ChangeProductByCategoryId(int CatalogId, ProductDTO productDto)
    {
        var product = ConvertDtoToProduct(productDto);
        
        if (CatalogId!=product.Id)
        {
            _logger.LogError("selected product does not exist.");
            throw new Exception("selected product does not exist.");
        }
        _db.Entry(product).State = EntityState.Modified;
        
        if(_db.SaveChangesAsync().IsCompletedSuccessfully)
        {
            _logger.LogInformation("Product was successfully changed.");
        }
        else
        {
            _logger.LogError("Product was not changed.");
            throw new Exception("Product was not changed.");
        }

        return Task.CompletedTask;
    }
    public async Task DeleteProductById(int id)
    {
        var product = await this._db.Products.FirstOrDefaultAsync(p=>p.Id==id);
        if (product is null)
        {
            _logger.LogError("Product does not exist.");
            throw new Exception("Product does not exist.");
        }

        foreach (var picture in product.Pictures)
        {
            var imagePath = Path.Combine("wwwroot/images", picture.FileName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            this._db.Remove(picture);
            await this._db.SaveChangesAsync();
            this._logger.LogInformation($"Image from {product.Id}: {product.Name} was successfully deleted.");
        }
        
        this._db.Products.Remove(product);
        await this._db.SaveChangesAsync();
        this._logger.LogInformation($"Product id: {product.Id}, name: {product.Name} was successfully deleted.");
    }

    public ProductDTO ConvertProductToDTO(Product product)
    {
        var listPicturesProductDTOs = new List<PictureProductDTO>();
        if (product.Pictures != null)
            foreach (var picture in product.Pictures)
            {
                listPicturesProductDTOs.Add(ConvertPictureToDto(picture));
            }

        var productDto = new ProductDTO()
        {
            ID = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Model = product.Model,
            Brand = product.Brand,
            Stock_quantity = product.Stock_quantity,
            SellerID = product.Seller.Id,
            CategoryID = product.Category.Id,
            Pictures = listPicturesProductDTOs,
            Characteristics = product.Characteristics,
            Favorites = product.Favorites,
            Ratings = product.Ratings,
            Reviews = product.Reviews,
            StatusModeration = product.StatusModeration,
        };
        return productDto;
    }

    private async Task<Product> ConvertDtoToProduct(ProductDTO productDto)
    {
        var listPictureProduct = new List<PictureProduct>();
        foreach (var picture in productDto.Pictures){listPictureProduct.Add(await ConvertDTOToPicture(picture));}
        var Product = new Product()
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Model = productDto.Model,
            Brand = productDto.Brand,
            Stock_quantity = productDto.Stock_quantity,
            Seller = await this._db.Sellers.FirstOrDefaultAsync(s=>s.Id == productDto.SellerID),
            Category = await this._db.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryID),
            Pictures = listPictureProduct,
            Characteristics = productDto.Characteristics,
            Favorites = productDto.Favorites,
            Ratings = productDto.Ratings,
            Reviews = productDto.Reviews,
            StatusModeration = productDto.StatusModeration,
        };
        return Product;
    }

    private PictureProductDTO ConvertPictureToDto(PictureProduct pictureProduct)
    {
        var pictureProductDto = new PictureProductDTO()
        {
            Id = pictureProduct.Id,
            FileName = pictureProduct.FileName,
            Path = pictureProduct.Path,
            PreviewPicture = pictureProduct.PreviewPicture,
            Link = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{pictureProduct.Link}",  
            ProductID = pictureProduct.Product.Id
        };
        return pictureProductDto;
    }

    private async Task<PictureProduct> ConvertDTOToPicture(PictureProductDTO pictureProductDto)
    {
        var product = await this._db.Products.FirstOrDefaultAsync(p=>p.Id == pictureProductDto.ProductID);
        var pictureProduct = new PictureProduct()
        {
            FileName = pictureProductDto.FileName,
            Path = pictureProductDto.Path,
            PreviewPicture = pictureProductDto.PreviewPicture,
            Product = product
        };
        return pictureProduct;
    }
}