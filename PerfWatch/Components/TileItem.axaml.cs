using Avalonia.Controls;
using Avalonia.Input;
using System;

namespace PerfWatch.Components;

public class TileItem : ContentControl
{
    protected override Type StyleKeyOverride => typeof(TileItem);
    // �������¼�
    public event EventHandler? Click;

    public TileItem()
    {
        this.PointerPressed += OnPointerPressed;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}