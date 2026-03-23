# Orleans Marten providers

[![NuGet version of Interflare.Orleans.Marten.Clustering](https://img.shields.io/nuget/v/Interflare.Orleans.Marten.Clustering?label=Clustering)](https://www.nuget.org/packages/Interflare.Orleans.Marten.Clustering/)
[![NuGet version of Interflare.Orleans.Marten.Persistence](https://img.shields.io/nuget/v/Interflare.Orleans.Marten.Persistence?label=Persistence)](https://www.nuget.org/packages/Interflare.Orleans.Marten.Persistence/)
[![NuGet version of Interflare.Orleans.Marten.Reminders](https://img.shields.io/nuget/v/Interflare.Orleans.Marten.Reminders?label=Reminders)](https://www.nuget.org/packages/Interflare.Orleans.Marten.Reminders/)   

[![Status of CI workflow](https://github.com/interflare/orleans-marten/actions/workflows/ci.yml/badge.svg)](https://github.com/interflare/orleans-marten/actions/workflows/ci.yml)
[![Status of Release workflow](https://github.com/interflare/orleans-marten/actions/workflows/release.yml/badge.svg)](https://github.com/interflare/orleans-marten/actions/workflows/release.yml)
[![Latest release version](https://img.shields.io/github/v/release/interflare/orleans-marten?label=Latest)](https://github.com/interflare/orleans-marten/releases/latest)

A [Marten](https://martendb.io/) implementation of [Orleans](https://docs.microsoft.com/dotnet/orleans) providers for **membership** (clustering), **state storage**, and **reminder
storage**. It makes use of your existing Marten project configuration and database management, and has a slim setup that's easy to get started with.

## Why this library

Orleans requires configuration of a backend storage provider in production to manage clustering and persist state. With Azure services, this is relatively easy to get set started
through Azure Table Storage - you can just point your cluster at the service, and you've got a production-ready system.

If you're not hosting in Azure, the storage options are not as easy to get started with. The ADO .NET (SQL) provider, for example, requires setting up the database tables manually
using loose SQL scripts you have to download and execute from several different places in the Orleans git repo.

This library is for applications which already use Marten as a data store in some way, or intend to use it - meaning that there is no additional or separate tooling to configure
just to run Orleans. The library automatically extends and uses your existing Marten config with tables for running Orleans.

## Installation

This repo publishes a package for each of the supported systems for Orleans, which you can add to your application as needed - install only the packages for the corresponding
service you wish to back with Marten.

- **Membership**: `dotnet add package Interflare.Orleans.Marten.Clustering`
- **State storage**: `dotnet add package Interflare.Orleans.Marten.Persistence`
- **Reminders**: `dotnet add package Interflare.Orleans.Marten.Reminders`

## Configuration

There isn't much to configure, you only need to tell the Orleans silo to use the providers:

```csharp
// Program.cs

var builder = WebApplication.CreateBuilder(args);

// Setup Orleans silo
builder.Host.UseOrleans(siloBuilder =>
{
    // Interflare.Orleans.Marten.Clustering:
    siloBuilder.UseMartenClustering();

    // Interflare.Orleans.Marten.Reminders:
    siloBuilder.UseMartenReminderService();

    // Interflare.Orleans.Marten.Persistence:
    siloBuilder.AddMartenGrainStorageAsDefault(); // or
    siloBuilder.AddMartenGrainStorage(name: "MyStorageName");
});


// Example Marten configuration
// see: https://martendb.io/configuration/hostbuilder.html
builder.Services.AddMarten(options =>
    {
        options.Connection(configuration.GetConnectionString("MyDatabase"));
    
        options.Schema.For<MyDocument1>();
        options.Schema.For<MyDocument2>();
    });
```

> [!IMPORTANT]   
> You will need to perform [code generation](https://martendb.io/configuration/prebuilding.html#pre-building-generated-types) (if needed)
> and [run migrations](https://martendb.io/schema/migrations.html) on first use of these packages as well as any subsequent updates - as you would if you were creating and updating
> new tables of your own.

The Orleans provider configuration methods (`UseMartenClustering()` etc) will inject their Marten schema definitions into your existing configuration.

If you follow the Orleans recommendation to [co-host](https://learn.microsoft.com/en-us/dotnet/orleans/host/client) your clients in the same process as the silos, you are ready to
hit deploy and get going. These Marten providers will use the Orleans configuration for the [
`ClusterOptions`](https://learn.microsoft.com/en-us/dotnet/orleans/host/configuration-guide/server-configuration?pivots=orleans-7-0#orleans-clustering-information) (service/cluster
ids) as well as the serializers.

**Should you be hosting your clients external to the silos in separate projects**, you can configure the client projects with the `Interflare.Orleans.Marten.Clustering` library
like so:

```csharp
// Program.cs

var builder = WebApplication.CreateBuilder(args);

// Setup Orleans client
builder.Host.UseOrleansClient(clientBuilder =>
{
    // Interflare.Orleans.Marten.Clustering:
    clientBuilder.UseMartenClustering();
});

// You still need to configure Marten in your client projects
builder.Services.AddMarten(options =>
    {
        // ...
    });
```
