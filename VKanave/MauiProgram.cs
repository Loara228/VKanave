using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using VKanave.Networking;

namespace VKanave;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}

	public static bool DisplayNetworkError = false;
	public const bool LOCAL = false;
	public const string IP_ADDRESS = "45.155.207.150";
}
