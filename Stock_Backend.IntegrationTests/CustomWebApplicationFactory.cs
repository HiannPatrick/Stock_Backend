﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Stock_Backend.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> :WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHost CreateHost( IHostBuilder builder )
        {
            builder.UseEnvironment( "Testing" );

            return base.CreateHost( builder );
        }
    }
}
