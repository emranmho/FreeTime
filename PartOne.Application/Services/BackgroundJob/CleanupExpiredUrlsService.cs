using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PartOne.Application.Common.Interfaces;
using PartOne.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PartOne.Application.Services.BackgroundJob
{
    public class CleanupExpiredUrlsService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeDactory;
        private Timer _timer;

        public CleanupExpiredUrlsService(IServiceScopeFactory scopeFactory)
        {
            _scopeDactory = scopeFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using var scope = _scopeDactory.CreateScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var shortenedUrlService = scope.ServiceProvider.GetRequiredService<IShortenedUrlService>();

            await shortenedUrlService.DeleteExpireUrls();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }


        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
