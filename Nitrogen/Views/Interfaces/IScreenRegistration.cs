using Nitrogen.Views.Navigation;

namespace Nitrogen.Views.Interfaces;

/// <summary>
/// Контракт подключения прикладного экрана.
/// </summary>
internal interface IScreenRegistration
{
    bool Open(ScreenContext context);
}