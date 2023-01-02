﻿// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wholesaler.Frontend.DataAccess;
using Wholesaler.Frontend.Domain;
using Wholesaler.Frontend.Presentation.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views;

var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddTransient<IUserService, WholesalerClient>();
    services.AddTransient<ILoginView, LoginView>();
    services.AddTransient<IMenuView, MenuView>();
    services.AddSingleton<ApplicationState>();
    });

var app = host.Build();

var login = ActivatorUtilities.CreateInstance<LoginView>(app.Services);
await login.Render();


