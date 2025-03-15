using Avalonia.Controls;
using Avalonia.Input;
using System;

namespace PerfWatch.Components;

public class TileItem : ContentControl
{
    protected override Type StyleKeyOverride => typeof(TileItem);
    // 定义点击事件
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