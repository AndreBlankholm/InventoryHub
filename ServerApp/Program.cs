
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// INDUSTRY STANDARD: Configure JSON serialization options
// - camelCase for property names (JavaScript convention)
// - Case-insensitive deserialization (handles client variations)
// - Formatted output for readability
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

// INDUSTRY STANDARD: Add RFC 7807 Problem Details for standardized error responses
// This provides consistent error format across all endpoints
builder.Services.AddProblemDetails();

// CORS CONFIGURATION: Allow cross-origin requests from Blazor client
// Required for browser security when client and server run on different ports
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        policy.AllowAnyOrigin()      // Allow any domain (development only)
              .AllowAnyMethod()      // Allow GET, POST, PUT, DELETE
              .AllowAnyHeader();     // Allow any request headers
    });
});

var app = builder.Build();

// MIDDLEWARE PIPELINE: Handle exceptions and status codes properly
// - UseExceptionHandler(): Catches unhandled exceptions, returns Problem Details
// - UseStatusCodePages(): Provides consistent responses for HTTP status codes
app.UseExceptionHandler();
app.UseStatusCodePages();

// ENABLE CORS: Must be before endpoint mapping
app.UseCors("BlazorPolicy");

// GET ENDPOINT: Retrieve all products with proper HTTP semantics
// Returns 200 OK with typed data instead of anonymous objects
app.MapGet("/api/productlist", () =>
{
    // TYPED MODELS: Using proper classes instead of anonymous objects
    // This enables validation, serialization control, and IntelliSense
    var products = new List<Product>
    {
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 1200.50m,        // Using decimal for currency (not double)
            Stock = 25,
            Category = new Category { Id = 101, Name = "Electronics" }
        },
        new Product
        {
            Id = 2,
            Name = "Headphones",
            Price = 50.00m,          // Decimal prevents floating-point precision errors
            Stock = 100,
            Category = new Category { Id = 102, Name = "Accessories" }
        }
    };

    // PROPER HTTP RESPONSE: Using Results.Ok() for explicit 200 status
    return Results.Ok(products);
});

// POST ENDPOINT: Create new product with validation
// Demonstrates input validation and proper HTTP status codes
app.MapPost("/api/products", async (Product product) =>
{
    // SERVER-SIDE VALIDATION: Validate model using Data Annotations
    // This runs the [Required], [Range], [StringLength] attributes
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(product);
    
    if (!Validator.TryValidateObject(product, validationContext, validationResults, true))
    {
        // VALIDATION ERRORS: Return 400 Bad Request with detailed error messages
        // Uses Problem Details format for consistent error structure
        var errors = validationResults.Select(vr => vr.ErrorMessage).ToArray();
        return Results.ValidationProblem(errors.ToDictionary(e => "product", e => new[] { e! }));
    }

    // SUCCESS RESPONSE: Return 201 Created with location header
    // Follows REST conventions for resource creation
    return Results.Created($"/api/products/{product.Id}", product);
});

app.Run();

// DATA MODELS: Strongly-typed classes with validation attributes
// These replace anonymous objects and provide compile-time safety
public class Product
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }    // Decimal for precise currency calculations

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public Category Category { get; set; } = new();
}

public class Category
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Category Id must be a positive number")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 50 characters")]
    public string Name { get; set; } = string.Empty;
}