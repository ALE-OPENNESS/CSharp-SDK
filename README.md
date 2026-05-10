# o2g-sdk

A C# SDK for the ALE International O2G (OmniPCX OpenTouch Gateway) platform, providing a comprehensive API for telephony, call control, management and contact center services.

## Requirements

- .NET 8 or later
- An OmniPCX Enterprise node connected to an O2G server
- An O2G API license appropriate for the services you intend to use

## Getting Started

New to .NET or O2G? Follow the [Getting Started guide](GETTING_STARTED.md)
for a complete step-by-step walkthrough from installing the tools to your first login.

## Installation

The o2g C# SDK is available on [NuGet](https://www.nuget.org/packages/o2g-sdk).

```bash
dotnet add package o2g-sdk --version 2.0.0
```

Or add the following to your `.csproj` file:

```xml
<PackageReference Include="o2g-sdk" Version="2.0.0" />
```

## Quick Start

```csharp
using o2g;
using o2g.Types;

// 1. Create the application and configure the O2G server
O2G.Application app = new("MyApplication");
app.SetHost(new Host
{
    PrivateAddress = "192.168.1.100",
    PublicAddress  = "192.168.1.100"
});

// 2. Optionally set a custom monitoring policy
app.SetSessionMonitoringPolicy(new MyMonitoringPolicy());

// 3. Login — retries automatically if the server is not yet reachable
await app.LoginAsync("loginName", "password");

// 4. Subscribe to events
Subscription subscription = Subscription.Builder
    .AddTelephonyEvents(["*"])
    .AddRoutingEvents()
    .SetTimeout(10)
    .Build();

app.TelephonyService.CallCreated += (_, e) =>
    Console.WriteLine($"New call: {e.Event.CallRef}");

await app.SubscribeAsync(subscription);

// 5. Use a service
await app.TelephonyService.MakeCallAsync("1234", "5678");

// 6. Shutdown when done
await app.ShutdownAsync();
```

## What's New in 2.0.1

- Add new field `EmailAddress` in object `User`

## Server Configuration

Use `SetHost` to configure the O2G server topology. Three deployment
configurations are supported.

### Standalone server

```csharp
app.SetHost(new Host { PrivateAddress = "10.0.0.1" });
```

### Local HA (virtual IP)

Two O2G server instances sharing the same virtual IP address or URL.
Configure it exactly like a standalone server — the virtual IP routes
transparently to whichever node is active:

```csharp
app.SetHost(new Host { PrivateAddress = "vip.example.com" });
```

### Geographic HA (two distinct hosts)

Two O2G server instances at different locations with distinct IP addresses.
On primary failure, the SDK switches immediately to the secondary and stays
there permanently:

```csharp
app.SetHost(
    new Host { PrivateAddress = "10.0.0.1" },
    new Host { PrivateAddress = "10.0.0.2" });
```

A `Host` can be configured with both a private and a public address:

```csharp
new Host { PrivateAddress = "10.0.0.1", PublicAddress = "93.12.1.1" }
```

The SDK tries the private address first, then falls back to the public address.

## Session Monitoring and Recovery

The SDK automatically handles session failures and recovery. When the O2G
server crashes or becomes unreachable, the SDK:

1. Detects the failure via the chunk stream or keep-alive
2. Notifies the application via `OnSessionLost`
3. Retries the connection
4. Switches to the secondary server if geographic HA is configured
5. Re-subscribes to events after recovery
6. Notifies the application via `OnSessionRecovered`

### Custom monitoring policy

Extend `SessionMonitoringPolicy` to control SDK behaviour and receive
notifications:

```csharp
using o2g;

class MyMonitoringPolicy : SessionMonitoringPolicy
{
    public override void OnSessionLost(string reason)
    {
        Console.WriteLine($"Session lost ({reason}) — SDK is recovering...");
        // Update your UI to show a reconnecting indicator
    }

    public override void OnSessionRecovered()
    {
        Console.WriteLine("Session recovered — back online.");
        // Resume application activity
    }

    public override Behavior OnConnectFailure(Exception e)
    {
        Console.WriteLine($"Connection failed: {e.Message} — retrying in 10s...");
        return Behavior.RetryAfter(10_000);
    }
}
```

Set the policy before calling `LoginAsync`:

```csharp
app.SetSessionMonitoringPolicy(new MyMonitoringPolicy());
await app.LoginAsync("loginName", "password");
```

### Default behaviours

| Situation | Default behaviour |
|---|---|
| Initial connection fails | Retry after 5 seconds |
| Chunk network error | Abort → trigger recovery |
| Keep-alive network error | Abort → trigger recovery |
| Keep-alive rejected by server | Trigger recovery |
| Server switched to secondary | Permanent — never switches back |

## Event Subscription

### Chunk eventing (default)

The SDK opens an outgoing HTTPS connection to the O2G server and receives
events as a stream. No server-side endpoint is required from the application.

```csharp
Subscription subscription = Subscription.Builder
    .AddTelephonyEvents(["*"])
    .AddRoutingEvents()
    .SetTimeout(10)
    .Build();

await app.SubscribeAsync(subscription);
```

### Webhook eventing

The O2G server sends events via HTTP POST to a URL provided by the application.
The application must expose an HTTPS endpoint and implement the `IWebHook`
interface:

```csharp
using o2g;

class MyWebHook : IWebHook
{
    private IEventProcessor _processor;
    public string Url => "https://myapp.example.com/o2g/events";

    public void ConnectProcessor(IEventProcessor processor)
    {
        _processor = processor;
        // Start your HTTP server here and call _processor.Process(rawBody)
        // from your POST handler
    }
}

Subscription subscription = Subscription.Builder
    .AddMaintenanceEvents()
    .SetWebHook(new MyWebHook())
    .Build();

await app.SubscribeAsync(subscription);
```

The SDK provides no built-in HTTP server — the application is responsible
for exposing the endpoint. See the [samples](samples/) directory for a
complete working example using `HttpListener`.

## Services

| Service property | Description | License Required |
|---|---|---|
| `TelephonyService` | Call control, transfer, conference, recording | `TELEPHONY_ADVANCED` |
| `RoutingService` | Forward, overflow, Do Not Disturb | `TELEPHONY_ADVANCED` |
| `CommunicationLogService` | Communication history records | `TELEPHONY_ADVANCED` |
| `MessagingService` | Voicemail and mailbox management | `TELEPHONY_ADVANCED` |
| `DirectoryService` | Enterprise directory search | `TELEPHONY_ADVANCED` |
| `EventSummaryService` | Missed calls, voicemail counters | `TELEPHONY_ADVANCED` |
| `UsersService` | User profile and preferences | — |
| `CallCenterAgentService` | CCD agent state and skills | `CONTACTCENTER_AGENT` |
| `CallCenterPilotService` | CCD pilot monitoring | `CONTACTCENTER_SVCS` |
| `CallCenterRealtimeService` | Real-time ACD statistics | `CONTACTCENTER_SVCS` |
| `CallCenterStatisticsService` | Historical ACD statistics | `CONTACTCENTER_SVCS` |
| `CallCenterManagementService` | CCD pilot and calendar management | `CONTACTCENTER_SVCS` |
| `MaintenanceService` | System status and PBX health | — |
| `PbxManagementService` | PBX object model management | `MANAGEMENT` |
| `PhoneSetProgrammingService` | Phone device keys and settings | — |
| `UserManagementService` | Administrator user management | — |
| `AnalyticsService` | Charging and incident data | — |

## Examples

### Forward calls to voicemail when busy

```csharp
using o2g.Types.Routing;

await app.RoutingService.ForwardOnVoiceMailAsync(Forward.ForwardCondition.Busy);
```

### Transfer a call

```csharp
// Supervised transfer
await app.TelephonyService.TransferAsync(activeCallRef, heldCallRef);

// Blind transfer
await app.TelephonyService.BlindTransferAsync(callRef, "12002");
```

### Monitor a CCD pilot

```csharp
app.CallCenterPilotService.PilotCallCreated += (_, e) =>
    Console.WriteLine($"New call on pilot: {e.Event.CallRef}");

Subscription subscription = Subscription.Builder
    .AddCallCenterPilotEvents(["60141"])
    .Build();

await app.SubscribeAsync(subscription);
await app.CallCenterPilotService.MonitorStartAsync("60141");
```

### Search the directory

```csharp
using o2g.Types.Directory;

var criteria = Criteria.Create(AttributeFilter.LastName, OperationFilter.BeginsWith, "doe");
await app.DirectoryService.SearchAsync(criteria);

bool finished = false;
while (!finished)
{
    SearchResult result = await app.DirectoryService.GetResultsAsync();
    if (result?.ResultCode == SearchResult.Code.Ok)
    {
        foreach (var item in result.ResultElements)
            foreach (var contact in item.Contacts)
                Console.WriteLine($"{contact.FirstName} {contact.LastName}");
    }
    else if (result?.ResultCode == SearchResult.Code.Finish)
        finished = true;
    else
        await Task.Delay(500);
}
```

### Query communication log records

```csharp
using o2g.Types.CommunicationLog;

var filter = QueryFilter.Builder
    .SetAfterDate(new DateTime(2026, 1, 1))
    .SetBeforeDate(new DateTime(2026, 1, 31))
    .SetOptions([Option.Unanswered])
    .Build();

var result = await app.CommunicationLogService.GetComRecordsAsync(filter);
Console.WriteLine($"Total records: {result?.TotalCount}");
```

### Manage a CCD agent

```csharp
await app.CallCenterAgentService.LogonAsync("oxe12000", "30000");
await app.CallCenterAgentService.SetReadyAsync();
```

### Configure geographic HA

```csharp
app.SetHost(
    new Host { PrivateAddress = "10.0.0.1" },
    new Host { PrivateAddress = "10.0.0.2" });

// The SDK switches permanently to the secondary host if the primary becomes
// unreachable. OnSessionLost / OnSessionRecovered in your policy are called.
```

## Logging

The SDK uses [NLog](https://nlog-project.org/) for internal logging.
Add an `NLog.config` file to your project to control verbosity:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <targets>
    <target name="console" xsi:type="Console"
            layout="${time} [${level}] ${logger:shortName=true} — ${message} ${exception}" />
  </targets>

  <rules>
    <!-- SDK internal logging -->
    <logger name="o2g.Internal.*" minlevel="Info" writeTo="console" />

    <!-- Uncomment for detailed recovery and failover traces -->
    <!-- <logger name="o2g.Internal.SessionImpl" minlevel="Trace" writeTo="console" /> -->
    <!-- <logger name="o2g.Internal.Events.ChunkEventListener" minlevel="Trace" writeTo="console" /> -->

    <logger name="*" minlevel="Info" writeTo="console" />
  </rules>
</nlog>
```

## API Reference

- [O2G REST API Reference](https://api.dspp.al-enterprise.com/o2g/)

## Versioning

This SDK follows the O2G API version it targets:

- **Major**: O2G API major version (currently 2)
- **Minor**: O2G API patch version (currently 7.5 → 0)
- **Patch**: SDK release number 

For example, `2.0.x` targets O2G API version 2.7.5.

## License

Copyright 2026 ALE International

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
