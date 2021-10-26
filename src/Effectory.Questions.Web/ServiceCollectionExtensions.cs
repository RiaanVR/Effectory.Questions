using Effectory.Questions.Adapters;
using Effectory.Questions.Core.Interfaces;
using Effectory.Questions.CrossCutting;
using Microsoft.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IQuestionRepository, EntityFrameworkQuestionRepository>()
            .AddDbContext<QuestionsDbContext>(config =>
            {
                config.UseInMemoryDatabase(nameof(QuestionsDbContext));
            })
            .AddStartupTask();
    }

    private static IServiceCollection AddStartupTask(this IServiceCollection services)
    {
        return services
            .Scan(scan => scan
                .FromApplicationDependencies()
                .AddClasses(classes => classes
                    .AssignableTo<IStartupTask>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());
    }


}

public static class WebApplicationExtensions
{
    public static async Task RunStartupTasks(this WebApplication webApplication)
    {
        using (var scope = webApplication.Services.CreateScope())
        {
            var allStartupTasks = scope.ServiceProvider.GetServices<IStartupTask>();
            await Task.WhenAll(allStartupTasks.Select(st => st.Execute()));
        }
    }
}

