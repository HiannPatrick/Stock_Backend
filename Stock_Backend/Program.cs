using MediatR;
using MySql.Data.MySqlClient;
using Stock_Backend.Application;
using Stock_Backend.Application.Handlers.ErrorLog;
using Stock_Backend.Database;
using Stock_Backend.Infrastructure;
using System.Data;

public class Program
{
    public static void Main( string[] args )
    {
        var builder = WebApplication.CreateBuilder(args);

        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // Configurações de dependências
        builder.Services.AddTransient<IDbConnection>( o => new MySqlConnection( connectionString ) );
        builder.Services.AddTransient( typeof( IPipelineBehavior<,> ), typeof( ErrorLogHandler<,> ) );

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductMovementRepository, ProductMovementRepository>();

        builder.Services.AddMediatR( o => o.RegisterServicesFromAssemblyContaining<CreateProductHandler>() );
        builder.Services.AddMediatR( o => o.RegisterServicesFromAssemblyContaining<DeleteProductHandler>() );
        builder.Services.AddMediatR( o => o.RegisterServicesFromAssemblyContaining<UpdateProductHandler>() );
        builder.Services.AddMediatR( o => o.RegisterServicesFromAssemblyContaining<GetAllProductsHandler>() );
        builder.Services.AddMediatR( o => o.RegisterServicesFromAssemblyContaining<GetProductByIdHandler>() );
        builder.Services.AddMediatR( o => o.RegisterServicesFromAssemblyContaining<DailyProductMovementHandler>() );
        builder.Services.AddMediatR( o => o.RegisterServicesFromAssemblyContaining<ProductMovementHandler>() );

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if( app.Environment.IsDevelopment() )
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        new DatabaseInitializer().InitializeDatabase();

        app.Run();
    }
}