using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

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

	/// <summary>
	/// Токен, отвечающий за авторизацию. Без него вы не имеете доступа к любым действиям (по идеи)
	/// </summary>
	public static string Token
	{
		get; set;
	}

	/// <summary>
	/// Херня чтобы не регаться постоянно и делать красивый интерфейс)
	/// <para>0 - nothing</para>
	/// <para>1 - skip auth</para>
	/// </summary>
	public const int DebugCode = 0;
}
