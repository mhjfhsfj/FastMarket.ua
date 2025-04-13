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
    public CatalogServices(ApplicationContext db, ILogger<TokensService> logger, IWebHostEnvironment env)
    {
        this._db = db;
        this._logger = logger;
        this._env = env;
    }

    
    public async Task<ProductDTO> CreateProduct(List<IFormFile> images, ProductDTO productDto)
    {

        var category = new Сategory();
        var seller = new Seller();
        if (productDto.CategoryId != null)
        {
            category = await this._db.Categories.FindAsync(productDto.CategoryId);
        }

        if (productDto.SellerId != null)
        {
            seller = await this._db.Sellers.FindAsync(productDto.SellerId);
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
            Seller = seller,
        };
        List<PictureProduct> pictureProducts = new List<PictureProduct>();
        var uploadsDir = Path.Combine(_env.WebRootPath, "images");
        Directory.CreateDirectory(uploadsDir);

        // var savedFilePaths = new List<string>();

        foreach (var image in images)
        {
            var savedPicture = new PictureProduct();
            // var uniqueName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var uniqueName = Path.GetExtension(image.FileName);
            var savePath = Path.Combine(uploadsDir, image.FileName);

            await using var stream = new FileStream(savePath, FileMode.Create);
            
            await image.CopyToAsync(stream);
            savedPicture.FileName = image.FileName;
            savedPicture.Path = "wwwroot/images";
            pictureProducts.Add(savedPicture);
            // savedFilePaths.Add(savePath);
        }
        product.Pictures = pictureProducts;
        var productEntity = await this._db.Products.AddAsync(product);
        await this._db.SaveChangesAsync();
        
        var productDTOs = ConvertProductToDTO(productEntity.Entity);

        return productDTOs;

    }

    public async Task<List<ProductDTO>> GetProducts()
    {
        List<ProductDTO> productDTOs = new List<ProductDTO>();
        var products = await this._db.Products
            .Include(p=>p.Pictures)
            .Include(p => p.Category)
            .Include(p=>p.Seller)
            .ToListAsync();

        foreach (var product in products)
        {
            productDTOs.Add(ConvertProductToDTO(product));
        }
        return productDTOs;
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        var product = await this._db.Products.FirstOrDefaultAsync(p=>p.Id==id);
        return ConvertProductToDTO(product);
    }

    public async Task<List<ProductDTO>> DeleteProductById(int id)
    {
        this._db.Products.Remove(await this._db.Products.FirstOrDefaultAsync(p=>p.Id==id));
        this._db.SaveChanges();
        
        return await this.GetProducts();
    }

    public ProductDTO ConvertProductToDTO(Product product)
    {
        var listPicturesProductDTOs = new List<PictureProductDTO>();
        if (product.Pictures != null)
            foreach (var picture in product.Pictures)
            {
                listPicturesProductDTOs.Add(ConvertPictureToDTO(picture));
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
            SellerId = product.Seller.Id,
            CategoryId = product.Category.Id,
            Pictures = listPicturesProductDTOs,
        };
        return productDto;
    }

    public async Task<Product> ConvertDtoToProduct(ProductDTO productDto)
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
            Seller = await this._db.Sellers.FirstOrDefaultAsync(s=>s.Id == productDto.SellerId),
            Category = await this._db.Categories.FirstOrDefaultAsync(c => c.Id == productDto.CategoryId),
            Pictures = listPictureProduct
        };
        return Product;
    }

    public PictureProductDTO ConvertPictureToDTO(PictureProduct pictureProduct)
    {
        var pictureProductDTO = new PictureProductDTO()
        {
            Id = pictureProduct.Id,
            FileName = pictureProduct.FileName,
            Path = pictureProduct.Path,
            PreviewPicture = pictureProduct.PreviewPicture,
            ProductID = pictureProduct.Product.Id
        };
        return pictureProductDTO;
    }

    public async Task<PictureProduct> ConvertDTOToPicture(PictureProductDTO pictureProductDto)
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