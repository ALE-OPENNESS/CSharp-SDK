# Getting Started with the C# o2g-sdk

This guide walks you through everything you need — from installing the required
tools to successfully logging in to an O2G server — with no prior .NET
experience required.

---

## What you will need

- A computer running **Windows**, **macOS**, or **Linux**
- Access to an **O2G server** (hostname or IP address, login name and password)
- An internet connection

---

## Step 1 — Install the .NET SDK

The .NET SDK is the toolkit that allows you to build and run C# programs.

1. Go to [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2. Download the **.NET 8 SDK** (Long Term Support — recommended)
3. Run the installer and follow the on-screen instructions
4. Verify the installation by opening a terminal and running:

```bash
dotnet --version
```

The command should print a version number (e.g. `8.0.100`).

> **Opening a terminal:**
> - **Windows**: press `Win + R`, type `cmd`, press Enter
> - **macOS**: press `Cmd + Space`, type `Terminal`, press Enter
> - **Linux**: press `Ctrl + Alt + T`

---

## Step 2 — Install an IDE (optional but recommended)

An IDE makes writing C# much easier with code completion, error highlighting
and project management.

**Option A — Visual Studio (Windows only)**
1. Go to [https://visualstudio.microsoft.com](https://visualstudio.microsoft.com)
2. Download the free **Community Edition**
3. During installation, select the **.NET desktop development** workload
4. Launch Visual Studio

**Option B — Visual Studio Code (all platforms)**
1. Go to [https://code.visualstudio.com](https://code.visualstudio.com)
2. Download and install VS Code
3. Open VS Code and install the **C# Dev Kit** extension from the Extensions panel

---

## Step 3 — Create your project

Open a terminal and run:

```bash
dotnet new console -n MyO2GApp
cd MyO2GApp
```

This creates a minimal console project. You should see a `Program.cs` file
and a `MyO2GApp.csproj` file in the folder.

---

## Step 4 — Add the O2G SDK package

Still in the `MyO2GApp` folder, run:

```bash
dotnet add package o2g-sdk --version 2.0.0
```

This downloads the SDK and adds it as a dependency to your project.
You can verify it was added by opening `MyO2GApp.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="o2g-sdk" Version="2.0.0" />
  </ItemGroup>
</Project>
```

---

## Step 5 — Write your first program

Open `Program.cs` and replace its content with the following:

```csharp
using o2g;
using o2g.Types;
using o2g.Utility;

HttpClientBuilder.DisableSSValidation = true; // remove in production

O2G.Application app = new("MyFirstApp");

app.SetHost(new Host
{
    PrivateAddress = "YOUR_O2G_SERVER_ADDRESS",
    PublicAddress  = "YOUR_O2G_SERVER_ADDRESS"
});

Console.WriteLine("Connecting to O2G server...");

try
{
    await app.LoginAsync("YOUR_LOGIN", "YOUR_PASSWORD");
    Console.WriteLine("Login successful!");

    // The SDK is now ready — you can call any service here

    Console.WriteLine("Press any key to exit.");
    Console.ReadKey(intercept: true);
}
catch (O2GException ex)
{
    Console.WriteLine($"Login failed: {ex.Message}");
}
finally
{
    await app.ShutdownAsync();
    Console.WriteLine("Logged out.");
}
```

Replace the following placeholders:
- `YOUR_O2G_SERVER_ADDRESS` — the hostname or IP address of your O2G server
  (e.g. `192.168.1.100`)
- `YOUR_LOGIN` — your O2G user login name
- `YOUR_PASSWORD` — your O2G user password

---

## Step 6 — Run your program

In the terminal, inside the `MyO2GApp` folder, run:

```bash
dotnet run
```

If everything is configured correctly, you should see:

```
Connecting to O2G server...
Login successful!
Press any key to exit.
```

> **Note:** If the O2G server is not yet reachable when you start the program,
> the SDK will automatically retry every 5 seconds until it connects.
> Start the server and the program will connect automatically.

---

## Step 7 — Add session monitoring (recommended)

In production, you should add a monitoring policy to be notified when the
session is lost and recovered. Add a new file `MyMonitoringPolicy.cs` to
your project:

```csharp
using o2g;

class MyMonitoringPolicy : SessionMonitoringPolicy
{
    public override void OnSessionLost(string reason)
    {
        Console.WriteLine($"Session lost ({reason}) — SDK is recovering...");
    }

    public override void OnSessionRecovered()
    {
        Console.WriteLine("Session recovered — back online.");
    }

    public override Behavior OnConnectFailure(Exception e)
    {
        Console.WriteLine($"Connection failed: {e.Message} — retrying in 5s...");
        return Behavior.RetryAfter(5_000);
    }
}
```

Then set it before calling `LoginAsync` in `Program.cs`:

```csharp
app.SetSessionMonitoringPolicy(new MyMonitoringPolicy());
await app.LoginAsync("YOUR_LOGIN", "YOUR_PASSWORD");
```

---

## Troubleshooting

**`Login failed`**
- Double-check the server address, login name and password
- Make sure the O2G server is reachable from your computer
- If the server uses a self-signed SSL certificate, add the following line
  **before** `LoginAsync` for testing:
  ```csharp
  HttpClientBuilder.DisableSSValidation = true;
  ```
  Remove this in production.

**`Could not resolve package`**
- Make sure you have an internet connection
- Try running `dotnet restore` to re-download dependencies
- Check that the version in your `.csproj` matches the latest published version

**Program keeps retrying and never connects**
- The SDK retries automatically when the server is unreachable — this is
  normal behaviour. Check that the server address is correct and the server
  is running.
- To abort immediately instead of retrying, override `OnConnectFailure` in
  your monitoring policy and return `Behavior.Abort()`.

---

## What's next?

Once logged in, you can start using the SDK services through the `app`
object. Here are a few examples:

### Make a phone call

```csharp
await app.TelephonyService.MakeCallAsync("myDeviceNumber", "1234");
```

### Get your active calls

```csharp
var calls = await app.TelephonyService.GetCallsAsync();
Console.WriteLine($"Active calls: {calls?.Count ?? 0}");
```

### Subscribe to telephony events and listen for incoming calls

```csharp
app.TelephonyService.CallCreated += (_, e) =>
    Console.WriteLine($"New call: {e.Event.CallRef}");

Subscription subscription = Subscription.Builder
    .AddTelephonyEvents(["*"])
    .SetTimeout(10)
    .Build();

await app.SubscribeAsync(subscription);
```

### Configure geographic HA

```csharp
app.SetHost(
    new Host { PrivateAddress = "10.0.0.1" },
    new Host { PrivateAddress = "10.0.0.2" });
```

For a full list of available services and methods, see the
[README](README.md) and the
[O2G REST API Reference](https://api.dspp.al-enterprise.com/o2g/).
