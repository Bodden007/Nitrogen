using System;

namespace Nitrogen.Mocks;

public readonly record struct RxValue(
    float Value,
    string Name,
    string Unit);

public interface ICoreLinkSource
{
    IObservable<RxValue> Value(ValueId id);
}