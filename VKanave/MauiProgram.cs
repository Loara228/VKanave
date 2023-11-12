using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using VKanave.Networking;

namespace VKanave;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		if (DebugCode == 2)
		{
			LocalUser.NewUser("test", "test", 0);
		}
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

    /// <summary>
    /// Херня чтобы не регаться постоянно и делать красивый интерфейс)
    /// <para>0 - nothing</para>
    /// <para>1 - skip connection</para>
	/// <para>2 - skip auth</para>
    /// </summary>
    public const int DebugCode = 0;
}
